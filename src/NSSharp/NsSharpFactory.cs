using System;
using System.Net.Http;

namespace NSSharp
{
    public sealed class NsSharpFactory
    {
        private const string nsPublicTravelInformationApiAddress = "https://gateway.apiportal.ns.nl/public-reisinformatie/api/";
        private readonly Func<HttpClient> _httpClientFactory;
        private readonly IJsonConverter _jsonConverter;
        private Action<TimeSpan, string> _requestTime;

        private NsSharpFactory(Func<HttpClient> httpClientFactory,
                               IJsonConverter jsonConverter)
        {
            _httpClientFactory = httpClientFactory;
            _jsonConverter = jsonConverter;
        }

        public NsTravelInformation NsTravelInformation()
        {
            var departures = new NsDepartures(_httpClientFactory, _jsonConverter, _requestTime);
            return new TravelInformation(departures);
        }

        public NsSharpFactory WithRequestTimeMetric(Action<TimeSpan, string> requestTime)
        {
            _requestTime = requestTime;
            return this;
        }

        public static NsSharpFactory Create(string key,
                                            IJsonConverter jsonConverter)
            => Create(key, nsPublicTravelInformationApiAddress, jsonConverter);

        public static NsSharpFactory Create(string key,
                                            string host,
                                            IJsonConverter jsonConverter)
            => new NsSharpFactory(() => CreateHttpClient(key, host), jsonConverter);

        private static HttpClient CreateHttpClient(string key, string host)
        {
            if(string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            const string subscriptionKeyHeaderName = "Ocp-Apim-Subscription-Key";

            var client = new HttpClient
                         {
                             BaseAddress = new Uri(host)
                         };
            client.DefaultRequestHeaders.Add(subscriptionKeyHeaderName, key);

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