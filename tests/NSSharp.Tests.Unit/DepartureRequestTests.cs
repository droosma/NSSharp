using System;
using System.Web;

using FluentAssertions;

using NSSharp.Entities;
using NSSharp.Utilities;

using Xunit;

namespace NSSharp.Tests.Unit
{
    public class DepartureRequestTests
    {
        [Fact]
        public void With_GivenStationCode_SetsStationCodeOnBuild()
        {
            var stationCode = A.StationCode;
            var expectedStationCode = HttpUtility.UrlEncode(stationCode.ToString());

            var sut = DepartureRequest.Create(stationCode);

            sut.Build().Should().ContainEquivalentOf(expectedStationCode);
        }

        [Fact]
        public void With_GivenNullStationCode_ThrowsArgumentException()
        {
            Action createAction = () => DepartureRequest.Create((StationCode)null);

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void With_GivenUicCode_SetsUicCodeOnBuild()
        {
            var uicCode = A.UicCode;
            var expectedUicCode = HttpUtility.UrlEncode(uicCode.ToString());

            var sut = DepartureRequest.Create(uicCode);

            sut.Build().Should().ContainEquivalentOf(expectedUicCode);
        }

        [Fact]
        public void With_GivenNullUicCode_ThrowsArgumentException()
        {
            Action createAction = () => DepartureRequest.Create((UicCode)null);

            createAction.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void With_GivenLanguage_SetsLanguageOnBuild()
        {
            const Language language = Language.Dutch;
            var request = A.DepartureRequest.With(language);

            request.Build().Should().Contain(language.AsNamedValue());
        }

        [Fact]
        public void With_GivenDateTimeOffset_SetsDateTimeOffsetOnBuild()
        {
            var dateTime = A.DateTimeOffset;
            var expectedDateTime = HttpUtility.UrlEncode(dateTime.ToString("s"));

            var request = A.DepartureRequest.With(dateTime);

            request.Build().Should().Contain(expectedDateTime);
        }

        [Fact]
        public void With_GivenNumberOfResults_SetsNumberOfResultsOnBuild()
        {
            const int numberOfResults = 15;

            var request = A.DepartureRequest.With(numberOfResults);

            request.Build().Should().Contain(numberOfResults.ToString());
        }
    }
}