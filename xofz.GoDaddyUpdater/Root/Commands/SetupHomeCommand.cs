namespace xofz.GoDaddyUpdater.Root.Commands
{
    using System.Threading;
    using xofz.Framework;
    using xofz.Root;
    using xofz.GoDaddyUpdater.Framework;
    using xofz.GoDaddyUpdater.Framework.Home;
    using xofz.GoDaddyUpdater.Presentation;
    using xofz.GoDaddyUpdater.UI;

    public class SetupHomeCommand : Command
    {
        public SetupHomeCommand(
            HomeUi ui,
            ClipboardCopier clipboardCopier,
            MethodWeb web)
        {
            this.ui = ui;
            this.clipboardCopier = clipboardCopier;
            this.web = web;
        }

        public override void Execute()
        {
            this.registerDependencies();

            new HomePresenter(
                    this.ui,
                    this.web)
                .Setup();
        }

        protected virtual void registerDependencies()
        {
            var w = this.web;
            if (w == null)
            {
                return;
            }

            w.RegisterDependency(
                this.ui);
            w.RegisterDependency(
                new xofz.Framework.Timer(),
                DependencyNames.Timer);
            w.RegisterDependency(
                this.clipboardCopier);
            w.RegisterDependency(
                new Messages
                {
                    CantReadIp =
                        @"Could not read current IP",
                    Waiting = "...",
                    IpTypeUnknown =
                        @"IP address could not be parsed.",
                    ErrorReadingFromDns =
                        @"Error reading synced IP from DNS.",
                    ErrorSyncing =
                        @"Error syncing. "
                });
            w.RegisterDependency(
                new ProcessStarter(w));
            w.RegisterDependency(
                new SetupHandler(w));
            w.RegisterDependency(
                new StartHandler(w));
            w.RegisterDependency(
                new StopHandler(w));
            w.RegisterDependency(
                new CopyHostnameKeyTappedHandler(w));
            w.RegisterDependency(
                new CopyCurrentIpKeyTappedHandler(w));
            w.RegisterDependency(
                new CopySyncedIpKeyTappedHandler(w));
            const bool truth = true;
            w.RegisterDependency(
                new LatchHolder
                {
                    Latch = new ManualResetEvent(truth)
                },
                DependencyNames.TimerLatch);
            w.RegisterDependency(
                new TimerHandler(w));
            w.RegisterDependency(
                new StartSyncingKeyTappedHandler(w));
            w.RegisterDependency(
                new StopSyncingKeyTappedHandler(w));
            w.RegisterDependency(
                new ExitRequestedHandler(w));
            w.RegisterDependency(
                new AdminChecker(w));
            w.RegisterDependency(
                new InstallServiceRequestedHandler(w));
            w.RegisterDependency(
                new UninstallServiceRequestedHandler(w));
        }

        protected readonly HomeUi ui;
        protected readonly ClipboardCopier clipboardCopier;
        protected readonly MethodWeb web;
    }
}
