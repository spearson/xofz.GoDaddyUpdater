namespace xofz.GoDaddyUpdater.Service.Root.Commands
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.Root;

    public class SetupMethodWebCommand : Command
    {
        public SetupMethodWebCommand(
            SettingsProvider settingsProvider,
            MethodWebV2 web)
        {
            this.settingsProvider = settingsProvider;
            this.web = web;
        }

        public override void Execute()
        {
            this.registerDependencies();
        }

        protected virtual void registerDependencies()
        {
            var w = this.web;
            w.RegisterDependency(
                new EventSubscriber());
            w.RegisterDependency(
                this.settingsProvider.Provide());

        }

        protected readonly SettingsProvider settingsProvider;
        protected readonly MethodWebV2 web;
    }
}
