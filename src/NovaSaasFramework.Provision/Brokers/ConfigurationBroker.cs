// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using NovaSaasFramework.Provision.Models.Configuration;

namespace NovaSaasFramework.Provision.Brokers
{
    public interface IConfigurationBroker
    {
        CloudManagementConfiguration GetConfiguration();
    }
    public class ConfigurationBroker : IConfigurationBroker
    {
        public CloudManagementConfiguration GetConfiguration()
        {
            DotEnv.Load(".env");

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(basePath: Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            return configurationRoot.Get<CloudManagementConfiguration>();
        }
    }
}


public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}