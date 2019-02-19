namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using xofz.Framework;

    public class StopHandler
    {
        public StopHandler(MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle()
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t =>
            {
                t.Stop();
            });
        }

        protected readonly MethodWeb web;
    }
}
