// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------


using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using NovaSaasFramework.Provision.Models.Storage;

namespace NovaSaasFramework.Provision.Brokers
{
    public interface ICloudBroker
    {
        ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName);
        ValueTask<IAppServicePlan> CreatePlanAsync(string planName, IResourceGroup resourceGroup);
        ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName);
        ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(string sqlDatabaseName, ISqlServer sqlServer);
        ValueTask<ISqlServer> CreateSqlServerAsync(string sqlServerName, IResourceGroup resourceGroup);
        ValueTask<IWebApp> CreateWebAppAsync(string webAppName, string databaseConnectionString, IAppServicePlan plan, IResourceGroup resourceGroup);
        ValueTask DeleteResourceGroupAsync(string resourceGroupName);
        SqlDatabaseAccess GetAdminAccess();
    }
}