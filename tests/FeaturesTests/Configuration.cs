using Gherkin.Ast;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(Configuration) + Constants.FeatureFileExtension)]
    public sealed class Configuration : BaseFeature
    {
        private int? _serverIndex;

        public Configuration(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [When(@"calling the method (\w+) without params")]
        public async Task CallWithoutParameters(string methodName)
        {
            await CallMethod(methodName, null, null, _serverIndex);
        }

        [When(@"selecting the server at index (\d)")]
        public void SelectedServerIndexIsOne(string serverIndex)
        {
            _serverIndex = new Nullable<int>(int.Parse(serverIndex));
        }
    }
}
