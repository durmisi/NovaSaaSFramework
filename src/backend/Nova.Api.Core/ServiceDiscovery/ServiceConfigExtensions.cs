using Microsoft.Extensions.Configuration;
using System;
using Nova.Api.Core.ServiceDiscovery;

namespace Nova.Api.Core.ServiceDiscovery
{
    public static class ServiceConfigExtensions
    {
        public static ServiceConfig GetServiceConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(configuration.GetValue<string>("ServiceConfig:Name")))
            {
                return null;
            }

            var serviceConfig = new ServiceConfig
            {
                Id = configuration.GetValue<string>("ServiceConfig:Id"),
                Name = configuration.GetValue<string>("ServiceConfig:Name"),
                Address = configuration.GetValue<string>("ServiceConfig:Address"),
                Port = configuration.GetValue<int>("ServiceConfig:Port"),
                DiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:DiscoveryAddress"),
                HealthCheckEndPoint = configuration.GetValue<string>("ServiceConfig:HealthCheckEndPoint"),
            };

            return serviceConfig;
        }
    }
}
