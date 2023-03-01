namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class TimerHandler
    {
        public TimerHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<LatchHolder>(latch =>
                {
                    latch.Latch?.Reset();
                },
                DependencyNames.TimerLatch);
            string currentIP = null;
            r?.Run<
                HttpClientFactory,
                GlobalSettingsHolder,
                Messages>(
                (factory, settings, messages) =>
                {
                    var cantReadIpMessage = messages.CantReadIp;
                    using (var hc = factory.Create())
                    {
                        hc.Timeout = System.TimeSpan.FromMilliseconds(10000);
                        Task<string> currentIpTask;
                        try
                        {
                            currentIpTask = hc.GetStringAsync(
                                settings.HttpExternalIpProviderUri);
                            currentIpTask.Wait();
                        }
                        catch
                        {
                            currentIP = cantReadIpMessage;
                            goto setCurrentIP;
                        }

                        currentIP = currentIpTask.Result?.Trim();
                    }

                    setCurrentIP:
                    r.Run<UiReaderWriter>(uiRw =>
                    {
                        uiRw.Write(
                            ui,
                            () =>
                            {
                                ui.CurrentIP = currentIP;
                            });
                    });
                });

            r?.Run<
                HttpClientFactory,
                GlobalSettingsHolder,
                TimeProvider,
                Messages>(
                (factory, settings, provider, messages) =>
                {
                    var ipTypeUnknownMessage = messages.IpTypeUnknown;
                    var errorReadingFromDnsMessage = messages.ErrorReadingFromDns;
                    var errorSyncingMessage = messages.ErrorSyncing;
                    var cantReadIpMessage = messages.CantReadIp;
                    string syncedIP;
                    IPAddress currentAddress;
                    bool aaaa;
                    System.DateTime lastChecked;
                    if (IPAddress.TryParse(currentIP, out currentAddress))
                    {
                        aaaa = currentAddress
                                   .AddressFamily ==
                               AddressFamily.InterNetworkV6;
                        goto buildUri;
                    }

                    syncedIP = ipTypeUnknownMessage;
                    lastChecked = provider.Now();
                    goto setSyncedIP;

                    buildUri:
                    var uri = new StringBuilder()
                        .Append(@"https://api.godaddy.com/v1/domains/")
                        .Append(settings.Domain)
                        .Append(@"/records/")
                        .Append(aaaa ? @"AAAA" : @"A")
                        .Append('/')
                        .Append(settings.Subdomain)
                        .ToString();

                    var hc = factory.CreateGoDaddy();
                    if (hc == null)
                    {
                        return;
                    }

                    using (hc)
                    {
                        hc.Timeout = System.TimeSpan.FromSeconds(12);
                        Record record;
                        var task = hc.GetAsync(uri);
                        bool syncedByService = falsity;
                        try
                        {
                            task.Wait();
                        }
                        catch
                        {
                            syncedIP = errorReadingFromDnsMessage;
                            lastChecked = provider.Now();
                            goto setSyncedIP;
                        }

                        var response = task.Result;
                        if (response == null || !response.IsSuccessStatusCode)
                        {
                            lastChecked = provider.Now();
                            goto error;
                        }

                        var json = response
                            .Content
                            .ReadAsStringAsync()
                            .Result;
                        var records = JsonConvert.DeserializeObject<ICollection<Record>>(
                            json);
                        lastChecked = provider.Now();
                        if (records.Count < 1)
                        {
                            record = new Record
                            {
                                data = currentIP,
                                name = settings.Subdomain,
                                ttl = 3600,
                                type = aaaa ? @"AAAA" : @"A"
                            };
                            records.Add(record);
                            syncedIP = new StringBuilder()
                                .Append(@"No ")
                                .Append(aaaa ? @"AAAA" : @"A")
                                .Append(@" record found for this subdomain.")
                                .ToString();
                            goto checkSync;
                        }

                        record = EnumerableHelpers.First(
                            records);
                        syncedIP = record.data;
                        if (syncedIP == currentIP)
                        {
                            var lsip = this.lastSyncedIP;
                            if (lsip != currentIP && lsip != default)
                            {
                                syncedByService = truth;
                                goto afterSync;
                            }

                            goto setSyncedIP;
                        }

                        if (currentIP == cantReadIpMessage)
                        {
                            goto setSyncedIP;
                        }

                        checkSync:
                        var shouldSync = false;
                        r.Run<UiReaderWriter>(uiRw =>
                        {
                            shouldSync = uiRw.Read(
                                ui,
                                () => ui.StartSyncingKeyDisabled);
                        });

                        if (!shouldSync)
                        {
                            goto setSyncedIP;
                        }

                        // sync the ip
                        record.data = currentIP;
                        json = JsonConvert.SerializeObject(records);
                        var content = new StringContent(
                            json,
                            System.Text.Encoding.UTF8,
                            @"application/json");
                        var syncTask = hc.PutAsync(
                            uri,
                            content);
                        if (syncTask == null)
                        {
                            return;
                        }

                        try
                        {
                            syncTask.Wait();
                        }
                        catch (System.Exception ex)
                        {
                            syncedIP = errorSyncingMessage
                                + ex.InnerException?.GetType()
                                + ": "
                                + ex.InnerException?.Message;
                            goto setSyncedIP;
                        }

                        response = syncTask.Result;
                        if (response == null)
                        {
                            return;
                        }

                        afterSync:
                        if (syncedByService || response.IsSuccessStatusCode)
                        {
                            var lastSynced = provider
                                .Now()
                                .ToString(CultureInfo.CurrentUICulture);
                            if (syncedByService)
                            {
                                lastSynced += System.Environment.NewLine;
                                lastSynced += settings.ServiceAttribution;
                            }

                            r.Run<UiReaderWriter>(uiRw =>
                            {
                                uiRw.Write(
                                    ui,
                                    () =>
                                    {
                                        ui.LastSynced = lastSynced;
                                    });
                            });
                            syncedIP = currentIP;
                            goto setSyncedIP;
                        }

                        error:
                        r.Run<EventRaiser>(er =>
                        {
                            er.Raise(
                                ui,
                                nameof(ui.StopSyncingKeyTapped));
                        });

                        syncedIP = errorReadingFromDnsMessage;
                    }

                    setSyncedIP:
                    r.Run<UiReaderWriter>(uiRW =>
                    {
                        var lastCheckedString = lastChecked.ToString(
                            CultureInfo.CurrentUICulture);
                        uiRW.Write(
                            ui,
                            () =>
                            {
                                ui.SyncedIP = syncedIP;
                                ui.LastChecked = lastCheckedString;
                            });
                    });

                    if (syncedIP == ipTypeUnknownMessage)
                    {
                        return;
                    }

                    if (syncedIP == errorReadingFromDnsMessage)
                    {
                        return;
                    }

                    if (syncedIP?.Contains(errorSyncingMessage) 
                        ?? truth)
                    {
                        return;
                    }

                    this.setLastSyncedIP(syncedIP);
                });

            r?.Run<ServiceChecker, UiReaderWriter>(
                (checker, uiRw) =>
            {
                var exists = checker.ServiceExists();
                uiRw.Write(
                    ui,
                    () =>
                    {
                        ui.ServiceInstalled = exists;
                    });
            });

            r?.Run<LatchHolder>(latch =>
                {
                    latch.Latch?.Set();
                },
                DependencyNames.TimerLatch);
        }

        protected virtual void setLastSyncedIP(
            string lastSyncedIP)
        {
            this.lastSyncedIP = lastSyncedIP;
        }

        protected const bool
            truth = true,
            falsity = false;
        protected string lastSyncedIP;
        protected readonly MethodRunner runner;
    }
}
