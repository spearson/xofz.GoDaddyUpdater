namespace xofz.GoDaddyUpdater.Framework.SettingsProviders
{
    using xofz.GoDaddyUpdater.Properties;

    public sealed class AppConfigSettingsProvider : SettingsProvider
    {
        GlobalSettingsHolder SettingsProvider.Provide()
        {
            return new GlobalSettingsHolder()
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
    }
}
