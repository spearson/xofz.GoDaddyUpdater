namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class ExitRequestedHandler
    {
        public ExitRequestedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui,
            Do shutdown)
        {
            var r = this.runner;
            r.Run<UiReaderWriter>(uiRW =>
            {
                uiRW.WriteSync(
                    ui,
                    ui.HideNotifyIcon);
            });

            shutdown?.Invoke();
        }

        protected readonly MethodRunner runner;
    }
}
