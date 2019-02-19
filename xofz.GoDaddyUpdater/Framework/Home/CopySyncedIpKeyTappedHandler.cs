namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class CopySyncedIpKeyTappedHandler
    {
        public CopySyncedIpKeyTappedHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<UiReaderWriter, ClipboardCopier>(
                (uiRw, copier) =>
                {
                    uiRw.Write(
                        ui,
                        () => copier.Copy(
                            ui.SyncedIP));
                });
        }

        protected readonly MethodWeb web;
    }
}
