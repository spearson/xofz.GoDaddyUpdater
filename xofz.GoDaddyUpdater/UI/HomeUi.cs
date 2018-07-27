namespace xofz.GoDaddyUpdater.UI
{
    using xofz.UI;

    public interface HomeUi : Ui
    {
        event Action StartSyncingKeyTapped;

        event Action StopSyncingKeyTapped;

        event Action CopyHostnameKeyTapped;

        event Action CopyCurrentIpKeyTapped;

        event Action CopySyncedIpKeyTapped;

        event Action ExitRequested;

        string Hostname { get; set; }

        string CurrentIP { get; set; }

        string SyncedIP { get; set; }

        string LastChecked { get; set; }

        string LastSynced { get; set; }

        string Version { get; set; }

        string CoreVersion { get; set; }

        bool StartSyncingKeyEnabled { get; set; }

        bool StopSyncingKeyEnabled { get; set; }
    }
}
