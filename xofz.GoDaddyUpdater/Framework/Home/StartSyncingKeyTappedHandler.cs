namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StartSyncingKeyTappedHandler
    {
        public StartSyncingKeyTappedHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t =>
                {
                    t.Stop();
                    w.Run<LatchHolder>(latch =>
                        {
                            latch.Latch.WaitOne();
                        },
                        DependencyNames.TimerLatch);
                },
                DependencyNames.Timer);

            w.Run<UiReaderWriter>(uiRw =>
            {
                uiRw.WriteSync(
                    ui,
                    () =>
                    {
                        ui.StartSyncingKeyEnabled = false;
                        ui.StopSyncingKeyEnabled = true;
                    });
            });

            w.Run<TimerHandler>(handler =>
            {
                handler.Handle(ui);
            });

            w.Run<xofz.Framework.Timer>(t =>
                {
                    t.Start(TimeSpan.FromMinutes(5));
                },
                DependencyNames.Timer);
        }

        protected readonly MethodWeb web;
    }
}
