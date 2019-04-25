using System;
using System.Net.Http;

namespace NSSharp.Exceptions
{
    public class GetDeparturesFailed : Exception
    {
        private GetDeparturesFailed(string message) : base(message)
        {
        }

        public static GetDeparturesFailed Create(HttpResponseMessage response)
        {
            return new GetDeparturesFailed(response.ReasonPhrase);
        }
    }
}