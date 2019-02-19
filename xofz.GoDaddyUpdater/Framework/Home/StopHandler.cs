namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;

    public class StopHandler
    {
        public StopHandler(
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
        }

        protected readonly MethodWeb web;
    }
}
