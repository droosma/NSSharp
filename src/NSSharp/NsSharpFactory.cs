using System;
using System.Net.Http;

namespace NSSharp
{
    public sealed class NsSharpFactory
    {
        private const string nsPublicTravelInformationApiAddress = "https://gateway.apiportal.ns.nl/public-reisinformatie/api/";

        private readonly Uri _host;
        private readonly IJsonConverter _jsonConverter;
        private readonly string _subscriptionKey;

        private HttpMessageHandler _messageHandler;
        private Action<TimeSpan, string> _requestTime;

        private NsSharpFactory(string subscriptionKey,
                               Uri host,
                               IJsonConverter jsonConverter)
        {
            if(string.IsNullOrWhiteSpace(subscriptionKey))
                throw new ArgumentNullException(nameof(subscriptionKey));
            _subscriptionKey = subscriptionKey;

            _host = host ?? throw new ArgumentNullException(nameof(host));

            _jsonConverter = jsonConverter ?? throw new ArgumentNullException(nameof(jsonConverter));
        }

        public NsTravelInformation NsTravelInformation()
        {
            var httpClient = CreateHttpClient();

            var departures = new NsDepartures(HttpClientFactory, _jsonConverter, _requestTime);
            return new TravelInformation(departures);

            HttpClient HttpClientFactory() => httpClient;
        }

        public NsSharpFactory WithRequestTimeMetric(Action<TimeSpan, string> requestTime)
        {
            _requestTime = requestTime;
            return this;
        }

        public static NsSharpFactory Create(string subscriptionKey,
                                            IJsonConverter jsonConverter)
            => new NsSharpFactory(subscriptionKey, new Uri(nsPublicTravelInformationApiAddress), jsonConverter);

        public NsSharpFactory With(HttpMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
            return this;
        }

        private HttpClient CreateHttpClient()
        {
            const string subscriptionKeyHeaderName = "Ocp-Apim-Subscription-Key";

            var messageHandler = _messageHandler ?? new HttpClientHandler();
            var client = new HttpClient(messageHandler)
                         {
                             BaseAddress = _host
                         };
            client.DefaultRequestHeaders.Add(subscriptionKeyHeaderName, _subscriptionKey);

            return client;
        }

        private class TravelInformation : NsTravelInformation
        {
            public TravelInformation(Departures departures)
            {
                Departures = departures;
            }

            public Departures Departures { get; }
        }
    }
}