using System;
using System.Net.Http;

using NSSharp.Utilities;

namespace NSSharp
{
    public sealed class NsSharpFactory
    {
        private readonly Func<HttpClient> _httpClientFactory;
        private readonly IJsonConverter _jsonConverter;

        private Action<TimeSpan, string> _requestTime;

        private NsSharpFactory(Func<HttpClient> httpClientFactory,
                               IJsonConverter jsonConverter)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _jsonConverter = jsonConverter ?? throw new ArgumentNullException(nameof(jsonConverter));
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

        public static NsSharpFactory Create(string subscriptionKey,
                                            IJsonConverter jsonConverter)
        {
            return new NsSharpFactory(ConfigureHttpClient(subscriptionKey, new HttpClient()), jsonConverter);
        }

        public static NsSharpFactory Create(string subscriptionKey,
                                            HttpClient client,
                                            IJsonConverter jsonConverter)
        {
            return new NsSharpFactory(ConfigureHttpClient(subscriptionKey, client), jsonConverter);
        }

        private static Func<HttpClient> ConfigureHttpClient(string subscriptionKey, HttpClient client)
        {
            if(string.IsNullOrWhiteSpace(subscriptionKey))
                throw new ArgumentNullException(nameof(subscriptionKey));
            if(client == null)
                throw new ArgumentNullException(nameof(client));

            client.DefaultRequestHeaders.Add(Constants.SubscriptionKeyHeaderName, subscriptionKey);
            client.BaseAddress = new Uri(Constants.NsPublicTravelInformationApiAddress);

            return () => client;
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