namespace xofz.GoDaddyUpdater.Service.Framework
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using xofz.Framework;

    public class HttpClientFactory
    {
        public HttpClientFactory(
            MethodRunner runner)
        {
            this.runner = runner;
        }

        public virtual HttpClient Create()
        {
            return new HttpClient();
        }

        public virtual HttpClient CreateGoDaddy()
        {
            var r = this.runner;
            var client = new HttpClient();
            r?.Run<GlobalSettingsHolder>(settings =>
            {
                client
                    .DefaultRequestHeaders
                    .Authorization = new AuthenticationHeaderValue(
                        @"sso-key",
                        settings.PublicApiKey 
                        + ':' 
                        + settings.PrivateApiKey);
            });

            return client;
        }

        protected readonly MethodRunner runner;
    }
}
