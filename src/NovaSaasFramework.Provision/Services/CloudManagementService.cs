﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using NovaSaasFramework.Provision.Brokers;
using NovaSaasFramework.Provision.Models.Storage;

namespace NovaSaasFramework.Provision.Services
{
    public interface ICloudManagementService
    {
        ValueTask DeprovisionResouceGroupAsync(string projectName, string environment);
        ValueTask<IAppServicePlan> ProvisionPlanAsync(string projectName, string environment, IResourceGroup resourceGroup);
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(string projectName, string environment);
        ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(string projectname, string environment, ISqlServer sqlServer);
        ValueTask<ISqlServer> ProvisionSqlServerAsync(string projectName, string environment, IResourceGroup resourceGroup);
        ValueTask<IWebApp> ProvisionWebAppAsync(string projectName, string environment, string databaseConnectionString, IResourceGroup resourceGroup, IAppServicePlan appServicePlan);
    }

    public class CloudManagementService : ICloudManagementService
    {
        private readonly ICloudBroker cloudBroker;
        private readonly ILoggingBroker loggingBroker;

        public CloudManagementService()
        {
            this.cloudBroker = new CloudBroker();
            this.loggingBroker = new LoggingBroker();
        }

        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCES-{environment}".ToUpper();
            this.loggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

            IResourceGroup resourceGroup =
                await this.cloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            this.loggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned.");

            return resourceGroup;
        }

        public async ValueTask<IAppServicePlan> ProvisionPlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string planName = $"{projectName}-PLAN-{environment}".ToUpper();
            this.loggingBroker.LogActivity(message: $"Provisioning {planName}...");

            IAppServicePlan plan =
                await this.cloudBroker.CreatePlanAsync(planName, resourceGroup);

            this.loggingBroker.LogActivity(message: $"{plan} Provisioned");

            return plan;
        }

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

            ISqlServer sqlServer =
                await this.cloudBroker.CreateSqlServerAsync(
                    sqlServerName,
                    resourceGroup);

            this.loggingBroker.LogActivity(message: $"{sqlServer} Provisioned");

            return sqlServer;
        }

        public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectname,
            string environment,
            ISqlServer sqlServer)
        {
            string sqlDatabaseName = $"{projectname}-db-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {sqlDatabaseName}...");

            ISqlDatabase sqlDatabase =
                await this.cloudBroker.CreateSqlDatabaseAsync(
                    sqlDatabaseName,
                    sqlServer);

            this.loggingBroker.LogActivity(message: $"{sqlDatabaseName} Provisioned");

            return new SqlDatabase
            {
                Database = sqlDatabase,
                ConnectionString = GenerateConnectionString(sqlDatabase)
            };
        }

        public async ValueTask<IWebApp> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            IResourceGroup resourceGroup,
            IAppServicePlan appServicePlan)
        {
            string webAppName = $"{projectName}-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {webAppName}");

            IWebApp webApp =
                await this.cloudBroker.CreateWebAppAsync(
                    webAppName,
                    databaseConnectionString,
                    appServicePlan,
                    resourceGroup);

            this.loggingBroker.LogActivity(message: $"{webAppName} Provisioned");

            return webApp;
        }

        public async ValueTask DeprovisionResouceGroupAsync(string projectName, string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCES-{environment}".ToUpper();

            bool isResourceGroupExist =
                await this.cloudBroker.CheckResourceGroupExistAsync(
                    resourceGroupName);

            if (isResourceGroupExist)
            {
                this.loggingBroker.LogActivity(message: $"Deprovisioning {resourceGroupName}...");
                await this.cloudBroker.DeleteResourceGroupAsync(resourceGroupName);
                this.loggingBroker.LogActivity(message: $"{resourceGroupName} Deprovisioned");
            }
            else
            {
                this.loggingBroker.LogActivity(
                    message: $"Resource group {resourceGroupName} doesn't exist. No action taken.");
            }
        }

        private string GenerateConnectionString(ISqlDatabase sqlDatabase)
        {
            SqlDatabaseAccess sqlDatabaseAccess =
                this.cloudBroker.GetAdminAccess();

            return $"Server=tcp:{sqlDatabase.SqlServerName}.database.windows.net,1433;" +
                $"Initial Catalog={sqlDatabase.Name};" +
                $"User ID={sqlDatabaseAccess.AdminName};" +
                $"Password={sqlDatabaseAccess.AdminAccess};";
        }
    }
}
