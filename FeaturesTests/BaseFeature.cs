using Xunit.Abstractions;
using RichardSzalay.MockHttp;

namespace Features
{
    public class BaseFeature : Xunit.Gherkin.Quick.Feature
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        protected readonly TestHelper _testHelper;

        protected readonly MockHttpMessageHandler _mockHttp;

        protected object _actual;

        protected string _docStringContent;

        public BaseFeature(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testHelper = new TestHelper(System.Guid.NewGuid().ToString().Substring(0, 8));
            _mockHttp = new MockHttpMessageHandler();
        }
    }
}