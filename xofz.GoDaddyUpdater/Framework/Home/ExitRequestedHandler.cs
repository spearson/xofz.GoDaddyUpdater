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
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<NavReader>(reader =>
            {
                r.Run<UiReaderWriter>(uiRW =>
                {
                    uiRW.WriteSync(
                        ui,
                        ui.HideNotifyIcon);
                });

                reader.ReadShutdown(
                    out var go);
                go?.Invoke();
            });
        }

        protected readonly MethodRunner runner;
    }
}
