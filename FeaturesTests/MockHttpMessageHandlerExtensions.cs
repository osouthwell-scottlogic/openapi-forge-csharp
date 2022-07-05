using RichardSzalay.MockHttp;

namespace Features
{
    public static class MockHttpMessageHandlerExtensions
    {
        public static MockedRequest ReplyWithRequestUrl(this MockHttpMessageHandler handler)
        {
            return handler.When("*").Respond((HttpRequestMessage request) =>
                {
                    return Task.FromResult<HttpResponseMessage>(
                        new HttpResponseMessage
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Content = new StringContent(request.RequestUri.ToString())
                        });
                });
        }
    }
}