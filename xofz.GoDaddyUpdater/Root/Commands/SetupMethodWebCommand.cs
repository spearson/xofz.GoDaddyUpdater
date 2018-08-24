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
            Func<MethodWeb> createWeb)
        {
            this.settingsProvider = settingsProvider;
            this.messenger = messenger;
            this.createWeb = createWeb;
        }

        public virtual MethodWeb Web => this.web;

        public override void Execute()
        {
            this.setWeb(this.createWeb());
            this.registerDependencies();
        }

        private void setWeb(MethodWeb web)
        {
            this.web = web;
        }

        private void registerDependencies()
        {
            var w = this.web;
            w.RegisterDependency(
                new EventRaiser());
            w.RegisterDependency(
                new EventSubscriber());
            w.RegisterDependency(
                new Navigator(w));
            w.RegisterDependency(
                this.messenger);
            w.RegisterDependency(
                this.settingsProvider.Provide());
            w.RegisterDependency(
                (Func<HttpMessageHandler>)(() => default(HttpMessageHandler)));
            w.RegisterDependency(
                new HttpClientFactory(w));
            w.RegisterDependency(
                new TextFileLog(@"Exceptions.log"),
                "Exceptions");
            w.RegisterDependency(
                new VersionReader(
                    Assembly.GetExecutingAssembly()));
            w.RegisterDependency(
                new ServiceChecker(w));
        }

        private MethodWeb web;
        private readonly SettingsProvider settingsProvider;
        private readonly Messenger messenger;
        private readonly Func<MethodWeb> createWeb;
    }
}
