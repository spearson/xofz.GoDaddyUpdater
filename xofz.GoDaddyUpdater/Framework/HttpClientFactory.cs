namespace xofz.GoDaddyUpdater.Framework
{
    using System;
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
            var w = this.web;
            var client = default(HttpClient);
            w.Run<Func<HttpMessageHandler>>(
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
            var w = this.web;
            var client = default(HttpClient);
            w.Run<Func<HttpMessageHandler>, GlobalSettingsHolder>(
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
                        "sso-key",
                        settings.PublicApiKey + ':' + settings.PrivateApiKey);
            });

            return client;
        }

        private readonly MethodWeb web;
    }
}
