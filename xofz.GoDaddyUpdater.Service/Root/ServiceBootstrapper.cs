namespace xofz.GoDaddyUpdater.Service.Root
{
    using System.ServiceProcess;
    using System.Threading;
    using xofz.Root;
    using xofz.GoDaddyUpdater.Service.Framework.SettingsProviders;
    using xofz.GoDaddyUpdater.Service.Root.Commands;

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

        public virtual ServiceBase Service => this.updaterService;

        public virtual void Bootstrap()
        {
            const byte one = 1;

            if (Interlocked.Exchange(
                    ref this.bootstrappedIf1, 
                    one) == one)
            {
                return;
            }

            var e = this.executor;
            if (e == null)
            {
                return;
            }

            var w = new xofz.Framework.MethodWebV2();
            e
                .Execute(new SetupMethodWebCommand(
                    new ExeConfigSettingsProvider(),
                    w))
                .Execute(new SetupUpdaterCommand(
                    w));

            this.updaterService = e
                .Get<SetupUpdaterCommand>()
                ?.UpdaterService;
        }

        protected long bootstrappedIf1;
        protected ServiceBase updaterService;
        protected readonly CommandExecutor executor;
    }
}
