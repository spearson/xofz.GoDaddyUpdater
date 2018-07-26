namespace xofz.GoDaddyUpdater.Framework
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using xofz.Framework;

    public class HttpClientFactory
    {
        public HttpClientFactory(MethodWeb web)
        {
            this.web = web;
        }

        public virtual HttpClient Create()
        {
            return new HttpClient();
        }

        public virtual HttpClient CreateGoDaddy()
        {
            var w = this.web;
            var client = new HttpClient();
            w.Run<GlobalSettingsHolder>(s =>
            {
                client
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue(
                    "sso-key",
                    s.PublicApiKey + ':' + s.PrivateApiKey);
            });

            return client;
        }

        private readonly MethodWeb web;
    }
}
