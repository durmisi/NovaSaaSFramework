// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using NovaSaasFramework.Provision.Brokers;
using NovaSaasFramework.Provision.Models.Configuration;
using NovaSaasFramework.Provision.Models.Storage;

namespace NovaSaasFramework.Provision.Services
{

    public interface ICloudManagementProcessingService
    {
        ValueTask ProcessAsync();
    }

    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly ICloudManagementService cloudManagementService;
        private readonly IConfigurationBroker configurationBroker;

        public CloudManagementProcessingService()
        {
            this.cloudManagementService = new CloudManagementService();
            this.configurationBroker = new ConfigurationBroker();
        }

        public async ValueTask ProcessAsync()
        {
            CloudManagementConfiguration cloudManagementConfiguration =
                this.configurationBroker.GetConfiguration();

            await ProvisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Up);

            await DeprovisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Down);
        }

        private async ValueTask ProvisionAsync(
            string projectName,
            CloudAction cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                IResourceGroup resourceGroup = await this.cloudManagementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environmentName);

                IAppServicePlan appServicePlan = await this.cloudManagementService
                    .ProvisionPlanAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                ISqlServer sqlServer = await this.cloudManagementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlDatabase sqlDatabase = await this.cloudManagementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environmentName,
                        sqlServer);

                IWebApp webApp = await this.cloudManagementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environmentName,
                        sqlDatabase.ConnectionString,
                        resourceGroup,
                        appServicePlan);
            }
        }

        private async ValueTask DeprovisionAsync(
            string projectName,
            CloudAction cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                await this.cloudManagementService.DeprovisionResouceGroupAsync(
                    projectName,
                    environmentName);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
