namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;

    public class ExitRequestedHandler
    {
        public ExitRequestedHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui,
            Do shutdown)
        {
            shutdown?.Invoke();
        }

        protected readonly MethodWeb web;
    }
}
