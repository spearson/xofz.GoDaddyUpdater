namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class SetupHandler
    {
        public SetupHandler(
            MethodWeb web)
        {
            this.web = web;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var w = this.web;
            w.Run<GlobalSettingsHolder, UiReaderWriter>(
                (settings, uiRw) =>
                {
                    var startKeyEnabled = !settings.AutoStart;
                    uiRw.WriteSync(
                        ui,
                        () =>
                        {
                            ui.StartSyncingKeyEnabled = startKeyEnabled;
                            ui.StopSyncingKeyEnabled = !startKeyEnabled;
                        });
                });

            w.Run<GlobalSettingsHolder, UiReaderWriter>(
                (settings, uiRw) =>
                {
                    var hostname = settings.Subdomain + '.' + settings.Domain;
                    uiRw.Write(
                        ui,
                        () => ui.Hostname = hostname);
                    var ipProviderUri = settings.HttpExternalIpProviderUri;
                    uiRw.Write(
                        ui,
                        () => ui.IpProviderUri = ipProviderUri);
                });

            w.Run<VersionReader, UiReaderWriter>(
                (vr, uiRw) =>
                {
                    var version = vr.Read();
                    var coreVersion = vr.ReadCoreVersion();
                    uiRw.Write(
                        ui,
                        () =>
                        {
                            ui.Version = version;
                            ui.CoreVersion = coreVersion;
                        });
                });
        }

        protected readonly MethodWeb web;
    }
}
