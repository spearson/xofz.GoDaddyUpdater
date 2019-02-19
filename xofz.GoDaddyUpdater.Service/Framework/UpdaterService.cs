namespace xofz.GoDaddyUpdater.Service.Framework
{
    using System.ServiceProcess;
    using System.Threading;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.Updater;

    public partial class UpdaterService : ServiceBase
    {
        private UpdaterService()
        {
            this.InitializeComponent();
        }

        public UpdaterService(MethodWeb web)
            : this()
        {
            this.web = web;

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

            var w = this.web;
            w.Run<EventSubscriber>(sub =>
            {
                w.Run<xofz.Framework.Timer>(t =>
                {
                    sub.Subscribe(
                        t,
                        nameof(t.Elapsed),
                        this.timer_Elapsed);
                });
            });

            Do<string> applyServiceName =
                name => this.ServiceName = name;
            w.Run<SetupHandler>(handler =>
            {
                handler.Handle(applyServiceName);
            });

        }

        protected override void OnStart(string[] args)
        {
            var w = this.web;
            w.Run<StartHandler>(handler =>
            {
                handler.Handle();
            });
        }

        protected override void OnStop()
        {
            var w = this.web;
            w.Run<StopHandler>(handler =>
            {
                handler.Handle();
            });
        }

        private void timer_Elapsed()
        {
            var w = this.web;
            w.Run<TimerHandler>(handler =>
            {
                handler.Handle();
            });
        }

        private long setupIf1;
        private readonly MethodWeb web;
    }
}
