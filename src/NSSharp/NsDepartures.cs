using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using NSSharp.Dto;
using NSSharp.Exceptions;
using NSSharp.Utilities;

namespace NSSharp
{
    internal class NsDepartures : Departures
    {
        private readonly Func<HttpClient> _httpClientFactory;
        private readonly IJsonConverter _jsonConverter;
        private readonly Action<TimeSpan, string> _requestTime;

        public NsDepartures(Func<HttpClient> httpClientFactory,
                            IJsonConverter jsonConverter,
                            Action<TimeSpan, string> requestTime = null)
        {
            _httpClientFactory = httpClientFactory;
            _jsonConverter = jsonConverter;
            _requestTime = requestTime;
        }

        public async Task<IReadOnlyList<Departure>> All(DepartureRequest request)
        {
            var (response, elapsed) = await Timeing.Time(RequestDepartures);
            _requestTime?.Invoke(elapsed, RequestTimeSource(nameof(All)));

            if(!response.IsSuccessStatusCode)
                throw GetDeparturesFailed.Create(response);

            var departureBoardDto = await response.Content.Read<RequestEnvelopeDto<DepartureBoardDto>>(_jsonConverter);
            return departureBoardDto.Payload
                                    .Departures.Select(d => d.ToDeparture())
                                    .ToList();

            Task<HttpResponseMessage> RequestDepartures()
                => _httpClientFactory().GetAsync($"v2/departures?{request.Build()}");
        }

        private string RequestTimeSource(string method)
            => $"{nameof(Departures)}.{method}";
    }
}