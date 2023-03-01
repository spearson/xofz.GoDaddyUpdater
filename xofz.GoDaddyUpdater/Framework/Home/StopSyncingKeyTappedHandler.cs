namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StopSyncingKeyTappedHandler
    {
        public StopSyncingKeyTappedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<UiReaderWriter>(uiRw =>
            {
                const bool
                    truth = true,
                    falsity = false;
                uiRw.Write(
                    ui,
                    () =>
                    {
                        ui.StartSyncingKeyDisabled = falsity;
                        ui.StopSyncingKeyDisabled = truth;
                    });
            });
        }

        protected readonly MethodRunner runner;
    }
}
