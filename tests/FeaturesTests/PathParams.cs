using System.Diagnostics;
using Gherkin.Ast;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(PathParams) + Constants.FeatureFileExtension)]
    public sealed class PathParams : FeatureBase
    {
        public PathParams(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [When(@"calling the method (\w+) with (object|array|parameters) ""(.+)""")]
        public async Task CallMethodWithStringParameters(string methodName, string paramType, string parametersString)
        {
            var parameters = paramType switch
            {
                "object" => new object[] { _testHelper.JsonToTypeInstance("InlineObject1", parametersString) },
                "array" => new object[] { parametersString.Split(",") },
                _ => parametersString.Split(",")
            };

            await CallMethod(methodName, parameters);
        }
    }
}
