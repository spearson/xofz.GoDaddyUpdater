namespace xofz.GoDaddyUpdater.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using xofz.Framework;
    using xofz.Presentation;
    using xofz.UI;
    using xofz.GoDaddyUpdater.Framework;
    using xofz.GoDaddyUpdater.UI;
    using System.Security.Principal;

    public sealed class HomePresenter : Presenter
    {
        public HomePresenter(
            HomeUi ui,
            MethodWeb web)
            : base(ui, null)
        {
            this.ui = ui;
            this.web = web;
            this.timerHandlerFinished = new ManualResetEvent(true);
        }

        public void Setup()
        {
            if (Interlocked.CompareExchange(ref this.setupIf1, 1, 0) == 1)
            {
                return;
            }

            var w = this.web;
            w.Run<EventSubscriber>(subscriber =>
            {
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.StartSyncingKeyTapped),
                    this.ui_StartSyncingKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.StopSyncingKeyTapped),
                    this.ui_StopSyncingKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopyHostnameKeyTapped),
                    this.ui_CopyHostnameKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopyCurrentIpKeyTapped),
                    this.ui_CopyCurrentIpKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopySyncedIpKeyTapped),
                    this.ui_CopySyncedIpKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.ExitRequested),
                    this.ui_ExitRequested);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.InstallServiceRequested),
                    this.ui_InstallServiceRequested);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.UninstallServiceRequested),
                    this.ui_UninstallServiceRequested);
                w.Run<xofz.Framework.Timer>(t =>
                    {
                        subscriber.Subscribe(
                            t,
                            nameof(t.Elapsed),
                            this.timer_Elapsed);
                    },
                    "HomeTimer");
            });

            w.Run<GlobalSettingsHolder>(settings =>
            {
                var startKeyEnabled = !settings.AutoStart;
                UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.StartSyncingKeyEnabled = startKeyEnabled;
                    this.ui.StopSyncingKeyEnabled = !startKeyEnabled;
                });
                this.ui.WriteFinished.WaitOne();
            });

            w.Run<GlobalSettingsHolder>(s =>
            {
                var hostname = s.Subdomain + "." + s.Domain;
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.Hostname = hostname);
                var ipProviderUri = s.HttpExternalIpProviderUri;
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.IpProviderUri = ipProviderUri);
            });

            w.Run<VersionReader>(vr =>
            {
                var version = vr.Read();
                var coreVersion = vr.ReadCoreVersion();
                UiHelpers.Write(
                    this.ui,
                    () =>
                    {
                        this.ui.Version = version;
                        this.ui.CoreVersion = coreVersion;
                    });
            });

            w.Run<Navigator>(n => n.RegisterPresenter(this));
        }

        public override void Start()
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t =>
                {
                    w.Run<EventRaiser>(er =>
                    {
                        er.Raise(
                            t,
                            nameof(t.Elapsed));
                    });
                    t.Start(TimeSpan.FromMinutes(5));
                },
                "HomeTimer");
        }

        public override void Stop()
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t =>
                {
                    t.Stop();
                    this.timerHandlerFinished.WaitOne();
                },
                "HomeTimer");
        }

        private void ui_CopyHostnameKeyTapped()
        {
            var hostname = UiHelpers.Read(
                this.ui,
                () => this.ui.Hostname);
            var w = this.web;
            w.Run<ClipboardCopier>(copier =>
            {
                UiHelpers.Write(
                    this.ui,
                    () => copier.Copy(hostname));
            });
        }

        private void ui_CopyCurrentIpKeyTapped()
        {
            var currentIP = UiHelpers.Read(
                this.ui,
                () => this.ui.CurrentIP);
            var w = this.web;
            w.Run<ClipboardCopier>(copier =>
            {
                UiHelpers.Write(
                    this.ui,
                    () => copier.Copy(currentIP));
            });
        }

        private void ui_CopySyncedIpKeyTapped()
        {
            var syncedIP = UiHelpers.Read(
                this.ui,
                () => this.ui.SyncedIP);
            var w = this.web;
            w.Run<ClipboardCopier>(copier =>
            {
                UiHelpers.Write(
                    this.ui,
                    () => copier.Copy(syncedIP));
            });
        }

        private void ui_StartSyncingKeyTapped()
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer, EventRaiser>((t, er) =>
                {
                    t.Stop();
                    this.timerHandlerFinished.WaitOne();
                },
                "HomeTimer");
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.StartSyncingKeyEnabled = false;
                    this.ui.StopSyncingKeyEnabled = true;
                });
            this.ui.WriteFinished.WaitOne();
            w.Run<xofz.Framework.Timer, EventRaiser>((t, er) =>
                {
                    er.Raise(t, nameof(t.Elapsed));
                    t.Start(TimeSpan.FromMinutes(5));
                },
                "HomeTimer");
        }

        private void ui_StopSyncingKeyTapped()
        {
            var w = this.web;
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.StartSyncingKeyEnabled = true;
                    this.ui.StopSyncingKeyEnabled = false;
                });
        }

        private void ui_ExitRequested()
        {
            var w = this.web;
            w.Run<Navigator>(n => n.Present<ShutdownPresenter>());
        }

        private void ui_InstallServiceRequested()
        {
            var w = this.web;
            if (!this.currentUserIsAdmin())
            {
                w.Run<Messenger>(m =>
                {
                    var response = UiHelpers.Read(
                        m.Subscriber,
                        () => m.Question(
                            "The app needs to run as administrator first."
                            + Environment.NewLine
                            + "Please try again after the app is running as administrator."
                            + Environment.NewLine
                            + "Run the app as administrator?"));
                    if (response == Response.Yes)
                    {
                        // thanks go to the question and accepted answer for this:
                        // https://stackoverflow.com/questions/16926232/run-process-as-administrator-from-a-non-admin-application
                        var psi = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            Verb = "runas",
                            WorkingDirectory = Environment.CurrentDirectory,
                            FileName = Path.GetFileName(
                                Assembly.GetEntryAssembly().Location)
                        };

                        Process.Start(psi);
                        UiHelpers.Write(
                            this.ui,
                            () => this.ui.HideNotifyIcon());
                        this.ui.WriteFinished.WaitOne();
                        w.Run<Navigator>(n => n.Present<ShutdownPresenter>());
                    }
                });

                return;
            }

            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory =
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly()
                    .Location);
            p.StartInfo.FileName = Path.Combine(
                Path.GetDirectoryName(
                        System
                        .Runtime
                        .InteropServices
                        .RuntimeEnvironment
                        .GetRuntimeDirectory()),
                @"installutil.exe");
            p.StartInfo.Arguments =
                nameof(xofz) +
                '.' +
                nameof(GoDaddyUpdater) +
                ".Service.exe";
            try
            {
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                w.Run<Messenger>(m =>
                {
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error installing service."
                            + Environment.NewLine
                            + ex.GetType().ToString()
                            + Environment.NewLine
                            + ex.Message));
                });
            }

            var ec = p.ExitCode;
            if (ec == 0)
            {
                w.Run<Messenger>(m =>
                {
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.Inform("Service installed!"));
                });
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.ServiceInstalled = true);
                return;
            }

            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () => m.GiveError(
                        "Error installing service."
                        + Environment.NewLine
                        + "Error code: " + ec));
            });
        }

        private void ui_UninstallServiceRequested()
        {
            var w = this.web;
            if (!this.currentUserIsAdmin())
            {
                w.Run<Messenger>(m =>
                {
                    var response = UiHelpers.Read(
                        m.Subscriber,
                        () => m.Question(
                            "The app needs to run as administrator first."
                            + Environment.NewLine
                            + "Please try again after the app is running as administrator."
                            + Environment.NewLine
                            + "Run the app as administrator?"));
                    if (response == Response.Yes)
                    {
                        var psi = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            Verb = "runas",
                            WorkingDirectory = Environment.CurrentDirectory,
                            FileName = Path.GetFileName(
                                Assembly.GetEntryAssembly().Location)
                        };

                        Process.Start(psi);
                        UiHelpers.Write(
                            this.ui,
                            () => this.ui.HideNotifyIcon());
                        this.ui.WriteFinished.WaitOne();
                        w.Run<Navigator>(n => n.Present<ShutdownPresenter>());
                    }
                });

                return;
            }

            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory =
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly()
                    .Location);
            p.StartInfo.FileName = Path.Combine(
                Path.GetDirectoryName(
                        System
                        .Runtime
                        .InteropServices
                        .RuntimeEnvironment
                        .GetRuntimeDirectory()),
                @"installutil.exe");
            p.StartInfo.Arguments =
                "/u " +
                nameof(xofz) +
                '.' +
                nameof(GoDaddyUpdater) +
                ".Service.exe";
            try
            {
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                w.Run<Messenger>(m =>
                {
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error uninstalling service."
                            + Environment.NewLine
                            + ex.GetType().ToString()
                            + Environment.NewLine
                            + ex.Message));
                });
                return;
            }

            var ec = p.ExitCode;
            if (ec != 0)
            {
                w.Run<Messenger>(m =>
                {
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error uninstalling service."
                            + Environment.NewLine
                            + "Error code: " + ec));
                });
                return;
            }

            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                  m.Subscriber,
                  () => m.Inform("Service uninstalled."));
            });
            UiHelpers.Write(
                    this.ui,
                    () => this.ui.ServiceInstalled = false);
        }

        private void timer_Elapsed()
        {
            var h = this.timerHandlerFinished;
            h.Reset();

            var w = this.web;
            string currentIP = null;
            w.Run<
                HttpClientFactory, 
                GlobalSettingsHolder, 
                Messages>(
                (factory, settings, messages) =>
            {
                var waitingMessage = messages.Waiting;
                var cantReadIpMessage = messages.CantReadIp;
                using (var hc = factory.Create())
                {
                    hc.Timeout = TimeSpan.FromMilliseconds(10000);

                    UiHelpers.Write(
                        this.ui,
                        () => this.ui.CurrentIP = waitingMessage);
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
                    
                    currentIP = currentIpTask.Result.Trim();
                }

                setCurrentIP:
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.CurrentIP = currentIP);
            });

            w.Run<
                HttpClientFactory,
                GlobalSettingsHolder,
                Messages>(
                (factory, settings, messages) =>
            {
                var waitingMessage = messages.Waiting;
                UiHelpers.Write(this.ui, () =>
                {
                    this.ui.SyncedIP = waitingMessage;
                });
                var ipTypeUnknownMessage = messages.IpTypeUnknown;
                var errorReadingFromDnsMessage = messages.ErrorReadingFromDns;
                var errorSyncingMessage = messages.ErrorSyncing;
                var cantReadIpMessage = messages.CantReadIp;
                string syncedIP;
                IPAddress currentAddress;
                bool aaaa;
                if (IPAddress.TryParse(currentIP, out currentAddress))
                {
                    aaaa = currentAddress
                        .AddressFamily ==
                        AddressFamily.InterNetworkV6;
                }
                else
                {
                    syncedIP = ipTypeUnknownMessage;
                    goto setSyncedIP;
                }

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
                        goto setSyncedIP;
                    }

                    var response = task.Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        goto error;
                    }

                    var json = response
                        .Content
                        .ReadAsStringAsync()
                        .Result;
                    var records = JsonConvert.DeserializeObject<ICollection<Record>>(
                        json);
                    var lastChecked = DateTime.Now.ToString();
                    UiHelpers.Write(
                        this.ui,
                        () => this.ui.LastChecked = lastChecked);
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
                    var shouldSync = UiHelpers.Read(
                        this.ui,
                        () => this.ui.StopSyncingKeyEnabled);
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
                        var lastSynced = DateTime.Now.ToString();
                        if (syncedByService)
                        {
                            lastSynced += Environment.NewLine;
                            lastSynced += settings.ServiceAttribution;
                        }

                        UiHelpers.Write(
                            this.ui,
                            () => this.ui.LastSynced = lastSynced);
                        syncedIP = currentIP;
                        goto setSyncedIP;
                    }

                    error:
                    w.Run<EventRaiser>(er =>
                    {
                        er.Raise(
                            this.ui,
                            nameof(this.ui.StopSyncingKeyTapped));
                    });

                    syncedIP = errorReadingFromDnsMessage;
                }

                setSyncedIP:
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.SyncedIP = syncedIP);

                if (syncedIP == ipTypeUnknownMessage)
                {
                    return;
                }

                if (syncedIP == errorReadingFromDnsMessage)
                {
                    return;
                }

                if (syncedIP.Contains(errorSyncingMessage))
                {
                    return;
                }

                this.setLastSyncedIP(syncedIP);
            });

            w.Run<ServiceChecker>(checker =>
            {
                var exists = checker.ServiceExists();
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.ServiceInstalled = exists);
            });
            
            h.Set();
        }

        private void setLastSyncedIP(string lastSyncedIP)
        {
            this.lastSyncedIP = lastSyncedIP;
        }

        private bool currentUserIsAdmin()
        {
            var principle = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());
            return principle.IsInRole(
                WindowsBuiltInRole.Administrator);
        }

        private long setupIf1;
        private string lastSyncedIP;
        private readonly HomeUi ui;
        private readonly MethodWeb web;
        private readonly ManualResetEvent timerHandlerFinished;
    }
}
