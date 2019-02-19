namespace xofz.GoDaddyUpdater.Root
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.Presentation;
    using xofz.Root;
    using xofz.Root.Commands;
    using xofz.UI;
    using xofz.UI.Forms;
    using xofz.GoDaddyUpdater.Framework.SettingsProviders;
    using xofz.GoDaddyUpdater.Presentation;
    using xofz.GoDaddyUpdater.Root.Commands;
    using xofz.GoDaddyUpdater.Root.Implementation;
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
            if (Interlocked.CompareExchange(
                    ref this.bootstrappedIf1, 
                    1, 
                    0) == 1)
            {
                return;
            }

            this.setMainForm(new MainForm());
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
            var w = new MethodWebV2();
            Messenger fm = new FormsMessenger();
            fm.Subscriber = s;
            e.Execute(new SetupMethodWebCommand(
                new AppConfigSettingsProvider(),
                fm,
                w));
            UnhandledExceptionEventHandler handler = this.onUnhandledException;
            w.Run<EventSubscriber>(subscriber =>
            {
                var cd = AppDomain.CurrentDomain;
                subscriber.Subscribe(
                    cd,
                    nameof(cd.UnhandledException),
                    handler);
            });

            e.Execute(new SetupHomeCommand(
                    s,
                    new FormsClipboardCopier(),
                    w));
            ThreadPool.QueueUserWorkItem(o =>
            {
                e.Execute(new SetupShutdownCommand(
                    w));
            });
                
            w.Run<Navigator>(n => n.Present<HomePresenter>());
        }

        protected virtual void setMainForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        protected virtual void onUnhandledException(
            object sender, 
            UnhandledExceptionEventArgs e)
        {
            var w = this.executor.Get<SetupMethodWebCommand>().W;
            w.Run<LogEditor>(le =>
                {
                    LogHelpers.AddEntry(le, e);
                },
                "Exceptions");
        }

        protected MainForm mainForm;
        protected long bootstrappedIf1;
        protected readonly CommandExecutor executor;
    }
}
