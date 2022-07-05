using System.IO;
using System.Diagnostics;
using System.Reflection;
using NSubstitute;
using Xunit;
using Microsoft.CodeAnalysis;

namespace Features
{
    public class TestHelper : IDisposable
    {
        private readonly string _outputPath;
        private readonly string _schemaFilePath;

        private readonly string _uniqueId;

        private Type _configurationType;
        private Type _apiClientType;

        public TestHelper(string uniqueId)
        {
            _uniqueId = uniqueId;
            _outputPath = $".\\generated-api\\{uniqueId}";
            _schemaFilePath = $"{_outputPath}\\schema.json";
        }

        public void GenerateApi(string schema)
        {
            WriteToJsonFile(schema);
            ForgeApi();
            GetApiClientTypes();
        }

        private void WriteToJsonFile(string fileContent)
        {
            if (Directory.Exists(Path.GetDirectoryName(_schemaFilePath)))
            {
                Directory.Delete(Path.GetDirectoryName(_schemaFilePath), true);
            };

            Directory.CreateDirectory(Path.GetDirectoryName(_schemaFilePath));
            using var sw = new StreamWriter(_schemaFilePath, false, new System.Text.UTF8Encoding());
            sw.Write(fileContent);
        }

        private void ForgeApi()
        {
            const string templateProjectPath = "..\\..\\..\\..\\";

            try
            {
                using var cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.RedirectStandardError = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                var commandText = $"openapi-forge forge {_schemaFilePath} {templateProjectPath} -o {_outputPath} -s";
                cmd.StartInfo.Arguments = $"/C {commandText}";
                cmd.Start();
                cmd.WaitForExit(1000);

                using var errorReader = cmd.StandardError;
                var errorOutput = errorReader.ReadToEnd();

                Assert.Equal(string.Empty, errorOutput);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        private void GetApiClientTypes()
        {
            CreateProjectFile();
            CompileCode();
            StoreApiClientType();
        }

        private void StoreApiClientType()
        {
            Assembly generatedAssembly;
            generatedAssembly = Assembly.LoadFrom(Path.GetFullPath($"{_outputPath}\\bin\\Api{_uniqueId}.dll"));
            _configurationType = generatedAssembly.GetType("OpenApiForge.Configuration");
            _apiClientType = generatedAssembly.GetType("OpenApiForge.ApiClient");
        }

        private void CreateProjectFile()
        {
            using var sw = new StreamWriter($"{_outputPath}\\Api{_uniqueId}.csproj", false, new System.Text.UTF8Encoding());
            sw.Write(@"
            <Project Sdk=""Microsoft.NET.Sdk"">
                <PropertyGroup>
                    <TargetFramework>net6.0</TargetFramework>
                    <Nullable>disable</Nullable>
                    <IsPackable>true</IsPackable>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include=""Microsoft.Extensions.Configuration"" Version=""6.0.1"" />
                    <PackageReference Include=""Microsoft.Extensions.DependencyInjection"" Version=""6.0.0"" />
                    <PackageReference Include=""Microsoft.Extensions.Http"" Version=""6.0.0"" />
                </ItemGroup>
            </Project>");
        }

        private void CompileCode()
        {
            try
            {
                using var cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.RedirectStandardError = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                var commandText = $"dotnet build {_outputPath}/Api{_uniqueId}.csproj -o {_outputPath}/bin";
                cmd.StartInfo.Arguments = $"/C {commandText}";
                cmd.Start();
                cmd.WaitForExit(1000);

                using var errorReader = cmd.StandardError;
                var errorOutput = errorReader.ReadToEnd();

                Assert.Equal(string.Empty, errorOutput);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        public object CreateApiClient(HttpClient client, int? serverIndex = null)
        {
            var configuration = Activator.CreateInstance(_configurationType);
            var serversPropInfo = configuration.GetType().GetProperty("Servers");
            var servers = (string[])serversPropInfo.GetValue(configuration);

            client.BaseAddress = servers.Length == 0
            ? new Uri(Constants.BaseAddress)
            : serverIndex.HasValue ? new Uri(servers[serverIndex.Value])
                : new Uri(servers[int.TryParse(configuration.GetType().GetProperty("SelectedServerIndex").GetValue(configuration)?.ToString(), out var index) ? index : 0]);

            return Activator.CreateInstance(_apiClientType, new object[] { client, configuration });
        }

        public void Dispose()
        {
            _apiClientType = null;
            _configurationType = null;
        }
    }
}