namespace xofz.GoDaddyUpdater.Framework
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
            var r = this.runner;
            var client = default(HttpClient);
            r.Run<Gen<HttpMessageHandler>>(
                createHandler =>
                {
                    var handler = createHandler();
                    if (handler == default(HttpMessageHandler))
                    {
                        client = new HttpClient();
                        return;
                    }

                    client = new HttpClient(handler);
                });

            return client;
        }

        public virtual HttpClient CreateGoDaddy()
        {
            var r = this.runner;
            var client = default(HttpClient);
            r.Run<Gen<HttpMessageHandler>, GlobalSettingsHolder>(
                (createHandler, settings) =>
            {
                var handler = createHandler();
                if (handler == default(HttpMessageHandler))
                {
                    client = new HttpClient();
                    goto setHeaderAuth;
                }

                client = new HttpClient(handler);

                setHeaderAuth:
                client
                    .DefaultRequestHeaders
                    .Authorization = new AuthenticationHeaderValue(
                        @"sso-key",
                        settings.PublicApiKey + ':' + settings.PrivateApiKey);
            });

            return client;
        }

        protected readonly MethodRunner runner;
    }
}
