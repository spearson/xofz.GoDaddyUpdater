namespace xofz.GoDaddyUpdater.Framework
{
    using System.Linq;
    using System.ServiceProcess;
    using xofz.Framework;

    public class ServiceChecker
    {
        public ServiceChecker(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual bool ServiceExists()
        {
            var r = this.runner;
            var exists = false;
            r.Run<GlobalSettingsHolder>(settings =>
            {
                var sc = ServiceController
                    .GetServices()
                    .FirstOrDefault(service => service.ServiceName ==
                        @"gdu."
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

        protected readonly MethodRunner runner;
    }
}
