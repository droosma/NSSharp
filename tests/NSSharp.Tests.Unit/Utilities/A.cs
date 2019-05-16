using System;

using Microsoft.AspNetCore.TestHost;

using NSSharp.Entities;

namespace NSSharp.Tests.Unit.Utilities
{
    internal static class A
    {
        public static DateTimeOffset DateTimeOffset
            => new DateTimeOffset(2019, 5, 9, 15, 0, 0, 0, TimeSpan.Zero);

        public static UicCode UicCode => UicCode.Create("random-UicCode");
        public static StationCode StationCode => StationCode.Create("random-stationCode");
        public static DepartureRequest DepartureRequest => DepartureRequest.Create(StationCode);

        public static IJsonConverter JsonConverter => new NewtonsoftJsonConverter();

        public static NsTravelInformation NsTravelInformation(this TestServer server)
            => NsSharpFactory.Create("subscriptionKey", server.CreateClient(), JsonConverter).NsTravelInformation();

        public static Departures Departures(this TestServer server)
            => new NsDepartures(server.CreateClient, JsonConverter);
    }
}