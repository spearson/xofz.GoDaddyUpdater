namespace xofz.GoDaddyUpdater.UI.Forms
{
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;
    using xofz.GoDaddyUpdater.Properties;
    

    public partial class MainForm 
        : FormUi, HomeUi
    {
        public MainForm()
        {
            this.InitializeComponent();

            this.Icon = Resources.GoDaddyUpdater_Icon;
            var ni = this.notifyIcon;
            ni.Icon = Resources.GoDaddyUpdater_Icon;
            var cm = new ContextMenu();
            var exitMenuItem = new MenuItem("Exit");
            exitMenuItem.Click += this.exitMenuItem_Click;
            cm.MenuItems.Add(exitMenuItem);
            ni.ContextMenu = cm;

            var h = this.Handle;
        }

        public event Action StartSyncingKeyTapped;

        public event Action StopSyncingKeyTapped;

        public event Action CopyHostnameKeyTapped;

        public event Action CopyCurrentIpKeyTapped;

        public event Action CopySyncedIpKeyTapped;

        public event Action ExitRequested;

        string HomeUi.Hostname
        {
            get => this.hostnameLabel.Text;

            set => this.hostnameLabel.Text = value;
        }

        string HomeUi.CurrentIP
        {
            get => this.currentIpLabel.Text;

            set => this.currentIpLabel.Text = value;
        }

        string HomeUi.SyncedIP
        {
            get => this.syncedIpLabel.Text;

            set => this.syncedIpLabel.Text = value;
        }

        string HomeUi.LastChecked
        {
            get
            {
                return this.lastCheckedLabel.Text;
            }

            set
            {
                this.lastCheckedLabel.Text = value;
            }
        }

        string HomeUi.LastSynced
        {
            get
            {
                return this.lastSyncedLabel.Text;
            }

            set
            {
                this.lastSyncedLabel.Text = value;
            }
        }

        bool HomeUi.StartSyncingKeyEnabled
        {
            get => this.startSyncingKey.Enabled;

            set => this.startSyncingKey.Enabled = value;
        }

        bool HomeUi.StopSyncingKeyEnabled
        {
            get => this.stopSyncingKey.Enabled;

            set => this.stopSyncingKey.Enabled = value;
        }

        private const string VersionFlavorText = @"v";

        string HomeUi.Version
        {
            get
            {
                var text = this.versionLabel.Text;
                if (text?.Contains(VersionFlavorText)
                    ?? false)
                {
                    return text.Substring(VersionFlavorText.Length);
                }

                return string.Empty;
            }

            set => this.versionLabel.Text = VersionFlavorText + value;
        }

        private const string CoreVersionFlavorText = @"Powered by xofz.Core98 v";

        string HomeUi.CoreVersion
        {
            get
            {
                var text = this.coreVersionLabel.Text;
                if (text?.Contains(CoreVersionFlavorText)
                    ?? false)
                {
                    return text.Substring(CoreVersionFlavorText.Length);
                }

                return string.Empty;
            }

            set => this.coreVersionLabel.Text = CoreVersionFlavorText + value;
        }

        private void startSyncingKey_Click(object sender, System.EventArgs e)
        {
            var sskt = this.StartSyncingKeyTapped;
            if(sskt == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o => sskt.Invoke());
        }

        private void stopSyncingKey_Click(object sender, System.EventArgs e)
        {
            var sskt = this.StopSyncingKeyTapped;
            if (sskt == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o => sskt.Invoke());
        }

        private void copyHostnameKey_Click(object sender, System.EventArgs e)
        {
            var chkt = this.CopyHostnameKeyTapped;
            if (chkt == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o => chkt.Invoke());
        }

        private void copyCurrentIpKey_Click(object sender, System.EventArgs e)
        {
            var ccikt = this.CopyCurrentIpKeyTapped;
            if (ccikt == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o => ccikt.Invoke());
        }

        private void copySyncedIpKey_Click(object sender, System.EventArgs e)
        {
            var csikt = this.CopySyncedIpKeyTapped;
            if (csikt == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o => csikt.Invoke());
        }

        private void notifyIcon_Click(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                    return;
                }

                this.Visible = false;
                return;
            }

            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void this_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void exitMenuItem_Click(
            object sender, 
            System.EventArgs e)
        {
            var er = this.ExitRequested;
            if (er == null)
            {
                return;
            }

            this.notifyIcon.Visible = false;
            ThreadPool.QueueUserWorkItem(
                o => er.Invoke());
        }
    }
}
