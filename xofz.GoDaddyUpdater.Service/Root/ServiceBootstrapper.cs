namespace xofz.GoDaddyUpdater.Service.Root
{
    using System.ServiceProcess;
    using System.Threading;
    using xofz.Framework;
    using xofz.Root;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.GlobalSettingsProviders;

    public class ServiceBootstrapper
    {
        public ServiceBootstrapper()
            :this(new CommandExecutor())
        {
        }

        public ServiceBootstrapper(
            CommandExecutor executor)
        {
            this.executor = executor;
        }

        public virtual ServiceBase Service => this.service;

        public virtual void Bootstrap()
        {
            if (Interlocked.CompareExchange(ref this.bootstrappedIf1, 1, 0) == 1)
            {
                return;
            }

            var e = this.executor;
            var w = new xofz.Framework.MethodWeb();
            this.setWeb(w);
            w.RegisterDependency(
                new xofz.Framework.Timer());
            w.RegisterDependency(
                new EventSubscriber());
            w.RegisterDependency(
                new EventRaiser());
            GlobalSettingsProvider provider = 
                new ExeConfigSettingsProvider();
            w.RegisterDependency(
                provider.Provide());
            w.RegisterDependency(
                new HttpClientFactory(w));
            this.setService(
                new UpdaterService(w));
        }

        private void setWeb(MethodWeb web)
        {
            this.web = web;
        }

        private void setService(ServiceBase service)
        {
            this.service = service;
        }

        private long bootstrappedIf1;
        private ServiceBase service;
        private MethodWeb web;
        private readonly CommandExecutor executor;
    }
}
