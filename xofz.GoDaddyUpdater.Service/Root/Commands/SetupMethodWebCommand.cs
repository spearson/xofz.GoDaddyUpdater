namespace xofz.GoDaddyUpdater.Service.Root.Commands
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.Root;

    public class SetupMethodWebCommand : Command
    {
        public SetupMethodWebCommand(
            SettingsProvider settingsProvider,
            MethodWeb web)
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
            if (w == null)
            {
                return;
            }

            w.RegisterDependency(
                new EnumerableHelper());
            w.RegisterDependency(
                new EventSubscriber());
            w.RegisterDependency(
                this.settingsProvider?.Provide());

        }

        protected readonly SettingsProvider settingsProvider;
        protected readonly MethodWeb web;
    }
}
