namespace xofz.GoDaddyUpdater.Service
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.updaterServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.updaterServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // updaterServiceProcessInstaller
            // 
            this.updaterServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.updaterServiceProcessInstaller.Password = null;
            this.updaterServiceProcessInstaller.Username = null;
            // 
            // updaterServiceInstaller
            // 
            this.updaterServiceInstaller.ServiceName = "GoDaddyUpdater.Service";
            this.updaterServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.updaterServiceProcessInstaller,
            this.updaterServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller updaterServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller updaterServiceInstaller;
    }
}