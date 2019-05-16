using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;

namespace NSSharp.Tests.Unit.Utilities
{
    internal class TestServerBuilder : IDisposable
    {
        private readonly List<Func<HttpContext, Func<Task>, Task>> _middleware = new List<Func<HttpContext, Func<Task>, Task>>();
        private RequestDelegate _response;
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
            _response = context =>
            {
                context.Response.ContentType = responseContentType;
                context.Response.StatusCode = (int)statusCode;
                return context.Response.WriteAsync(responseBody);
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
            var webHostBuilder = new WebHostBuilder().Configure(Configure);
            _testServer = new TestServer(webHostBuilder);
            return _testServer;
        }

        private void Configure(IApplicationBuilder app)
        {
            foreach(var mw in _middleware)
            {
                app.Use(mw);
            }

            app.Run(_response);
        }

        public TestServerBuilder WithMiddleware(params Func<HttpContext, Func<Task>, Task>[] middleware)
        {
            if(middleware.Any(mw => mw == null))
                throw new ArgumentNullException();

            _middleware.AddRange(middleware);
            return this;
        }
    }
}