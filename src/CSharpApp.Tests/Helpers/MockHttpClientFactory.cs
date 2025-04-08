using System.Net;
using System.Text;

namespace CSharpApp.Tests.Helpers
{
    public class MockHttpClientFactory
    {
        public static HttpClient CreateHttpClient(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var handler = new MockHttpResponseMessage(httpResponseMessage);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.escuelajs.co/api/v1/")
            };

            return httpClient;
        }
    }
}
