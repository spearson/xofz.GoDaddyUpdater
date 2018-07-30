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
                    nameof(this.ui.RefreshServiceRequested),
                    this.ui_RefreshServiceRequested);
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

        private void ui_RefreshServiceRequested()
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
                            "Error refreshing service."
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
                            "Error refreshing service."
                            + Environment.NewLine
                            + "Error code: " + ec));
                });
                return;
            }

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
                            "Error refreshing service."
                            + Environment.NewLine
                            + ex.GetType().ToString()
                            + Environment.NewLine
                            + ex.Message));
                });
                return;
            }

            ec = p.ExitCode;
            if (ec != 0)
            {
                w.Run<Messenger>(m =>
                {
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error refreshing service."
                            + Environment.NewLine
                            + "Error code: " + ec));
                });
                return;
            }

            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () => m.Inform(
                        "Service refreshed!"));
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
        }

        private void timer_Elapsed()
        {
            var h = this.timerHandlerFinished;
            h.Reset();

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

            w.Run<HttpClientFactory, GlobalSettingsHolder>(
                (factory, settings) =>
            {
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
                    syncedIP = "Could not tell if IP is IPv4 or IPv6 address.";
                    goto setSyncedIP;
                }

                var uri = new StringBuilder()
                    .Append(@"https://api.godaddy.com/v1/domains/")
                    .Append(settings.Domain)
                    .Append(@"/records/")
                    .Append(aaaa ? "AAAA" : "A")
                    .Append(@"/")
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
                        syncedIP = @"Error reading synced IP from DNS.";
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
                    syncTask.Wait();
                    response = task.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var lastSynced = DateTime.Now.ToString();
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

                    w.Run<xofz.Framework.Timer, Messenger>((t, m) =>
                    {
                        t.Stop();

                        var message = "There was an error reading or updating DNS records for "
                            + settings.Subdomain
                            + @"."
                            + settings.Domain
                            + @"."
                            + Environment.NewLine
                            + (int)response.StatusCode
                            + Environment.NewLine
                            + response.ToString();
                        UiHelpers.Write(
                            m.Subscriber,
                            () => m.GiveError(message));
                        m.Subscriber.WriteFinished.WaitOne();

                        t.Start(TimeSpan.FromMinutes(5));
                    },
                    "HomeTimer");
                    return;
                }

                setSyncedIP:
                UiHelpers.Write(
                    this.ui,
                    () => this.ui.SyncedIP = syncedIP);
            });

            h.Set();
        }

        private bool currentUserIsAdmin()
        {
            var principle = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());
            return principle.IsInRole(
                WindowsBuiltInRole.Administrator);
        }

        private long setupIf1;
        private readonly HomeUi ui;
        private readonly MethodWeb web;
        private readonly ManualResetEvent timerHandlerFinished;
        private const string cantReadIpMessage = @"Could not read current IP";
    }
}
