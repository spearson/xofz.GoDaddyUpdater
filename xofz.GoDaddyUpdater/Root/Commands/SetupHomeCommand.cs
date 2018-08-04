namespace xofz.GoDaddyUpdater.Root.Commands
{
    using xofz.Framework;
    using xofz.Root;
    using xofz.GoDaddyUpdater.Framework;
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

        private void registerDependencies()
        {
            var w = this.web;
            w.RegisterDependency(
                new xofz.Framework.Timer(),
                "HomeTimer");
            w.RegisterDependency(
                this.clipboardCopier);
            w.RegisterDependency(
                new Messages
                {
                    IpTypeUnknown =
                        "Could not tell if IP is IPv4 or IPv6 address.",
                    ErrorReadingFromDns =
                        "Error reading synced IP from DNS.",
                    ErrorSyncing =
                        "Error syncing. "
                });
        }

        private readonly HomeUi ui;
        private readonly ClipboardCopier clipboardCopier;
        private readonly MethodWeb web;
    }
}
