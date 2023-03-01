namespace xofz.GoDaddyUpdater.Framework
{
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
            const bool falsity = false;
            var exists = falsity;
            r?.Run<
                GlobalSettingsHolder,
                EnumerableHelper>(
                (settings, helper) =>
                {
                    var service = helper.FirstOrNull(
                        ServiceController.GetServices(),
                        s => s.ServiceName ==
                             @"gdu."
                             + settings.Subdomain
                             + '.'
                             + settings.Domain
                             + '.'
                             + settings
                                 .HttpExternalIpProviderUri
                                 .Replace('/', '-'));
                    exists = service != null;
                });

            return exists;
        }

        protected readonly MethodRunner runner;
    }
}
