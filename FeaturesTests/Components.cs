using Xunit.Gherkin.Quick;
using Xunit;
using Xunit.Abstractions;
using Gherkin.Ast;
using RichardSzalay.MockHttp;

namespace Features
{
    [FeatureFile(nameof(Components) + Constants.FeatureFileExtension)]
    public sealed class Components : Xunit.Gherkin.Quick.Feature
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly TestHelper _testHelper;

        private readonly MockHttpMessageHandler _mockHttp;

        private object _actual;

        private string _docStringContent;

        public Components(ITestOutputHelper testOutputHelper)
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

        ///   Scenario: a response defined in the components section
        [When(@"calling the method (\w+) and the server responds with")]
        public async Task CallWithResponse(string methodName, DocString response)
        {
            try
            {
                var request = _mockHttp.When("*").Respond("application/json", response.Content);
                var apiClient = _testHelper.CreateApiClient(_mockHttp.ToHttpClient());

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

        /// Scenario: a parameter defined in the components section
        /// Given an API with the following specification...
        [When(@"calling the method (\w+) with parameters ""(.+)""")]
        public async Task CallMethodWithParameters(string methodName, string rawParameters)
        {
            var paramStringValues = rawParameters.Split(",");
            var parameters = new object[] { paramStringValues[0], int.TryParse(paramStringValues[1], out var parsed) ? new Nullable<int>(parsed) : null };

            try
            {
                Assert.NotNull(_docStringContent);

                _mockHttp.ReplyWithRequestUrl();

                var apiClient = _testHelper.CreateApiClient(_mockHttp.ToHttpClient());

                var methodInfo = apiClient.GetType().GetMethod(methodName);

                dynamic awaitable = methodInfo.Invoke(apiClient, parameters);
                await awaitable;
                _actual = awaitable.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        [Then(@"the requested URL should be (.+)")]
        public void CheckRequest(string url)
        {
            Assert.Equal(url, _actual);
        }
    }
}