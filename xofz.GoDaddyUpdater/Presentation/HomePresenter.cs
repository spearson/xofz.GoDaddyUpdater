namespace xofz.GoDaddyUpdater.Presentation
{
    using System.Threading;
    using xofz.Framework;
    using xofz.Presentation;
    using xofz.GoDaddyUpdater.UI;
    using xofz.GoDaddyUpdater.Framework.Home;

    public sealed class HomePresenter : Presenter
    {
        public HomePresenter(
            HomeUi ui,
            MethodWeb web)
            : base(ui, null)
        {
            this.ui = ui;
            this.web = web;
        }

        public void Setup()
        {
            if (Interlocked.CompareExchange(
                    ref this.setupIf1, 
                    1, 
                    0) == 1)
            {
                return;
            }

            var w = this.web;
            w.Run<EventSubscriber>(subscriber =>
            {
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.StartSyncingKeyTapped),
                    this.ui_StartSyncingKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.StopSyncingKeyTapped),
                    this.ui_StopSyncingKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopyHostnameKeyTapped),
                    this.ui_CopyHostnameKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopyCurrentIpKeyTapped),
                    this.ui_CopyCurrentIpKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.CopySyncedIpKeyTapped),
                    this.ui_CopySyncedIpKeyTapped);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.ExitRequested),
                    this.ui_ExitRequested);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.InstallServiceRequested),
                    this.ui_InstallServiceRequested);
                subscriber.Subscribe(
                    this.ui,
                    nameof(this.ui.UninstallServiceRequested),
                    this.ui_UninstallServiceRequested);
                w.Run<xofz.Framework.Timer>(t =>
                    {
                        subscriber.Subscribe(
                            t,
                            nameof(t.Elapsed),
                            this.timer_Elapsed);
                    },
                    DependencyNames.Timer);
            });

            w.Run<SetupHandler>(handler =>
            {
                handler.Handle(this.ui);
            });

            w.Run<Navigator>(n => n.RegisterPresenter(this));
        }

        public override void Start()
        {
            var w = this.web;
            w.Run<StartHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        public override void Stop()
        {
            var w = this.web;
            w.Run<StopHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_CopyHostnameKeyTapped()
        {
            var w = this.web;
            w.Run<CopyHostnameKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_CopyCurrentIpKeyTapped()
        {
            var w = this.web;
            w.Run<CopyCurrentIpKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_CopySyncedIpKeyTapped()
        {
            var w = this.web;
            w.Run<CopySyncedIpKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_StartSyncingKeyTapped()
        {
            var w = this.web;
            w.Run<StartSyncingKeyTappedHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_StopSyncingKeyTapped()
        {
            var w = this.web;
            w.Run<StopSyncingKeyTappedHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_ExitRequested()
        {
            var w = this.web;
            var nav = w.Run<Navigator>();
            Do shutdown = nav.Present<ShutdownPresenter>;
            w.Run<ExitRequestedHandler>(handler =>
            {
                handler.Handle(
                    this.ui,
                    shutdown);
            });
        }

        private void ui_InstallServiceRequested()
        {
            var w = this.web;
            var nav = w.Run<Navigator>();
            Do shutdown = nav.Present<ShutdownPresenter>;
            w.Run<InstallServiceRequestedHandler>(handler =>
            {
                handler.Handle(
                    this.ui,
                    shutdown);
            });
        }

        private void ui_UninstallServiceRequested()
        {
            var w = this.web;
            var nav = w.Run<Navigator>();
            Do shutdown = nav.Present<ShutdownPresenter>;
            w.Run<UninstallServiceRequestedHandler>(handler =>
            {
                handler.Handle(
                    this.ui,
                    shutdown);
            });
        }

        private void timer_Elapsed()
        {
            var w = this.web;
            w.Run<TimerHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private long setupIf1;
        private readonly HomeUi ui;
        private readonly MethodWeb web;
    }
}
