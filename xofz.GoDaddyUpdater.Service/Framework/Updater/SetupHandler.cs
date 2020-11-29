namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using xofz.Framework;

    public class SetupHandler
    {
        public SetupHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            Do<string> applyServiceName)
        {
            var r = this.runner;
            r?.Run<GlobalSettingsHolder>(settings =>
            {
                // must match value in ProjectInstaller ctor
                applyServiceName(
                    @"gdu."
                    + settings.Subdomain
                    + '.'
                    + settings.Domain
                    + '.'
                    + settings
                        .HttpExternalIpProviderUri
                        ?.Replace('/', '-'));
            });
        }

        protected readonly MethodRunner runner;
    }
}
