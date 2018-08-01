namespace xofz.GoDaddyUpdater.Framework
{
    using System.Linq;
    using System.ServiceProcess;
    using xofz.Framework;

    public class ServiceChecker
    {
        public ServiceChecker(MethodWeb web)
        {
            this.web = web;
        }

        public virtual bool ServiceExists()
        {
            var w = this.web;
            var exists = false;
            w.Run<GlobalSettingsHolder>(settings =>
            {
                var sc = ServiceController
                    .GetServices()
                    .FirstOrDefault(service => service.ServiceName ==
                        "gdu."
                        + settings.Subdomain
                        + '.'
                        + settings.Domain
                        + '.'
                        + settings
                            .HttpExternalIpProviderUri
                            .Replace('/', '-'));
                if (sc != default(ServiceController))
                {
                    exists = true;
                }
            });

            return exists;
        }

        private readonly MethodWeb web;
    }
}
