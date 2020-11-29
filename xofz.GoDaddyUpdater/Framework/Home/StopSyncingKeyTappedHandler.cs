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
                uiRw.Write(
                    ui,
                    () =>
                    {
                        ui.StartSyncingKeyEnabled = true;
                        ui.StopSyncingKeyEnabled = false;
                    });
            });
        }

        protected readonly MethodRunner runner;
    }
}
