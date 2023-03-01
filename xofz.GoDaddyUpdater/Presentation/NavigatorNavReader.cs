namespace xofz.GoDaddyUpdater.Presentation
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Framework;
    using xofz.Presentation;
    using xofz.Presentation.Presenters;

    public sealed class NavigatorNavReader
        : NavReader
    {
        public NavigatorNavReader(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        void NavReader.ReadShutdown(
            out Do go)
        {
            var r = this.runner;
            Do result = null;
            r?.Run<Navigator>(nav =>
            {
                result = nav.Present<ShutdownPresenter>;
            });

            go = result;
        }

        private readonly MethodRunner runner;
    }
}
