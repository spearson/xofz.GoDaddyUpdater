namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class ProcessStarter
    {
        public ProcessStarter(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void RestartAsAdmin()
        {
            var r = this.runner;
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                return;
            }

            // thanks go to the question and accepted answer for this:
            // https://stackoverflow.com/questions/16926232/run-process-as-administrator-from-a-non-admin-application
            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                Verb = @"runas",
                WorkingDirectory = System.Environment.CurrentDirectory,
                FileName = Path.GetFileName(
                    entryAssembly.Location)
            };

            Process.Start(psi);
            r?.Run<HomeUi, UiHelper>((ui, uiH) =>
            {
                uiH.WriteSync(
                    ui,
                    ui.HideNotifyIcon);
            });

            r?.Run<NavReader>(reader =>
            {
                reader.ReadShutdown(
                    out var shutdown);
                shutdown?.Invoke();
            });
        }

        public virtual void InstallService()
        {
            const bool
                truth = true,
                falsity = false;
            var r = this.runner;
            var exAssembly = Assembly.GetExecutingAssembly();
            string workingDirectory;
            try
            {
                workingDirectory = Path.GetDirectoryName(
                    exAssembly.Location);
            }
            catch
            {
                workingDirectory = null;
            }

            if (workingDirectory == null)
            {
                return;
            }

            var rtd = System
                .Runtime
                .InteropServices
                .RuntimeEnvironment
                .GetRuntimeDirectory();
            string rtdn;
            try
            {
                rtdn = Path.GetDirectoryName(rtd);
            }
            catch
            {
                rtdn = null;
            }


            if (rtdn == null)
            {
                return;
            }

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = falsity,
                    WorkingDirectory = workingDirectory,
                    FileName = Path.Combine(
                        rtdn,
                        @"installutil.exe"),
                    Arguments = nameof(xofz) +
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
            catch (System.Exception ex)
            {
                r?.Run<Messenger, UiReaderWriter>(
                    (m, uiRW) =>
                    {
                        uiRW.Write(
                            m.Subscriber,
                            () => m.GiveError(
                                @"Error installing service."
                                + System.Environment.NewLine
                                + ex.GetType()
                                + System.Environment.NewLine
                                + ex.Message));
                        r.Run<HomeUi>(ui =>
                        {
                            uiRW.Write(
                                ui,
                                () => { ui.ServiceInstalled = falsity; });
                        });

                    });
                return;
            }

            const byte successCode = 0;
            var ec = p.ExitCode;
            if (ec == successCode)
            {
                r.Run<UiReaderWriter>(uiRW =>
                {
                    r.Run<Messenger>(m =>
                    {
                        uiRW.Write(
                            m.Subscriber,
                            () => { m.Inform(@"Service installed!"); });
                    });
                    r.Run<HomeUi>(ui =>
                    {
                        uiRW.Write(
                            ui,
                            () => { ui.ServiceInstalled = truth; });
                    });
                });

                return;
            }

            r.Run<Messenger, UiReaderWriter>(
                (m, uiRW) =>
                {
                    uiRW.Write(
                        m.Subscriber,
                        () =>
                        {
                            m.GiveError(
                                @"Error installing service."
                                + System.Environment.NewLine
                                + @"Error code: " + ec);
                            r.Run<HomeUi>(ui =>
                            {
                                uiRW.Write(
                                    ui,
                                    () => { ui.ServiceInstalled = falsity; });
                            });
                        });
                });
        }

        public virtual void UninstallService()
        {
            const bool
                truth = true,
                falsity = false;
            var r = this.runner;
            var exAssembly = Assembly.GetExecutingAssembly();
            string workingDirectory;
            try
            {
                workingDirectory = Path.GetDirectoryName(
                    exAssembly.Location);
            }
            catch
            {
                workingDirectory = null;
            }

            if (workingDirectory == null)
            {
                return;
            }

            var rtd = System
                .Runtime
                .InteropServices
                .RuntimeEnvironment
                .GetRuntimeDirectory();
            string rtdn;
            try
            {
                rtdn = Path.GetDirectoryName(rtd);
            }
            catch
            {
                rtdn = null;
            }


            if (rtdn == null)
            {
                return;
            }

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
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
            catch (System.Exception ex)
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
                                    + System.Environment.NewLine
                                    + ex.GetType()
                                    + System.Environment.NewLine
                                    + ex.Message);
                            });

                        r.Run<HomeUi>(ui =>
                        {
                            uiRW.Write(
                                ui,
                                () =>
                                {
                                    ui.ServiceInstalled = truth;
                                });
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
                                    + System.Environment.NewLine
                                    + @"Error code: " + ec);
                            });

                        r.Run<HomeUi>(ui =>
                        {
                            uiRW.Write(
                                ui,
                                () =>
                                {
                                    ui.ServiceInstalled = truth;
                                });
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

                r.Run<HomeUi>(ui =>
                {
                    uiRW.Write(
                        ui,
                        () =>
                        {
                            ui.ServiceInstalled = falsity;
                        });
                });
            });
        }

        protected readonly MethodRunner runner;
    }
}
