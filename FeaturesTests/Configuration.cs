using Xunit.Gherkin.Quick;
using Xunit;
using Xunit.Abstractions;
using Gherkin.Ast;
using RichardSzalay.MockHttp;

namespace Features
{
    [FeatureFile(nameof(Configuration) + Constants.FeatureFileExtension)]
    public sealed class Configuration : Xunit.Gherkin.Quick.Feature
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly TestHelper _testHelper;

        private readonly MockHttpMessageHandler _mockHttp;

        private object _actual;

        private int? _serverIndex;

        private string _docStringContent;

        public Configuration(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testHelper = new TestHelper(System.Guid.NewGuid().ToString().Substring(0, 8));
            _mockHttp = new MockHttpMessageHandler();
        }

        [Given(@"an API with the following specification")]
        public void Generate(DocString schema)
        {
            Assert.False(string.IsNullOrWhiteSpace(schema.Content), $"Parameter '{nameof(schema)}' must not be null or whitespace");
            _docStringContent = schema.Content;
            try
            {
                _testHelper.GenerateApi(schema.Content);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        ///   Scenario: When there are multiple servers, the user can select which configuration to use
        [When(@"calling the method (\w+) without params")]
        public async Task CallWithoutParameters(string methodName)
        {
            try
            {
                _mockHttp.ReplyWithRequestUrl();

                var apiClient = _testHelper.CreateApiClient(_mockHttp.ToHttpClient(), _serverIndex);

                var methodInfo = apiClient.GetType().GetMethod(methodName);

                dynamic awaitable = methodInfo.Invoke(apiClient, null);
                await awaitable;
                _actual = awaitable.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
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