using System;
using System.Net.Http;

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

        public static NsTravelInformation NsTravelInformation(HttpMessageHandler handler)
            => NsSharpFactory.Create("subscriptionKey", JsonConverter).With(handler).NsTravelInformation();
    }
}