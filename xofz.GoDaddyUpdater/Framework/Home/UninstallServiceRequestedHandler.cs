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
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui,
            Do shutdown)
        {
            var w = this.web;
            var admin = w.Run<AdminChecker>()?.CurrentUserIsAdmin() ?? false;
            if (!admin)
            {
                w.Run<Messenger, UiReaderWriter>((m, uiRW) =>
                {
                    var response = uiRW.Read(
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
                        uiRW.WriteSync(
                            ui,
                            ui.HideNotifyIcon);
                        shutdown?.Invoke();
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
                w.Run<Messenger, UiReaderWriter>((m, uiRw) =>
                {
                    uiRw.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error uninstalling service."
                            + Environment.NewLine
                            + ex.GetType()
                            + Environment.NewLine
                            + ex.Message));
                });
                return;
            }

            var ec = p.ExitCode;
            if (ec != 0)
            {
                w.Run<Messenger, UiReaderWriter>((m, uiRW) =>
                {
                    uiRW.Write(
                        m.Subscriber,
                        () => m.GiveError(
                            "Error uninstalling service."
                            + Environment.NewLine
                            + "Error code: " + ec));
                });
                return;
            }

            w.Run<UiReaderWriter>(uiRw =>
            {
                w.Run<Messenger>(m =>
                {
                    uiRw.Write(
                        m.Subscriber,
                        () => m.Inform("Service uninstalled."));
                });

                uiRw.Write(
                    ui,
                    () =>
                    {
                        ui.ServiceInstalled = false;
                    });
            });
        }

        protected readonly MethodWeb web;
    }
}
