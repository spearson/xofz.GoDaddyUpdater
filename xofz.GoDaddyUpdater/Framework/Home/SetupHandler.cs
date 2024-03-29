﻿namespace xofz.GoDaddyUpdater.Framework.Home
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
            r?.Run<UiReaderWriter>(uiRW =>
            {
                r?.Run<GlobalSettingsHolder>(
                    settings =>
                    {
                        var startKeyDisabled = settings.AutoStart;
                        uiRW.WriteSync(
                            ui,
                            () =>
                            {
                                ui.StartSyncingKeyDisabled = startKeyDisabled;
                                ui.StopSyncingKeyDisabled = !startKeyDisabled;
                            });
                    });

                r?.Run<GlobalSettingsHolder>(
                    settings =>
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

                r?.Run<VersionReader>(
                    vr =>
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
            });
        }

        protected readonly MethodRunner runner;
    }
}
