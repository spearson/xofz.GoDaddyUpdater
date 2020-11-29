namespace xofz.GoDaddyUpdater.Framework.Home
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.UI;
    using xofz.UI;

    public class SetupHandler
    {
        public SetupHandler(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual void Handle(
            HomeUi ui)
        {
            var r = this.runner;
            r?.Run<GlobalSettingsHolder, UiReaderWriter>(
                (settings, uiRW) =>
                {
                    var startKeyEnabled = !settings.AutoStart;
                    uiRW.WriteSync(
                        ui,
                        () =>
                        {
                            ui.StartSyncingKeyEnabled = startKeyEnabled;
                            ui.StopSyncingKeyEnabled = !startKeyEnabled;
                        });
                });

            r?.Run<GlobalSettingsHolder, UiReaderWriter>(
                (settings, uiRW) =>
                {
                    var hostname = settings.Subdomain + '.' + settings.Domain;
                    var ipProviderUri = settings.HttpExternalIpProviderUri;
                    uiRW.Write(
                        ui,
                        () =>
                        {
                            ui.Hostname = hostname;
                            ui.IpProviderUri = ipProviderUri;
                        });
                });

            r?.Run<VersionReader, UiReaderWriter>(
                (vr, uiRW) =>
                {
                    var version = vr.Read();
                    var coreVersion = vr.ReadCoreVersion();
                    uiRW.Write(
                        ui,
                        () =>
                        {
                            ui.Version = version;
                            ui.CoreVersion = coreVersion;
                        });
                });
        }

        protected readonly MethodRunner runner;
    }
}
