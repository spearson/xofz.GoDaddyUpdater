namespace xofz.GoDaddyUpdater.Root
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.Presentation;
    using xofz.Root;
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

        public virtual Form MainForm => this.mainForm;

        public virtual void Bootstrap()
        {
            if (Interlocked.CompareExchange(ref this.bootstrappedIf1, 1, 0) == 1)
            {
                return;
            }

            this.setMainForm(new MainForm());
            var s = this.mainForm;
            var e = this.executor;
            Messenger fm = new FormsMessenger();
            fm.Subscriber = s;
            e.Execute(new SetupMethodWebCommand(
                new AppConfigSettingsProvider(),
                    fm,
                    () => new MethodWeb()));
            var w = e.Get<SetupMethodWebCommand>().Web;
            UnhandledExceptionEventHandler handler = this.unhandledException;
            w.Run<EventSubscriber>(subscriber =>
            {
                var cd = AppDomain.CurrentDomain;
                subscriber.Subscribe(
                    cd,
                    nameof(cd.UnhandledException),
                    handler);
            });
            
            e
                .Execute(new SetupHomeCommand(
                    s,
                    new FormsClipboardCopier(),
                    w));
            w.Run<Navigator>(n => n.Present<HomePresenter>());
        }

        private void setMainForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        private void unhandledException(
            object sender, 
            UnhandledExceptionEventArgs e)
        {
            var w = this.executor.Get<SetupMethodWebCommand>().Web;
            w.Run<LogEditor>(le =>
                {
                    LogHelpers.AddEntry(le, e);
                },
                "Exceptions");
        }

        private MainForm mainForm;
        private long bootstrappedIf1;
        private readonly CommandExecutor executor;
    }
}
