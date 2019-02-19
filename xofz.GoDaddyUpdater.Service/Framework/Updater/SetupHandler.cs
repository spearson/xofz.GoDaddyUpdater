namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using xofz.Framework;

    public class SetupHandler
    {
        public SetupHandler(MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            Do<string> applyServiceName)
        {
            var w = this.web;
            w.Run<GlobalSettingsHolder>(settings =>
            {
                // must match value in ProjectInstaller ctor
                applyServiceName(
                    "gdu."
                    + settings.Subdomain
                    + '.'
                    + settings.Domain
                    + '.'
                    + settings
                        .HttpExternalIpProviderUri
                        .Replace('/', '-'));
            });
        }

        protected readonly MethodWeb web;
    }
}
