namespace xofz.GoDaddyUpdater.Service.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using xofz.Framework;

    public partial class UpdaterService : ServiceBase
    {
        private UpdaterService()
        {
            this.InitializeComponent();
        }

        public UpdaterService(MethodWeb web)
            : this()
        {
            this.web = web;

            web.Run<xofz.Framework.Timer, EventSubscriber>(
                (t, subscriber) =>
            {
                subscriber.Subscribe(
                    t,
                    nameof(t.Elapsed),
                    this.timer_Elapsed);
            });
            web.Run<GlobalSettingsHolder>(settings =>
            {
                // must match value in ProjectInstaller ctor
                this.ServiceName = "gdu."
                    + settings.Subdomain
                    + '.'
                    + settings.Domain
                    + '.'
                    + settings
                        .HttpExternalIpProviderUri
                        .Replace('/', '-');
            });
        }

        protected override void OnStart(string[] args)
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer, EventRaiser>((t, er) =>
            {
                er.Raise(t, nameof(t.Elapsed));
            });

            w.Run<xofz.Framework.Timer>(t => t.Start(
                TimeSpan.FromMinutes(5)));
        }

        protected override void OnStop()
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t => t.Stop());
        }

        private void timer_Elapsed()
        {
            var w = this.web;
            string currentIP = null;
            w.Run<HttpClientFactory, GlobalSettingsHolder>(
                (factory, settings) =>
                {
                    using (var hc = factory.Create())
                    {
                        hc.Timeout = TimeSpan.FromMilliseconds(3000);
                        Task<string> currentIpTask;
                        try
                        {
                            currentIpTask = hc.GetStringAsync(
                                settings.HttpExternalIpProviderUri);
                            currentIpTask.Wait();
                        }
                        catch
                        {
                            return;
                        }

                        currentIP = currentIpTask.Result?.Trim();
                    }
                });
            if (currentIP == null)
            {
                // try to read current IP next time...
                return;
            }

            w.Run<HttpClientFactory, GlobalSettingsHolder>(
                (factory, settings) =>
                {
                    string syncedIP;
                    IPAddress currentAddress;
                    bool aaaa;
                    if (IPAddress.TryParse(currentIP, out currentAddress))
                    {
                        aaaa = currentAddress.AddressFamily ==
                            AddressFamily.InterNetworkV6;
                        goto afterParseIP;
                    }

                    return;

                    afterParseIP:
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
                        hc.Timeout = TimeSpan.FromMilliseconds(5000);
                        Record record;
                        var task = hc.GetAsync(uri);
                        try
                        {
                            task.Wait();
                        }
                        catch
                        {
                            return;
                        }

                        var response = task.Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            return;
                        }

                        var json = response
                            .Content
                            .ReadAsStringAsync()
                            .Result;
                        var records = JsonConvert.DeserializeObject<ICollection<Record>>(
                            json);
                        if (records.Count == 0)
                        {
                            record = new Record()
                            {
                                data = currentIP,
                                name = settings.Subdomain,
                                ttl = 3600,
                                type = aaaa ? "AAAA" : "A"
                            };
                            records.Add(record);
                            goto sync;
                        }

                        record = records.First();
                        syncedIP = record.data;
                        if (syncedIP == currentIP)
                        {
                            // still synced up!
                            return;
                        }

                        sync:
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
                        catch
                        {
                            return;
                        }
                    }
                });
        }

        private readonly MethodWeb web;
    }
}
