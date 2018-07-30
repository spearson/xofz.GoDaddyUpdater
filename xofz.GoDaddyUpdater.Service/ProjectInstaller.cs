namespace xofz.GoDaddyUpdater.Service
{
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.ServiceProcess;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.GlobalSettingsProviders;

    [RunInstaller(true)]
    public partial class ProjectInstaller 
        : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            GlobalSettingsProvider sp = new ExeConfigSettingsProvider();
            var settings = sp.Provide();
            foreach (var anyInstaller in this.Installers)
            {
                if(!ReferenceEquals(
                    anyInstaller,
                    this.updaterServiceInstaller))
                {
                    continue;
                }

                var installer = (ServiceInstaller)anyInstaller;
                installer.ServiceName = "GoDaddyUpdater.Service ["
                    + settings.Subdomain
                    + "."
                    + settings.Domain
                    + "]";
                installer.DisplayName = installer.ServiceName;
                break;
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            var installer = this.updaterServiceInstaller;
            base.OnAfterInstall(savedState);
            System.IO.File.WriteAllText(
                "service name.txt",
                installer.ServiceName);
            using (var sc = new ServiceController(installer.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
