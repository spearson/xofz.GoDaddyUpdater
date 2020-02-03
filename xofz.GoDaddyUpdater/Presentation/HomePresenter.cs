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
            MethodRunner runner)
            : base(ui, null)
        {
            this.ui = ui;
            this.runner = runner;
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

            var r = this.runner;
            r.Run<EventSubscriber>(subscriber =>
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
                r.Run<xofz.Framework.Timer>(t =>
                    {
                        subscriber.Subscribe(
                            t,
                            nameof(t.Elapsed),
                            this.timer_Elapsed);
                    },
                    DependencyNames.Timer);
            });

            r.Run<SetupHandler>(handler =>
            {
                handler.Handle(this.ui);
            });

            r.Run<Navigator>(nav => 
                nav.RegisterPresenter(this));
        }

        public override void Start()
        {
            var r = this.runner;
            r.Run<StartHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        public override void Stop()
        {
            var r = this.runner;
            r.Run<StopHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_CopyHostnameKeyTapped()
        {
            var r = this.runner;
            r.Run<CopyHostnameKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_CopyCurrentIpKeyTapped()
        {
            var r = this.runner;
            r.Run<CopyCurrentIpKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_CopySyncedIpKeyTapped()
        {
            var r = this.runner;
            r.Run<CopySyncedIpKeyTappedHandler>(
                handler =>
                {
                    handler.Handle(this.ui);
                });
        }

        private void ui_StartSyncingKeyTapped()
        {
            var r = this.runner;
            r.Run<StartSyncingKeyTappedHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_StopSyncingKeyTapped()
        {
            var r = this.runner;
            r.Run<StopSyncingKeyTappedHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private void ui_ExitRequested()
        {
            var r = this.runner;
            r.Run<Navigator>(nav =>
            {
                Do shutdown = nav.Present<ShutdownPresenter>;
                r.Run<ExitRequestedHandler>(handler =>
                {
                    handler.Handle(
                        this.ui,
                        shutdown);
                });
            });
        }

        private void ui_InstallServiceRequested()
        {
            var r = this.runner;
            r.Run<Navigator>(nav =>
            {
                Do shutdown = nav.Present<ShutdownPresenter>;
                r.Run<InstallServiceRequestedHandler>(handler =>
                {
                    handler.Handle(
                        this.ui,
                        shutdown);
                });
            });
        }

        private void ui_UninstallServiceRequested()
        {
            var r = this.runner;
            r.Run<Navigator>(nav =>
            {
                Do shutdown = nav.Present<ShutdownPresenter>;
                r.Run<UninstallServiceRequestedHandler>(handler =>
                {
                    handler.Handle(
                        this.ui,
                        shutdown);
                });
            });
        }

        private void timer_Elapsed()
        {
            var r = this.runner;
            r.Run<TimerHandler>(handler =>
            {
                handler.Handle(this.ui);
            });
        }

        private long setupIf1;
        private readonly HomeUi ui;
        private readonly MethodRunner runner;
    }
}
