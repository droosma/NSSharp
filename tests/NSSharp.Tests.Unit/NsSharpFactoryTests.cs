using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

using NSSharp.Tests.Unit.Utilities;
using NSSharp.Utilities;

using Xunit;

namespace NSSharp.Tests.Unit
{
    public class NsSharpFactoryTests
    {
        private const string departureResource = "NSSharp.Tests.Unit.Responses.departures-8400280.json";

        [Fact]
        public async Task All_GivenUnsuccessfulResponse_ThrowsException()
        {
            const string subscriptionKey = "subscriptionKey";
            var requestReceived = new ManualResetEvent(false);

            using(var server = TestServerBuilder.New()
                                                .WithResponseFromJsonResource(departureResource, HttpStatusCode.OK)
                                                .WithMiddleware(RequestListener)
                                                .Build())
            {
                var tranInformation = NsSharpFactory.Create(subscriptionKey, server.CreateClient(), A.JsonConverter)
                                                    .NsTravelInformation();

                await tranInformation.Departures.All(DepartureRequest.Create(A.StationCode));

                requestReceived.WaitOne(1500, false)
                               .Should().BeTrue();
            }

            Task RequestListener(HttpContext context, Func<Task> next)
            {
                context.Request.Headers.Values.Should().Contain(subscriptionKey);
                context.Request.GetDisplayUrl().Should().Contain(Constants.NsPublicTravelInformationApiAddress);

                requestReceived.Set();
                return next();
            }
        }
    }
}