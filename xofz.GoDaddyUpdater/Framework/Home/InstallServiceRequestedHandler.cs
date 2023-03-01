namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.UI;

    public class InstallServiceRequestedHandler
    {
        public InstallServiceRequestedHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle()
        {
            var r = this.runner;
            r?.Run<AdminChecker>(checker =>
            {
                if (!checker.CurrentUserIsAdmin())
                {
                    r.Run<Messenger, UiReaderWriter>((m, uiRW) =>
                    {
                        var response = uiRW.Read(
                            m.Subscriber,
                            () => m.Question(
                                @"The app needs to run as administrator first."
                                + System.Environment.NewLine
                                + @"Please try again after the app is running as administrator."
                                + System.Environment.NewLine
                                + @"Run the app as administrator?"));
                        if (response == Response.Yes)
                        {
                            r.Run<ProcessStarter>(starter =>
                            {
                                starter.RestartAsAdmin();
                            });
                        }
                    });

                    return;
                }

                r.Run<ProcessStarter>(starter =>
                {
                    starter.InstallService();
                });
            });
        }

        protected readonly MethodRunner runner;
    }
}
