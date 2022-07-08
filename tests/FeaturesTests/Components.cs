using Gherkin.Ast;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(Components) + Constants.FeatureFileExtension)]
    public sealed class Components : FeatureBase
    {
        public Components(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [When(@"calling the method (\w+) and the server responds with")]
        public async Task CallWithResponse(string methodName, DocString response)
        {
            await CallMethod(methodName, null, response.Content);
        }

        [Then(@"the response should be of type (\w+)")]
        public void CheckResponseType(string type)
        {
            Assert.EndsWith(type, _actual.GetType().Name);
        }

        [And(@"the response should have a property id with value (\d+)")]
        public void CheckResponseIdProperty(string propValue)
        {
            var propInfo = _actual.GetType().GetProperty("id");
            Assert.NotNull(propInfo);
            Assert.Equal(int.Parse(propValue), propInfo.GetValue(_actual));
        }

        [And(@"the response should have a property value with value (\w+)")]
        public void CheckResponseValueProperty(string propValue)
        {
            var propInfo = _actual.GetType().GetProperty("value");
            Assert.NotNull(propInfo);
            Assert.Equal(propValue, propInfo.GetValue(_actual));
        }

        [When(@"calling the method (\w+) with parameters ""(.+)""")]
        public async Task CallMethodWithParameters(string methodName, string rawParameters)
        {
            var paramStringValues = rawParameters.Split(",");
            var parameters = new object[] { paramStringValues[0], int.TryParse(paramStringValues[1], out var parsed) ? new Nullable<int>(parsed) : null };

            await CallMethod(methodName, parameters);
        }
    }
}
