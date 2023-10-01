namespace xofz.GoDaddyUpdater.Root
{
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.GoDaddyUpdater.Framework;
    using xofz.Presentation;
    using xofz.Root;
    using xofz.Root.Commands;
    using xofz.UI;
    using xofz.UI.Forms.Messengers;
    using xofz.UI.Forms.UiMessagePumpers;
    using xofz.GoDaddyUpdater.Framework.SettingsProviders;
    using xofz.GoDaddyUpdater.Presentation;
    using xofz.GoDaddyUpdater.Root.Commands;
    using xofz.GoDaddyUpdater.UI.Forms;

    public class FormsBootstrapper
    {
        public FormsBootstrapper()
            : this(new CommandExecutor())
        {
        }

        public FormsBootstrapper(
            CommandExecutor executor)
        {
            this.executor = executor;
        }

        public virtual Form Shell => this.mainForm;

        public virtual void Bootstrap()
        {
            const byte one = 1;
            if (Interlocked.Exchange(
                    ref this.bootstrappedIf1, 
                    one) == one)
            {
                return;
            }

            this.setMainForm(
                new MainForm());
            var finished = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(o =>
            {
                this.onBootstrap();
                finished.Set();
            });

            UiMessagePumper pumper = new FormsUiMessagePumper();
            while (!finished.WaitOne(0, false))
            {
                pumper.Pump();
            }
        }

        protected virtual void onBootstrap()
        {
            var s = this.mainForm;
            var e = this.executor;
            if (e == null)
            {
                return;
            }

            Messenger fm = new FormsMessenger();
            fm.Subscriber = s;
            e.Execute(new SetupMethodWebCommand(
                new AppConfigSettingsProvider(),
                fm,
                new MethodWebV2()));

            var w = e.Get<SetupMethodWebCommand>()?.W;
            if (w == null)
            {
                return;
            }

            System.UnhandledExceptionEventHandler handler = this.onUnhandledException;
            w.Run<EventSubscriber>(subscriber =>
            {
                var cd = System.AppDomain.CurrentDomain;
                subscriber.Subscribe(
                    cd,
                    nameof(cd.UnhandledException),
                    handler);
            });

            e
                .Execute(new SetupHomeCommand(
                    s,
                    new FormsClipboardCopier(),
                    w))
                .Execute(new SetupShutdownCommand(
                    w));
                
            w.Run<Navigator>(nav => 
                nav.Present<HomePresenter>());
        }

        protected virtual void setMainForm(
            MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        protected virtual void onUnhandledException(
            object sender, 
            System.UnhandledExceptionEventArgs e)
        {
            var w = this.executor?.Get<SetupMethodWebCommand>()?.W;
            w?.Run<LogEditor>(le =>
                {
                    LogHelpers.AddEntry(le, e);
                },
                LogNames.Exceptions);
        }

        protected MainForm mainForm;
        protected long bootstrappedIf1;
        protected readonly CommandExecutor executor;
    }
}
