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

        [When(@"calling the method (\w+) with parameter(s*) ""(.+)""")]
        public async Task CallMethodWithStringParameters(string methodName, string isSimpleParam, string parametersString)
        {
            var parameters = isSimpleParam == "s"
            ? parametersString.Split(",").Length == 2
                ? new object[] { parametersString.Split(",") }
                : parametersString.Split(",")
            : new object[] { _testHelper.JsonToTypeInstance("InlineObject1", parametersString) };

            await CallMethod(methodName, parameters);
        }
    }
}
