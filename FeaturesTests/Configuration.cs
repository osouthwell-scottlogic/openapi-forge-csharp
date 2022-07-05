using Xunit.Gherkin.Quick;
using Xunit;
using Xunit.Abstractions;
using Gherkin.Ast;
using RichardSzalay.MockHttp;

namespace Features
{
    [FeatureFile(nameof(Configuration) + Constants.FeatureFileExtension)]
    public sealed class Configuration : BaseFeature
    {
        private int? _serverIndex;

        public Configuration(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Given(@"an API with the following specification")]
        public void Generate(DocString schema)
        {
            Assert.False(string.IsNullOrWhiteSpace(schema.Content), $"Parameter '{nameof(schema)}' must not be null or whitespace");
            _docStringContent = schema.Content;
            _testHelper.GenerateApi(schema.Content);
        }

        ///   Scenario: When there are multiple servers, the user can select which configuration to use
        [When(@"calling the method (\w+) without params")]
        public async Task CallWithoutParameters(string methodName)
        {
            _mockHttp.ReplyWithRequestUrl();

            var apiClient = _testHelper.CreateApiClient(_mockHttp.ToHttpClient(), _serverIndex);

            var methodInfo = apiClient.GetType().GetMethod(methodName);

            dynamic awaitable = methodInfo.Invoke(apiClient, null);
            await awaitable;
            _actual = awaitable.GetAwaiter().GetResult();
        }

        [Then(@"the requested URL should be (.+)")]
        public void CheckResponseType(string url)
        {
            Assert.Equal(url, _actual);
        }

        [When(@"selecting the server at index (\d)")]
        public void SelectedServerIndexIsOne(string serverIndex)
        {
            _serverIndex = new Nullable<int>(int.Parse(serverIndex));
        }
    }
}