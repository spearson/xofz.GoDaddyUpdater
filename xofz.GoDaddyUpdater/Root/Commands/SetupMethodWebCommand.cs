namespace xofz.GoDaddyUpdater.Root.Commands
{
    using System.Net.Http;
    using System.Reflection;
    using xofz.Framework;
    using xofz.Framework.Logging.Logs;
    using xofz.Presentation;
    using xofz.Root;
    using xofz.UI;
    using xofz.GoDaddyUpdater.Framework;

    public class SetupMethodWebCommand : Command
    {
        public SetupMethodWebCommand(
            SettingsProvider settingsProvider,
            Messenger messenger,
            MethodWebV2 web)
        {
            this.settingsProvider = settingsProvider;
            this.messenger = messenger;
            this.web = web;
        }

        public virtual MethodWebV2 W => this.web;

        public override void Execute()
        {
            this.registerDependencies();
        }

        protected virtual void registerDependencies()
        {
            var w = this.web;
            w?.RegisterDependency(
                new UiReaderWriter());
            w?.RegisterDependency(
                new EventRaiser());
            w?.RegisterDependency(
                new EventSubscriber());
            w?.RegisterDependency(
                new NavigatorV2(w));
            w?.RegisterDependency(
                this.messenger);
            w?.RegisterDependency(
                this.settingsProvider.Provide());
            w?.RegisterDependency(
                (Gen<HttpMessageHandler>)(() => default));
            w?.RegisterDependency(
                new HttpClientFactory(w));
            w?.RegisterDependency(
                new TextFileLog(
                    @"Exceptions.log"),
                LogNames.Exceptions);
            w?.RegisterDependency(
                new VersionReader(
                    Assembly.GetExecutingAssembly()));
            w?.RegisterDependency(
                new ServiceChecker(w));
        }

        protected readonly SettingsProvider settingsProvider;
        protected readonly Messenger messenger;
        protected readonly MethodWebV2 web;
    }
}
