using System;
using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using NSSharp.Exceptions;
using NSSharp.Tests.Unit.Utilities;

using Xunit;

namespace NSSharp.Tests.Unit
{
    public class DeparturesTests
    {
        private const string departureResource = "NSSharp.Tests.Unit.Responses.departures-8400280.json";

        [Fact]
        public void All_GivenUnsuccessfulResponse_ThrowsException()
        {
            using(var server = TestServerBuilder.New()
                                                .WithResponseFromJsonResource(departureResource, HttpStatusCode.InternalServerError)
                                                .Build())
            {
                var handler = server.CreateHandler();
                var travelInformation = A.NsTravelInformation(handler);

                var sut = travelInformation.Departures;

                Func<Task> all = () => sut.All(DepartureRequest.Create(A.StationCode));

                all.Should().Throw<GetDeparturesFailed>();
            }
        }

        [Fact]
        public async Task All_GivenSuccessfulResponse_ReturnsDepartures()
        {
            using(var server = TestServerBuilder.New()
                                                .WithResponseFromJsonResource(departureResource, HttpStatusCode.OK)
                                                .Build())
            {
                var handler = server.CreateHandler();
                var travelInformation = A.NsTravelInformation(handler);

                var sut = travelInformation.Departures;

                var departures = await sut.All(DepartureRequest.Create(A.StationCode));

                departures.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task All_GivenRequestTime_ReportsTimeOfRequest()
        {
            const string subscriptionKey = "subscriptionKey";

            using(var server = TestServerBuilder.New()
                                                .WithResponseFromJsonResource(departureResource, HttpStatusCode.OK)
                                                .Build())
            {
                var handler = server.CreateHandler();
                (TimeSpan, string) requestTime = (TimeSpan.Zero, string.Empty);
                var travelInformation = NsSharpFactory.Create(subscriptionKey, A.JsonConverter)
                                                      .With(handler)
                                                      .WithRequestTimeMetric((span, s) => requestTime = (span, s))
                                                      .NsTravelInformation();

                var sut = travelInformation.Departures;

                await sut.All(DepartureRequest.Create(A.StationCode));

                requestTime.Should().NotBeNull();
                requestTime.Item2.Should().NotBeNullOrEmpty();
                requestTime.Item1.Should().BeGreaterThan(TimeSpan.Zero);
            }
        }
    }
}