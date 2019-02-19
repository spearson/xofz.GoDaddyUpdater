namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class StopSyncingKeyTappedHandler
    {
        public StopSyncingKeyTappedHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<UiReaderWriter>(uiRw =>
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

        protected readonly MethodWeb web;
    }
}
