namespace xofz.GoDaddyUpdater.Service
{
    using System.Collections;
    using System.ComponentModel;
    using System.ServiceProcess;
    using xofz.GoDaddyUpdater.Service.Framework;
    using xofz.GoDaddyUpdater.Service.Framework.SettingsProviders;

    [RunInstaller(true)]
    public partial class ProjectInstaller 
        : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            SettingsProvider sp = new ExeConfigSettingsProvider();
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
                installer.DisplayName = "GoDaddyUpdater.Service ["
                    + settings.Subdomain
                    + '.'
                    + settings.Domain
                    + "] ("
                    + settings.HttpExternalIpProviderUri
                    + ')';
                installer.ServiceName = "gdu."
                    + settings.Subdomain
                    + '.'
                    + settings.Domain
                    + '.'
                    + settings
                        .HttpExternalIpProviderUri
                        .Replace('/', '-');
                
                break;
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            var installer = this.updaterServiceInstaller;
            using (var sc = new ServiceController(installer.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
