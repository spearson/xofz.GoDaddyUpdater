namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;

    public class StopHandler
    {
        public StopHandler(
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
        }

        protected readonly MethodRunner runner;
    }
}
