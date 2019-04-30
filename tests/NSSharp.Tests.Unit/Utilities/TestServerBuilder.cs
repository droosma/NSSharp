using System;
using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;

namespace NSSharp.Tests.Unit.Utilities
{
    internal class TestServerBuilder : IDisposable
    {
        private Action<IApplicationBuilder> _appBuilderAction;
        private TestServer _testServer;

        private TestServerBuilder()
        {
        }

        public void Dispose()
        {
            _testServer?.Dispose();
        }

        public static TestServerBuilder New() => new TestServerBuilder();

        public TestServerBuilder WithResponse(string responseBody, string responseContentType, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var previousAppBuilderAction = _appBuilderAction;

            _appBuilderAction = app =>
            {
                previousAppBuilderAction?.Invoke(app);

                app.Run(
                    context =>
                    {
                        context.Response.ContentType = responseContentType;
                        context.Response.StatusCode = (int)statusCode;
                        return context.Response.WriteAsync(responseBody);
                    });
            };

            return this;
        }

        public TestServerBuilder WithResponseFromJsonResource(string resourceName, HttpStatusCode statusCode)
        {
            var requestResponse = ResourceReader.Read(resourceName);
            return WithJsonResponse(requestResponse, statusCode);
        }

        public TestServerBuilder WithJsonResponse(string jsonResponseBody, HttpStatusCode statusCode)
        {
            const string jsonContentType = "application/json";
            return WithResponse(jsonResponseBody, jsonContentType, statusCode);
        }

        public TestServer Build()
        {
            var webHostBuilder = new WebHostBuilder().Configure(_appBuilderAction);
            _testServer = new TestServer(webHostBuilder);
            return _testServer;
        }
    }
}