namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using System;
    using xofz.Framework;

    public class StartHandler
    {
        public StartHandler(MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle()
        {
            var w = this.web;
            w.Run<TimerHandler>(handler =>
            {
                handler.Handle();
            });

            w.Run<xofz.Framework.Timer>(t =>
            {
                t.Start(TimeSpan.FromMinutes(5));
            });
        }

        protected readonly MethodWeb web;
    }
}
