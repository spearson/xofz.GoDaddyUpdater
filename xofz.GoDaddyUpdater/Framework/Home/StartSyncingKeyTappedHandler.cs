namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StartSyncingKeyTappedHandler
    {
        public StartSyncingKeyTappedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<xofz.Framework.Timer>(t =>
                {
                    t.Stop();
                    r.Run<LatchHolder>(latch =>
                        {
                            latch.Latch.WaitOne();
                        },
                        DependencyNames.TimerLatch);
                },
                DependencyNames.Timer);

            r?.Run<UiReaderWriter>(uiRw =>
            {
                const bool
                    truth = true,
                    falsity = false;
                uiRw.WriteSync(
                    ui,
                    () =>
                    {
                        ui.StartSyncingKeyDisabled = truth;
                        ui.StopSyncingKeyDisabled = falsity;
                    });
            });

            r?.Run<TimerHandler>(handler =>
            {
                handler.Handle(ui);
            });

            r?.Run<xofz.Framework.Timer>(
                t =>
                {
                    t.Start(System.TimeSpan.FromMinutes(5));
                },
                DependencyNames.Timer);
        }

        protected readonly MethodRunner runner;
    }
}
