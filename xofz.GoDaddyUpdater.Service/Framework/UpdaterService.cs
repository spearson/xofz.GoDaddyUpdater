namespace xofz.GoDaddyUpdater.Service.Framework
{
    using System.ServiceProcess;
    using System.Threading;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.Updater;

    public partial class UpdaterService 
        : ServiceBase
    {
        public UpdaterService(
            MethodRunner runner)
            : this()
        {
            this.runner = runner;
        }

        private UpdaterService()
        {
            this.InitializeComponent();
        }

        public virtual void Setup()
        {
            if (Interlocked.CompareExchange(
                ref this.setupIf1, 
                1, 
                0) == 1)
            {
                return;
            }

            var r = this.runner;
            r.Run<EventSubscriber>(sub =>
            {
                r.Run<xofz.Framework.Timer>(t =>
                {
                    sub.Subscribe(
                        t,
                        nameof(t.Elapsed),
                        this.timer_Elapsed);
                });
            });

            Do<string> applyServiceName =
                name => this.ServiceName = name;
            r.Run<SetupHandler>(handler =>
            {
                handler.Handle(applyServiceName);
            });

        }

        protected override void OnStart(string[] args)
        {
            var r = this.runner;
            r.Run<StartHandler>(handler =>
            {
                handler.Handle();
            });
        }

        protected override void OnStop()
        {
            var r = this.runner;
            r.Run<StopHandler>(handler =>
            {
                handler.Handle();
            });
        }

        protected virtual void timer_Elapsed()
        {
            var r = this.runner;
            r.Run<TimerHandler>(handler =>
            {
                handler.Handle();
            });
        }

        protected long setupIf1;
        protected readonly MethodRunner runner;
    }
}
