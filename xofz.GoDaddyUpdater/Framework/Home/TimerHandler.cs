namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<LatchHolder>(latch =>
                {
                    latch.Latch.Reset();
                },
                DependencyNames.TimerLatch);
            string currentIP = null;
            w.Run<
                HttpClientFactory,
                GlobalSettingsHolder,
                Messages>(
                (factory, settings, messages) =>
                {
                    var cantReadIpMessage = messages.CantReadIp;
                    using (var hc = factory.Create())
                    {
                        hc.Timeout = TimeSpan.FromMilliseconds(10000);
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
                    w.Run<UiReaderWriter>(uiRw =>
                    {
                        uiRw.Write(
                            ui,
                            () => ui.CurrentIP = currentIP);
                    });
                });

            w.Run<
                HttpClientFactory,
                GlobalSettingsHolder,
                Messages>(
                (factory, settings, messages) =>
                {
                    var ipTypeUnknownMessage = messages.IpTypeUnknown;
                    var errorReadingFromDnsMessage = messages.ErrorReadingFromDns;
                    var errorSyncingMessage = messages.ErrorSyncing;
                    var cantReadIpMessage = messages.CantReadIp;
                    string syncedIP;
                    IPAddress currentAddress;
                    bool aaaa;
                    DateTime lastChecked;
                    if (IPAddress.TryParse(currentIP, out currentAddress))
                    {
                        aaaa = currentAddress
                            .AddressFamily ==
                            AddressFamily.InterNetworkV6;
                        goto buildUri;
                    }

                    syncedIP = ipTypeUnknownMessage;
                    lastChecked = DateTime.Now;
                    goto setSyncedIP;

                    buildUri:
                    var uri = new StringBuilder()
                        .Append(@"https://api.godaddy.com/v1/domains/")
                        .Append(settings.Domain)
                        .Append(@"/records/")
                        .Append(aaaa ? "AAAA" : "A")
                        .Append('/')
                        .Append(settings.Subdomain)
                        .ToString();

                    using (var hc = factory.CreateGoDaddy())
                    {
                        hc.Timeout = TimeSpan.FromSeconds(12);
                        Record record;
                        var task = hc.GetAsync(uri);
                        bool syncedByService = false;
                        try
                        {
                            task.Wait();
                        }
                        catch
                        {
                            syncedIP = errorReadingFromDnsMessage;
                            lastChecked = DateTime.Now;
                            goto setSyncedIP;
                        }

                        var response = task.Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            lastChecked = DateTime.Now;
                            goto error;
                        }

                        var json = response
                            .Content
                            .ReadAsStringAsync()
                            .Result;
                        var records = JsonConvert.DeserializeObject<ICollection<Record>>(
                            json);
                        lastChecked = DateTime.Now;
                        if (records.Count == 0)
                        {
                            record = new Record
                            {
                                data = currentIP,
                                name = settings.Subdomain,
                                ttl = 3600,
                                type = aaaa ? "AAAA" : "A"
                            };
                            records.Add(record);
                            syncedIP = new StringBuilder()
                                .Append("No ")
                                .Append(aaaa ? "AAAA" : "A")
                                .Append(" record found for this subdomain.")
                                .ToString();
                            goto checkSync;
                        }

                        record = records.First();
                        syncedIP = record.data;
                        if (syncedIP == currentIP)
                        {
                            var lsip = this.lastSyncedIP;
                            if (lsip != currentIP && lsip != default(string))
                            {
                                syncedByService = true;
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
                        w.Run<UiReaderWriter>(uiRw =>
                        {
                            shouldSync = uiRw.Read(
                                ui,
                                () => ui.StopSyncingKeyEnabled);
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
                            "application/json");
                        var syncTask = hc.PutAsync(
                            uri,
                            content);
                        try
                        {
                            syncTask.Wait();
                        }
                        catch (Exception ex)
                        {
                            syncedIP = errorSyncingMessage
                                + ex.InnerException?.GetType()
                                + ": "
                                + ex.InnerException?.Message;
                            goto setSyncedIP;
                        }

                        response = syncTask.Result;

                        afterSync:
                        if (syncedByService || response.IsSuccessStatusCode)
                        {
                            var lastSynced = DateTime
                                .Now
                                .ToString(CultureInfo.CurrentUICulture);
                            if (syncedByService)
                            {
                                lastSynced += Environment.NewLine;
                                lastSynced += settings.ServiceAttribution;
                            }

                            w.Run<UiReaderWriter>(uiRw =>
                            {
                                uiRw.Write(
                                    ui,
                                    () => ui.LastSynced = lastSynced);
                            });
                            syncedIP = currentIP;
                            goto setSyncedIP;
                        }

                        error:
                        w.Run<EventRaiser>(er =>
                        {
                            er.Raise(
                                ui,
                                nameof(ui.StopSyncingKeyTapped));
                        });

                        syncedIP = errorReadingFromDnsMessage;
                    }

                    setSyncedIP:
                    var lastCheckedString = lastChecked.ToString(
                        CultureInfo.CurrentUICulture);
                    w.Run<UiReaderWriter>(uiRw =>
                    {
                        uiRw.Write(
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

                    if (syncedIP?.Contains(errorSyncingMessage) ?? true)
                    {
                        return;
                    }

                    this.setLastSyncedIP(syncedIP);
                });

            w.Run<ServiceChecker, UiReaderWriter>((checker, uiRw) =>
            {
                var exists = checker.ServiceExists();
                uiRw.Write(
                    ui,
                    () => ui.ServiceInstalled = exists);
            });

            w.Run<LatchHolder>(latch =>
                {
                    latch.Latch.Set();
                },
                DependencyNames.TimerLatch);
        }

        protected virtual void setLastSyncedIP(
            string lastSyncedIP)
        {
            this.lastSyncedIP = lastSyncedIP;
        }

        protected string lastSyncedIP;
        protected readonly MethodWeb web;
    }
}
