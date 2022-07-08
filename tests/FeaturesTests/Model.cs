using Gherkin.Ast;
using Xunit;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace Features
{
    [FeatureFile(nameof(Model) + Constants.FeatureFileExtension)]
    public sealed class Model : FeatureBase
    {
        public Model(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [When(@"generating an API from the following specification")]
        new public void Generate(DocString schema)
        {
            Assert.False(string.IsNullOrWhiteSpace(schema.Content), $"Parameter '{nameof(schema)}' must not be null or whitespace");
            _docStringContent = schema.Content;
            _testHelper.GenerateApi(schema.Content);
        }

        [Then(@"it should generate a model object named (\w+)")]
        [And(@"it should generate a model object named (\w+)")]
        public void CheckModelExists(string typeName)
        {
            Assert.NotNull(_testHelper.TryGetType(typeName));
        }

        [And(@"(\w+) should have (an optional|a required) property named (\w+) of type (\w+)")]
        public void CheckPropertyTypes(string parentTypeName, string requiredOrOptional, string propertyName, string propertyTypeName)
        {
            var isRequired = requiredOrOptional == "a required";

            var parentType = _testHelper.TryGetType(parentTypeName);
            var propertyType = _testHelper.TryGetType(propertyTypeName);

            var propInfo = parentType.GetProperty(propertyName);
            Assert.NotNull(propInfo);

            if (propertyType != null)
            {
                Assert.Equal(propertyType, propInfo.PropertyType);
            }
            else
            {
                switch (propertyTypeName)
                {
                    case "number":
                        Assert.Contains<Type>(propInfo.PropertyType,
                            isRequired
                            ? new Type[] { typeof(int), typeof(long), typeof(float), typeof(double) }
                            : new Type[] { typeof(int?), typeof(long?), typeof(float?), typeof(double?) });
                        break;
                    case "string":
                        Assert.Equal(typeof(string), propInfo.PropertyType);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{nameof(CheckPropertyTypes)} does not handle {nameof(propertyTypeName)} = '{propertyTypeName}'");
                }
            }
        }
    }
}
