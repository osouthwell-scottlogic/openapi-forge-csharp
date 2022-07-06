using RichardSzalay.MockHttp;

namespace Features
{
    public static class MockHttpMessageHandlerExtensions
    {
        public static MockedRequest ReplyWithRequestUrl(this MockHttpMessageHandler handler, Action<HttpRequestMessage> requestInterceptor = null)
        {
            return handler.When("*").Respond((HttpRequestMessage request) =>
                {
                    requestInterceptor?.Invoke(request);

                    return Task.FromResult(
                        new HttpResponseMessage
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Content = new StringContent(request.RequestUri.AbsoluteUri.ToString()),
                            RequestMessage = request
                        });
                });
        }
    }
}
