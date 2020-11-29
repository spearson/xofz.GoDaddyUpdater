namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class UninstallServiceRequestedHandler
    {
        public UninstallServiceRequestedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui,
            Do shutdown)
        {
            var r = this.runner;
            var admin = r.Run<AdminChecker>()?.CurrentUserIsAdmin() ?? false;
            if (!admin)
            {
                r?.Run<Messenger, UiReaderWriter>((m, uiRW) =>
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly == null)
                    {
                        return;
                    }

                    var response = uiRW.Read(
                        m.Subscriber,
                        () => m.Question(
                            @"The app needs to run as administrator first."
                            + Environment.NewLine
                            + @"Please try again after the app is running as administrator."
                            + Environment.NewLine
                            + @"Run the app as administrator?"));
                    if (response == Response.Yes)
                    {
                        var psi = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            Verb = @"runas",
                            WorkingDirectory = Environment.CurrentDirectory,
                            FileName = Path.GetFileName(
                                entryAssembly.Location)
                        };

                        Process.Start(psi);
                        uiRW.WriteSync(
                            ui,
                            ui.HideNotifyIcon);
                        shutdown?.Invoke();
                    }
                });

                return;
            }

            var exAssembly = Assembly.GetExecutingAssembly();
            if (exAssembly == null)
            {
                return;
            }

            var wd = Path.GetDirectoryName(
                exAssembly.Location);
            if (wd == null)
            {
                return;
            }

            var rtd = System
                .Runtime
                .InteropServices
                .RuntimeEnvironment
                .GetRuntimeDirectory();
            var rtdn = Path.GetDirectoryName(
                rtd);
            if (rtdn == null)
            {
                return;
            }

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    WorkingDirectory = wd,
                    FileName = Path.Combine(
                        rtdn,
                        @"installutil.exe"),
                    Arguments = "/u " +
                                nameof(xofz) +
                                '.' +
                                nameof(GoDaddyUpdater) +
                                @".Service.exe"
                }
            };
            try
            {
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                r?.Run<Messenger, UiReaderWriter>(
                    (m, uiRW) =>
                    {
                        uiRW.Write(
                            m.Subscriber,
                            () =>
                            {
                                m.GiveError(
                                    @"Error uninstalling service."
                                    + Environment.NewLine
                                    + ex.GetType()
                                    + Environment.NewLine
                                    + ex.Message);
                            });

                        uiRW.Write(
                            ui,
                            () =>
                            {
                                ui.ServiceInstalled = true;
                            });
                    });
                return;
            }

            var ec = p.ExitCode;
            if (ec != 0)
            {
                r?.Run<Messenger, UiReaderWriter>(
                    (m, uiRW) =>
                    {
                        uiRW.Write(
                            m.Subscriber,
                            () =>
                            {
                                m.GiveError(
                                    @"Error uninstalling service."
                                    + Environment.NewLine
                                    + @"Error code: " + ec);
                            });

                        uiRW.Write(
                            ui,
                            () =>
                            {
                                ui.ServiceInstalled = true;
                            });
                    });

                return;
            }

            r?.Run<UiReaderWriter>(uiRW =>
            {
                r.Run<Messenger>(m =>
                {
                    uiRW.Write(
                        m.Subscriber,
                        () =>
                        {
                            m.Inform(@"Service uninstalled.");
                        });
                });

                uiRW.Write(
                    ui,
                    () =>
                    {
                        ui.ServiceInstalled = false;
                    });
            });
        }

        protected readonly MethodRunner runner;
    }
}
