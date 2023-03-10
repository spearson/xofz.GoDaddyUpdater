namespace xofz.GoDaddyUpdater.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            this.startSyncingKey = new System.Windows.Forms.Button();
            this.stopSyncingKey = new System.Windows.Forms.Button();
            this.lastSyncedLabel = new System.Windows.Forms.Label();
            this.lastCheckedLabel = new System.Windows.Forms.Label();
            this.hostnameLabel = new System.Windows.Forms.Label();
            this.currentIpLabel = new System.Windows.Forms.Label();
            this.syncedIpLabel = new System.Windows.Forms.Label();
            this.copyHostnameKey = new System.Windows.Forms.Button();
            this.copyCurrentIpKey = new System.Windows.Forms.Button();
            this.copySyncedIpKey = new System.Windows.Forms.Button();
            this.coreVersionLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(17, 82);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 16);
            label1.TabIndex = 99;
            label1.Text = "Current IP:";
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(13, 115);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 16);
            label2.TabIndex = 100;
            label2.Text = "Synced IP:";
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            label3.Location = new System.Drawing.Point(12, 194);
            label3.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(105, 16);
            label3.TabIndex = 99;
            label3.Text = "Last checked at:";
            // 
            // label4
            // 
            label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label4.ForeColor = System.Drawing.SystemColors.ControlDark;
            label4.Location = new System.Drawing.Point(20, 216);
            label4.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(97, 16);
            label4.TabIndex = 99;
            label4.Text = "Last synced at:";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label5.Location = new System.Drawing.Point(12, 48);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(73, 16);
            label5.TabIndex = 102;
            label5.Text = "Hostname:";
            // 
            // startSyncingKey
            // 
            this.startSyncingKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startSyncingKey.AutoSize = true;
            this.startSyncingKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startSyncingKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.startSyncingKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.startSyncingKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startSyncingKey.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startSyncingKey.Location = new System.Drawing.Point(12, 144);
            this.startSyncingKey.Name = "startSyncingKey";
            this.startSyncingKey.Size = new System.Drawing.Size(222, 44);
            this.startSyncingKey.TabIndex = 0;
            this.startSyncingKey.Text = "Start Syncing";
            this.startSyncingKey.UseVisualStyleBackColor = true;
            this.startSyncingKey.Click += new System.EventHandler(this.startSyncingKey_Click);
            // 
            // stopSyncingKey
            // 
            this.stopSyncingKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopSyncingKey.AutoSize = true;
            this.stopSyncingKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stopSyncingKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.stopSyncingKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.stopSyncingKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopSyncingKey.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopSyncingKey.Location = new System.Drawing.Point(305, 144);
            this.stopSyncingKey.Name = "stopSyncingKey";
            this.stopSyncingKey.Size = new System.Drawing.Size(207, 44);
            this.stopSyncingKey.TabIndex = 1;
            this.stopSyncingKey.Text = "Stop Syncing";
            this.stopSyncingKey.UseVisualStyleBackColor = true;
            this.stopSyncingKey.Click += new System.EventHandler(this.stopSyncingKey_Click);
            // 
            // lastSyncedLabel
            // 
            this.lastSyncedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastSyncedLabel.AutoSize = true;
            this.lastSyncedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastSyncedLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lastSyncedLabel.Location = new System.Drawing.Point(117, 216);
            this.lastSyncedLabel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lastSyncedLabel.Name = "lastSyncedLabel";
            this.lastSyncedLabel.Size = new System.Drawing.Size(34, 16);
            this.lastSyncedLabel.TabIndex = 99;
            this.lastSyncedLabel.Text = "N/A";
            // 
            // lastCheckedLabel
            // 
            this.lastCheckedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastCheckedLabel.AutoSize = true;
            this.lastCheckedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastCheckedLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lastCheckedLabel.Location = new System.Drawing.Point(117, 194);
            this.lastCheckedLabel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lastCheckedLabel.Name = "lastCheckedLabel";
            this.lastCheckedLabel.Size = new System.Drawing.Size(34, 16);
            this.lastCheckedLabel.TabIndex = 99;
            this.lastCheckedLabel.Text = "N/A";
            // 
            // hostnameLabel
            // 
            this.hostnameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hostnameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hostnameLabel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hostnameLabel.Location = new System.Drawing.Point(91, 42);
            this.hostnameLabel.Margin = new System.Windows.Forms.Padding(3);
            this.hostnameLabel.Name = "hostnameLabel";
            this.hostnameLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.hostnameLabel.Size = new System.Drawing.Size(359, 28);
            this.hostnameLabel.TabIndex = 99;
            this.hostnameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentIpLabel
            // 
            this.currentIpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentIpLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentIpLabel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentIpLabel.Location = new System.Drawing.Point(91, 76);
            this.currentIpLabel.Margin = new System.Windows.Forms.Padding(3);
            this.currentIpLabel.Name = "currentIpLabel";
            this.currentIpLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.currentIpLabel.Size = new System.Drawing.Size(359, 28);
            this.currentIpLabel.TabIndex = 103;
            this.currentIpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syncedIpLabel
            // 
            this.syncedIpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.syncedIpLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.syncedIpLabel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.syncedIpLabel.Location = new System.Drawing.Point(91, 110);
            this.syncedIpLabel.Margin = new System.Windows.Forms.Padding(3);
            this.syncedIpLabel.Name = "syncedIpLabel";
            this.syncedIpLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.syncedIpLabel.Size = new System.Drawing.Size(359, 28);
            this.syncedIpLabel.TabIndex = 104;
            this.syncedIpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // copyHostnameKey
            // 
            this.copyHostnameKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyHostnameKey.AutoSize = true;
            this.copyHostnameKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.copyHostnameKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.copyHostnameKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.copyHostnameKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.copyHostnameKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyHostnameKey.Location = new System.Drawing.Point(456, 42);
            this.copyHostnameKey.Name = "copyHostnameKey";
            this.copyHostnameKey.Size = new System.Drawing.Size(56, 28);
            this.copyHostnameKey.TabIndex = 105;
            this.copyHostnameKey.Text = "Copy";
            this.copyHostnameKey.UseVisualStyleBackColor = true;
            this.copyHostnameKey.Click += new System.EventHandler(this.copyHostnameKey_Click);
            // 
            // copyCurrentIpKey
            // 
            this.copyCurrentIpKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyCurrentIpKey.AutoSize = true;
            this.copyCurrentIpKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.copyCurrentIpKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.copyCurrentIpKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.copyCurrentIpKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.copyCurrentIpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyCurrentIpKey.Location = new System.Drawing.Point(456, 76);
            this.copyCurrentIpKey.Name = "copyCurrentIpKey";
            this.copyCurrentIpKey.Size = new System.Drawing.Size(56, 28);
            this.copyCurrentIpKey.TabIndex = 106;
            this.copyCurrentIpKey.Text = "Copy";
            this.copyCurrentIpKey.UseVisualStyleBackColor = true;
            this.copyCurrentIpKey.Click += new System.EventHandler(this.copyCurrentIpKey_Click);
            // 
            // copySyncedIpKey
            // 
            this.copySyncedIpKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copySyncedIpKey.AutoSize = true;
            this.copySyncedIpKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.copySyncedIpKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.copySyncedIpKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.copySyncedIpKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.copySyncedIpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copySyncedIpKey.Location = new System.Drawing.Point(456, 110);
            this.copySyncedIpKey.Name = "copySyncedIpKey";
            this.copySyncedIpKey.Size = new System.Drawing.Size(56, 28);
            this.copySyncedIpKey.TabIndex = 107;
            this.copySyncedIpKey.Text = "Copy";
            this.copySyncedIpKey.UseVisualStyleBackColor = true;
            this.copySyncedIpKey.Click += new System.EventHandler(this.copySyncedIpKey_Click);
            // 
            // coreVersionLabel
            // 
            this.coreVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.coreVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coreVersionLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.coreVersionLabel.Location = new System.Drawing.Point(324, 228);
            this.coreVersionLabel.Margin = new System.Windows.Forms.Padding(0);
            this.coreVersionLabel.Name = "coreVersionLabel";
            this.coreVersionLabel.Size = new System.Drawing.Size(191, 24);
            this.coreVersionLabel.TabIndex = 99;
            this.coreVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.versionLabel.Location = new System.Drawing.Point(324, 204);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(191, 24);
            this.versionLabel.TabIndex = 99;
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "GoDaddyUpdater";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(524, 32);
            this.menuStrip1.TabIndex = 108;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installServiceToolStripMenuItem,
            this.uninstallServiceToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileMenuItem.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(70, 28);
            this.fileMenuItem.Text = "File";
            // 
            // installServiceToolStripMenuItem
            // 
            this.installServiceToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installServiceToolStripMenuItem.Name = "installServiceToolStripMenuItem";
            this.installServiceToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.installServiceToolStripMenuItem.Text = "&Install Service";
            this.installServiceToolStripMenuItem.Click += new System.EventHandler(this.installServiceToolStripMenuItem_Click);
            // 
            // uninstallServiceToolStripMenuItem
            // 
            this.uninstallServiceToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uninstallServiceToolStripMenuItem.Name = "uninstallServiceToolStripMenuItem";
            this.uninstallServiceToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.uninstallServiceToolStripMenuItem.Text = "&Uninstall Service";
            this.uninstallServiceToolStripMenuItem.Click += new System.EventHandler(this.uninstallServiceToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(524, 261);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.coreVersionLabel);
            this.Controls.Add(this.copySyncedIpKey);
            this.Controls.Add(this.copyCurrentIpKey);
            this.Controls.Add(this.copyHostnameKey);
            this.Controls.Add(this.syncedIpLabel);
            this.Controls.Add(this.currentIpLabel);
            this.Controls.Add(this.hostnameLabel);
            this.Controls.Add(label5);
            this.Controls.Add(this.lastSyncedLabel);
            this.Controls.Add(this.lastCheckedLabel);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.stopSyncingKey);
            this.Controls.Add(this.startSyncingKey);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "x(z) GoDaddyUpdater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.this_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button startSyncingKey;
        private System.Windows.Forms.Button stopSyncingKey;
        private System.Windows.Forms.Label lastSyncedLabel;
        private System.Windows.Forms.Label lastCheckedLabel;
        private System.Windows.Forms.Label hostnameLabel;
        private System.Windows.Forms.Label currentIpLabel;
        private System.Windows.Forms.Label syncedIpLabel;
        private System.Windows.Forms.Button copyHostnameKey;
        private System.Windows.Forms.Button copyCurrentIpKey;
        private System.Windows.Forms.Button copySyncedIpKey;
        private System.Windows.Forms.Label coreVersionLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

