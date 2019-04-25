using System;

using NSSharp.Entities;

namespace NSSharp.Tests.Unit
{
    internal static class A
    {
        public static DateTimeOffset DateTimeOffset
            => new DateTimeOffset(2019, 5, 9, 15, 0, 0, 0, TimeSpan.Zero);

        public static UicCode UicCode => UicCode.Create("random-UicCode");
        public static StationCode StationCode => StationCode.Create("random-stationCode");
        public static DepartureRequest DepartureRequest => DepartureRequest.Create(StationCode);
    }
}