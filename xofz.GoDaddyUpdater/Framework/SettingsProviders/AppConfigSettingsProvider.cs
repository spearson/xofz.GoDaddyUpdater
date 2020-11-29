namespace xofz.GoDaddyUpdater.Framework.SettingsProviders
{
    using xofz.Framework;
    using xofz.GoDaddyUpdater.Properties;

    public sealed class AppConfigSettingsProvider 
        : SettingsProvider
    {
        public AppConfigSettingsProvider()
        {
        }

        public AppConfigSettingsProvider(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        GlobalSettingsHolder SettingsProvider.Provide()
        {
            return new GlobalSettingsHolder
            {
                ServiceAttribution = @"(by Service)",
                PublicApiKey = Settings.Default.PublicApiKey,
                PrivateApiKey = Settings.Default.PrivateApiKey,
                Domain = Settings.Default.Domain,
                Subdomain = Settings.Default.Subdomain,
                HttpExternalIpProviderUri = Settings.Default.HttpExternalIpProviderUri,
                AutoStart = Settings.Default.AutoStart                
            };
        }

        private readonly MethodRunner runner;
    }
}
