namespace xofz.GoDaddyUpdater.Framework
{
    public class GlobalSettingsHolder
    {
        public virtual string ServiceAttribution { get; set; }

        public virtual string PublicApiKey { get; set; }

        public virtual string PrivateApiKey { get; set; }

        public virtual string Domain { get; set; }

        public virtual string Subdomain { get; set; }

        // calls Trim() on the response string
        public virtual string HttpExternalIpProviderUri { get; set; }

        public virtual bool AutoStart { get; set; }
    }
}
