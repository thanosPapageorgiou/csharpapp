
namespace CSharpApp.Tests.Helpers
{
    class MockHttpResponseMessage: HttpMessageHandler
    {
        private readonly HttpResponseMessage _mockResponse;

        public MockHttpResponseMessage(HttpResponseMessage mockResponse)
        {
            _mockResponse = mockResponse;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mockResponse);
        }
    }
}
