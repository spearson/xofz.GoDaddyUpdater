namespace xofz.GoDaddyUpdater.Service.Framework.GlobalSettingsProviders
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using xofz.GoDaddyUpdater.Service.Framework;

    public sealed class ExeConfigSettingsProvider : GlobalSettingsProvider
    {
        GlobalSettingsHolder GlobalSettingsProvider.Provide()
        {
            var exePath = new StringBuilder()
                .Append(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location))
                .Append("\\")
                .Append(nameof(xofz))
                .Append('.')
                .Append(nameof(GoDaddyUpdater))
                .Append(".exe")
                .ToString();
            Configuration config;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(exePath);
            }
            catch
            {
                exePath = new StringBuilder()
                    .Append(Environment.CurrentDirectory)
                    .Append("\\")
                    .Append(nameof(xofz))
                    .Append('.')
                    .Append(nameof(GoDaddyUpdater))
                    .Append(".exe")
                    .ToString();
                config = ConfigurationManager.OpenExeConfiguration(exePath);
            }

            var section = config
                .SectionGroups["applicationSettings"]
                .Sections[
                    nameof(xofz) +
                    "." + 
                    nameof(GoDaddyUpdater) + 
                    ".Properties.Settings"]
                as ClientSettingsSection;
            var gsh = new GlobalSettingsHolder();
            foreach (SettingElement setting in section.Settings)
            {
                var value = setting?.Value?.ValueXml?.InnerText;
                switch (setting.Name)
                {
                    case "PublicApiKey":
                        gsh.PublicApiKey = value;
                        continue;
                    case "PrivateApiKey":
                        gsh.PrivateApiKey = value;
                        continue;
                    case "Domain":
                        gsh.Domain = value;
                        continue;
                    case "Subdomain":
                        gsh.Subdomain = value;
                        continue;
                    case "HttpExternalIpProviderUri":
                        gsh.HttpExternalIpProviderUri = value;
                        continue;
                }
            }

            return gsh;
        }
    }
}
