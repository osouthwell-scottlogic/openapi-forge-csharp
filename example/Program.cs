// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using OpenApiForge;

var config = new Configuration
{
    BasePath = "/api/v3",
    Servers = new[] { "https://petstore3.swagger.io" }
};

var services = new ServiceCollection();

services.AddSingleton(config);
services.RegisterApiClient(config);

using var serviceProvider = services.BuildServiceProvider();

var api = serviceProvider.GetRequiredService<IApiClient>();

Console.WriteLine("Getting pets..");

// e.g. Get pets by status = available
var pets = await api.findPetsByStatus();

Console.WriteLine("Pets:");
Console.WriteLine(JsonSerializer.Serialize(pets, new JsonSerializerOptions { WriteIndented = true }));
