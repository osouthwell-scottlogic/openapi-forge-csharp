using System;
using System.Reflection;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    public class BaseFeature : Xunit.Gherkin.Quick.Feature
    {
        protected readonly ITestOutputHelper _testOutputHelper;

        protected readonly TestHelper _testHelper;

        protected readonly MockHttpMessageHandler _mockHttp;

        protected object _actual;

        protected string _docStringContent;

        protected HttpRequestMessage _request;

        public BaseFeature(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testHelper = new TestHelper(System.Guid.NewGuid().ToString().Substring(0, 8));
            _mockHttp = new MockHttpMessageHandler();
        }

        [Then(@"the requested URL should be (.+)")]
        public void CheckRequestUri(string url)
        {
            var expected = new Uri(url);
            var actual = new Uri(_actual.ToString());
            Assert.True(expected.Equals(actual));
        }

        protected async Task CallMethod(string methodName, object[] parameters = null, string response = null, int? serverIndex = 0)
        {
            _mockHttp.When("*").Respond((HttpRequestMessage request) =>
            {
                InterceptRequest(request);

                return Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(response ?? request.RequestUri.AbsoluteUri.ToString()),
                        RequestMessage = request
                    });
            });

            var apiClient = _testHelper.CreateApiClient(_mockHttp.ToHttpClient(), serverIndex);

            var methodInfo = apiClient.GetType().GetMethod(methodName);

            var methodParameters = methodInfo.GetParameters();

            dynamic awaitable = methodInfo.Invoke(apiClient, GetMethodArgs(methodParameters, parameters));
            await awaitable;
            _actual = awaitable.GetAwaiter().GetResult();
        }

        private static object[] GetMethodArgs(ParameterInfo[] paramInfos, object[] arguments)
        {
            if (paramInfos.Length == 0)
            {
                return null;
            }

            if (arguments == null)
            {
                arguments = new object[paramInfos.Length];
            }
            else if (paramInfos.Length > arguments.Length)
            {
                Array.Resize(ref arguments, paramInfos.Length);
            }

            return paramInfos
                .Zip(arguments, (paramInfo, arg) => arg != null
                    ? TryParsePrimitiveArgs(arg, paramInfo.ParameterType)
                    : (paramInfo.HasDefaultValue ? paramInfo.DefaultValue : null))
                .ToArray();
        }

        private void InterceptRequest(HttpRequestMessage request)
        {
            _request = request;
        }

        private static object TryParsePrimitiveArgs(object arg, Type paramType)
        {
            switch (Type.GetTypeCode(paramType))
            {
                case TypeCode.Int32:
                    return int.Parse(arg.ToString());
                case TypeCode.Int64:
                    return long.Parse(arg.ToString());
                case TypeCode.Single:
                    return float.Parse(arg.ToString());
                case TypeCode.Double:
                    return double.Parse(arg.ToString());
                case TypeCode.Boolean:
                    return bool.Parse(arg.ToString());
                case TypeCode.DateTime:
                    return DateTime.Parse(arg.ToString());
                default:
                    return arg;
            };
        }
    }
}
