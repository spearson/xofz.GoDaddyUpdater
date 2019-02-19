namespace xofz.GoDaddyUpdater.Framework.Home
{
    using System;
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StartHandler
    {
        public StartHandler(MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<xofz.Framework.Timer>(t =>
                {
                    w.Run<EventRaiser>(er =>
                    {
                        w.Run<Messages, UiReaderWriter>(
                            (messages, uiRw) =>
                            {
                                var waitingMessage = messages.Waiting;
                                uiRw.Write(
                                    ui,
                                    () =>
                                    {
                                        ui.CurrentIP = waitingMessage;
                                        ui.SyncedIP = waitingMessage;
                                    });
                            });

                        er.Raise(
                            t,
                            nameof(t.Elapsed));
                    });
                    t.Start(TimeSpan.FromMinutes(5));
                },
                DependencyNames.Timer);
        }

        protected readonly MethodWeb web;
    }
}
