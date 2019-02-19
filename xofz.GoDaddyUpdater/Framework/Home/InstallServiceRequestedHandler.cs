namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class InstallServiceRequestedHandler
    {
        public InstallServiceRequestedHandler(
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
                w.Run<Messenger, UiReaderWriter>(
                    (m, uiRW) =>
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
                            "Error installing service."
                            + Environment.NewLine
                            + ex.GetType()
                            + Environment.NewLine
                            + ex.Message));
                });
            }

            var ec = p.ExitCode;
            if (ec == 0)
            {
                w.Run<UiReaderWriter>(uiRW =>
                {
                    w.Run<Messenger>(m =>
                    {
                        uiRW.Write(
                            m.Subscriber,
                            () => m.Inform("Service installed!"));
                    });

                    uiRW.Write(
                        ui,
                        () => ui.ServiceInstalled = true);
                });

                return;
            }

            w.Run<Messenger, UiReaderWriter>((m, uiRw) =>
            {
                uiRw.Write(
                    m.Subscriber,
                    () => m.GiveError(
                        "Error installing service."
                        + Environment.NewLine
                        + "Error code: " + ec));
            });
        }

        protected readonly MethodWeb web;
    }
}
