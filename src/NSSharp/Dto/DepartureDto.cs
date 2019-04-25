using System;

namespace NSSharp.Dto
{
    internal class DepartureDto
    {
        public string direction { get; set; }
        public string name { get; set; }
        public DateTime plannedDateTime { get; set; }
        public int plannedTimeZoneOffset { get; set; }
        public DateTime actualDateTime { get; set; }
        public int actualTimeZoneOffset { get; set; }
        public string plannedTrack { get; set; }
        public ProductDto product { get; set; }
        public string trainCategory { get; set; }
        public bool cancelled { get; set; }
        public RoutestationDto[] routeStations { get; set; }
        public string departureStatus { get; set; }

        public Departure ToDeparture()
        {
            throw new NotImplementedException();
        }
    }
}