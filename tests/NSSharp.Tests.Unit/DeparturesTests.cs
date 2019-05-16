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
                var sut = server.Departures();

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
                var sut = server.Departures();

                var departures = await sut.All(DepartureRequest.Create(A.StationCode));

                departures.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task All_GivenRequestTime_ReportsTimeOfRequest()
        {
            using(var server = TestServerBuilder.New()
                                                .WithResponseFromJsonResource(departureResource, HttpStatusCode.OK)
                                                .Build())
            {
                var httpClient = server.CreateClient();
                (TimeSpan, string) requestTime = (TimeSpan.Zero, string.Empty);
                var sut = new NsDepartures(() => httpClient, A.JsonConverter, (span, s) => requestTime = (span, s));

                await sut.All(DepartureRequest.Create(A.StationCode));

                requestTime.Should().NotBeNull();
                requestTime.Item2.Should().NotBeNullOrEmpty();
                requestTime.Item1.Should().BeGreaterThan(TimeSpan.Zero);
            }
        }
    }
}