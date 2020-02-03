namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
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
            r.Run<xofz.Framework.Timer>(t =>
                {
                    t.Stop();
                    r.Run<LatchHolder>(latch =>
                        {
                            latch.Latch.WaitOne();
                        },
                        DependencyNames.TimerLatch);
                },
                DependencyNames.Timer);

            r.Run<UiReaderWriter>(uiRw =>
            {
                uiRw.WriteSync(
                    ui,
                    () =>
                    {
                        ui.StartSyncingKeyEnabled = false;
                        ui.StopSyncingKeyEnabled = true;
                    });
            });

            r.Run<TimerHandler>(handler =>
            {
                handler.Handle(ui);
            });

            r.Run<xofz.Framework.Timer>(
                t =>
                {
                    t.Start(TimeSpan.FromMinutes(5));
                },
                DependencyNames.Timer);
        }

        protected readonly MethodRunner runner;
    }
}
