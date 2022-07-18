using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OpenApiForge
{
    public static class Startup
    {
        public static IServiceCollection RegisterApiClient(this IServiceCollection services,
            Configuration configuration)
        {
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (!string.IsNullOrWhiteSpace(configuration.BearerToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration.BearerToken}");
                }
            });

            return services;
        }
    }
}