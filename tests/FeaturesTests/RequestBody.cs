using System.Diagnostics;
using Gherkin.Ast;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(RequestBody) + Constants.FeatureFileExtension)]
    public sealed class RequestBody : BaseFeature
    {

        public RequestBody(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [When(@"calling the method (\w+) with parameters ""(.+)""")]
        public async Task CallMethodWithParameters(string methodName, string jsonTextObect)
        {
            var inlineObj = _testHelper.JsonToTypeInstance("ObjectResponse", jsonTextObect);

            await CallMethod(methodName, new object[] { inlineObj });
        }

        [And(@"the request should have a body with value ""(.+)""")]
        public async Task CheckRequestBody(string propValue)
        {
            Assert.NotNull(_request);
            var body = await _request.Content.ReadAsStringAsync();
            Assert.NotNull(body);
            body = body.Replace("\"", "'");
            Assert.Equal(propValue, body);
        }
    }
}
