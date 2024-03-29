﻿namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using xofz.Framework;

    public class TimerHandler
    {
        public TimerHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle()
        {
            var r = this.runner;
            string currentIP = null;
            r?.Run<HttpClientFactory, GlobalSettingsHolder>(
                (factory, settings) =>
                {
                    using (var hc = factory.Create())
                    {
                        hc.Timeout = System.TimeSpan.FromMilliseconds(3000);
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

            r.Run<HttpClientFactory, GlobalSettingsHolder>(
                (factory, settings) =>
                {
                    string syncedIP;
                    IPAddress currentAddress;
                    bool aaaa;
                    if (IPAddress.TryParse(
                        currentIP, 
                        out currentAddress))
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
                        .Append(aaaa ? @"AAAA" : @"A")
                        .Append('/')
                        .Append(settings.Subdomain)
                        .ToString();

                    using (var hc = factory.CreateGoDaddy())
                    {
                        hc.Timeout = System.TimeSpan.FromMilliseconds(5000);
                        Record record;
                        Task<HttpResponseMessage> task;
                        try
                        {
                            task = hc.GetAsync(uri);
                            task.Wait();
                        }
                        catch
                        {
                            return;
                        }

                        var response = task.Result;
                        if (response == null || 
                            !response.IsSuccessStatusCode)
                        {
                            return;
                        }

                        var json = response
                            .Content
                            .ReadAsStringAsync()
                            .Result;
                        var records = JsonConvert
                                          .DeserializeObject<ICollection<Record>>(
                                              json)
                                      ?? new XLinkedList<Record>();
                        const byte one = 1;
                        if (records.Count < one)
                        {
                            const short defaultTtl = 3600;
                            record = new Record
                            {
                                data = currentIP,
                                name = settings.Subdomain,
                                ttl = defaultTtl,
                                type = aaaa ? @"AAAA" : @"A"
                            };
                            records.Add(record);
                            goto sync;
                        }

                        var helper = r.Run<EnumerableHelper>();
                        if (helper == null)
                        {
                            return;
                        }

                        record = helper.FirstOrNull(
                            records);
                        if (record == null)
                        {
                            return;
                        }

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
                        HttpContent content = new StringContent(
                            json,
                            System.Text.Encoding.UTF8,
                            @"application/json");
                        Task<HttpResponseMessage> syncTask;
                        try
                        {
                            syncTask = hc.PutAsync(
                                uri,
                                content);
                            syncTask.Wait();
                        }
                        catch
                        {
                            // swallow
                        }
                    }
                });
        }

        protected readonly MethodRunner runner;
    }
}
