using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Tests.Helpers
{
    class MockHttpResponseMessage: HttpMessageHandler
    {
        private readonly HttpResponseMessage _testResponse;

        public MockHttpResponseMessage(HttpResponseMessage fakeResponse)
        {
            _testResponse = fakeResponse;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_testResponse);
        }
    }
}
