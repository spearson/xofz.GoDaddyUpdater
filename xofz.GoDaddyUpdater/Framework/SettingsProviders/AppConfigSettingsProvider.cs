namespace xofz.GoDaddyUpdater.Framework.SettingsProviders
{
    using xofz.GoDaddyUpdater.Properties;

    public sealed class AppConfigSettingsProvider : SettingsProvider
    {
        GlobalSettingsHolder SettingsProvider.Provide()
        {
            return new GlobalSettingsHolder()
            {
                PublicApiKey = Settings.Default.PublicApiKey,
                PrivateApiKey = Settings.Default.PrivateApiKey,
                Domain = Settings.Default.Domain,
                Subdomain = Settings.Default.Subdomain
            };
        }
    }
}
