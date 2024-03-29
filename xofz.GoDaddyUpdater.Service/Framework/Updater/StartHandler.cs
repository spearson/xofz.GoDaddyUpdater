﻿namespace xofz.GoDaddyUpdater.Service.Framework.Updater
{
    using xofz.Framework;

    public class StartHandler
    {
        public StartHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle()
        {
            var r = this.runner;
            r?.Run<TimerHandler>(handler =>
            {
                handler.Handle();
            });

            r?.Run<xofz.Framework.Timer>(t =>
            {
                t.Start(
                    System.TimeSpan.FromMinutes(5));
            });
        }

        protected readonly MethodRunner runner;
    }
}
