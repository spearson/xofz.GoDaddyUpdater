namespace xofz.GoDaddyUpdater.UI
{
    using xofz.UI;

    public interface HomeUi : Ui
    {
        event Do StartSyncingKeyTapped;

        event Do StopSyncingKeyTapped;

        event Do CopyHostnameKeyTapped;

        event Do CopyCurrentIpKeyTapped;

        event Do CopySyncedIpKeyTapped;

        event Do ExitRequested;

        event Do InstallServiceRequested;

        event Do UninstallServiceRequested;

        string Hostname { get; set; }

        string IpProviderUri { get; set; }

        string CurrentIP { get; set; }

        string SyncedIP { get; set; }

        string LastChecked { get; set; }

        string LastSynced { get; set; }

        string Version { get; set; }

        string CoreVersion { get; set; }

        bool StartSyncingKeyDisabled { get; set; }

        bool StopSyncingKeyDisabled { get; set; }

        bool ServiceInstalled { get; set; }

        void HideNotifyIcon();
    }
}
