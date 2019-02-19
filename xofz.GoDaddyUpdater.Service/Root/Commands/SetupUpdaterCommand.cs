namespace xofz.GoDaddyUpdater.Service.Root.Commands
{
    using System.ServiceProcess;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.Updater;
    using xofz.Root;

    public class SetupUpdaterCommand : Command
    {
        public SetupUpdaterCommand(MethodWeb web)
        {
            this.web = web;
        }

        public virtual ServiceBase UpdaterService => this.service;

        public override void Execute()
        {
            this.registerDependencies();

            this.setService(
                new UpdaterService(this.web));
        }

        protected virtual void setService(
            UpdaterService service)
        {
            service?.Setup();
            this.service = service;
        }

        protected virtual void registerDependencies()
        {
            var w = this.web;
            w.RegisterDependency(
                new xofz.Framework.Timer());
            w.RegisterDependency(
                new SetupHandler(w));
            w.RegisterDependency(
                new HttpClientFactory(w));
            w.RegisterDependency(
                new TimerHandler(w));
            w.RegisterDependency(
                new StartHandler(w));
            w.RegisterDependency(
                new StopHandler(w));
        }

        protected ServiceBase service;
        protected readonly MethodWeb web;
    }
}
