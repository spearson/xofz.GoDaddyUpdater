namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class CopyHostnameKeyTappedHandler
    {
        public CopyHostnameKeyTappedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<UiReaderWriter, ClipboardCopier>(
                (uiRW, copier) =>
                {
                    uiRW.Write(
                        ui,
                        () =>
                        {
                            copier.Copy(
                                ui.Hostname);
                        });
                });
        }

        protected readonly MethodRunner runner;
    }
}
