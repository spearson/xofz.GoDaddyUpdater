namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StartHandler
    {
        public StartHandler(
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
                    r.Run<EventRaiser>(er =>
                    {
                        r.Run<Messages, UiReaderWriter>(
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

                    t.Start(
                        System.TimeSpan.FromMinutes(5));
                },
                DependencyNames.Timer);
        }

        protected readonly MethodRunner runner;
    }
}
