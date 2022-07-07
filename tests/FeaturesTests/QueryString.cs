using Gherkin.Ast;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(QueryString) + Constants.FeatureFileExtension)]
    public sealed class QueryString : BaseFeature
    {
        public QueryString(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Given(@"an API with the following specification")]
        public void Generate(DocString schema)
        {
            Assert.False(string.IsNullOrWhiteSpace(schema.Content), $"Parameter '{nameof(schema)}' must not be null or whitespace");
            _docStringContent = schema.Content;
            _testHelper.GenerateApi(schema.Content);
        }

        [When(@"calling the method (\w+) with (?:params|parameter|parameters) ""(.+)""")]
        public async Task CallMethodWithStringParameters(string methodName, string parametersString)
        {
            var parameters = methodName switch
            {
                "sendStringArray" => new object[] { parametersString.Split(",") },
                "sendValueObject" => new object[] { _testHelper.JsonToTypeInstance("InlineObject1", parametersString) },
                _ => parametersString.Split(",")
            };

            await CallMethod(methodName, parameters);
        }

        [When(@"calling the method (\w+) without params")]
        public async Task CallMethodWithStringParameters(string methodName)
        {
            await CallMethod(methodName);
        }
    }
}
