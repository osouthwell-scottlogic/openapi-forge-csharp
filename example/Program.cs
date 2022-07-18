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

Console.WriteLine("Getting inventory..");

var inventory = await api.getInventory();

Console.WriteLine("Inventory:");
Console.WriteLine(JsonSerializer.Serialize(inventory));
