using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

using NSSharp.Entities;
using NSSharp.Utilities;

namespace NSSharp
{
    public class DepartureRequest
    {
        private readonly StationCode _stationCode;
        private readonly UicCode _uicCode;
        private DateTimeOffset? _dateTime;
        private Language? _language;
        private int? _numberOfResults;
        private string _source;

        private DepartureRequest(UicCode uicCode)
        {
            _uicCode = uicCode ?? throw new ArgumentNullException(nameof(uicCode));
        }

        private DepartureRequest(StationCode stationCode)
        {
            _stationCode = stationCode ?? throw new ArgumentNullException(nameof(stationCode));
        }

        public DepartureRequest With(Language language)
        {
            _language = language;
            return this;
        }

        public DepartureRequest With(DateTimeOffset dateTime)
        {
            _dateTime = dateTime;
            return this;
        }

        public DepartureRequest With(int numberOfResults)
        {
            _numberOfResults = numberOfResults;
            return this;
        }

        public static DepartureRequest Create(StationCode stationCode)
            => new DepartureRequest(stationCode);

        public static DepartureRequest Create(UicCode uicCode)
            => new DepartureRequest(uicCode);

        internal string Build() => NameValueCollection().ToString();

        private NameValueCollection NameValueCollection()
        {
            const string dateTimeParameterName = "dateTime";
            const string maxJourneysParameterName = "maxJourneys";
            const string languageParameterName = "lang";
            const string stationParameterName = "station";
            const string uicCodeParameterName = "uicCode";
            const string sourceParameterName = "source";

            var collection = HttpUtility.ParseQueryString(string.Empty);

            if(_dateTime.HasValue)
                collection[dateTimeParameterName] = _dateTime.Value.ToString("s", CultureInfo.InvariantCulture);
            if(_numberOfResults.HasValue)
                collection[maxJourneysParameterName] = _numberOfResults.Value.ToString();
            if(_language.HasValue)
                collection[languageParameterName] = _language.Value.AsNamedValue();
            if(_stationCode != null)
                collection[stationParameterName] = _stationCode.ToString();
            if(_uicCode != null)
                collection[uicCodeParameterName] = _uicCode.ToString();
            if(!string.IsNullOrWhiteSpace(_source))
                collection[sourceParameterName] = _source;

            return collection;
        }
    }
}