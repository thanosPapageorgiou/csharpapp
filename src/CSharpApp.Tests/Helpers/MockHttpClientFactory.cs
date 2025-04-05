using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Tests.Helpers
{
    public class MockHttpClientFactory
    {
        public static HttpClient CreateHttpClient(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            // Create a mock HttpResponseMessage with the provided JSON content
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            // Create a mock HttpMessageHandler
            var handler = new MockHttpResponseMessage(httpResponseMessage);

            // Create HttpClient using the mock handler
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.escuelajs.co/api/v1/")
            };

            return httpClient;
        }
    }
}
