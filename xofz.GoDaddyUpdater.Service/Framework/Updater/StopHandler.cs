namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using xofz.Framework;

    public class StopHandler
    {
        public StopHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle()
        {
            var r = this.runner;
            r.Run<xofz.Framework.Timer>(t =>
            {
                t.Stop();
            });
        }

        protected readonly MethodRunner runner;
    }
}
