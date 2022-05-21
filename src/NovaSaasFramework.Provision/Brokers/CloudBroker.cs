// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------


using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using NovaSaasFramework.Provision.Models.Storage;

namespace NovaSaasFramework.Provision.Brokers
{


    public class CloudBroker : ICloudBroker
    {

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tenantId;
        private readonly string adminName;
        private readonly string adminAccess;
        private readonly IAzure azure;

        public CloudBroker()
        {
            this.clientId = Environment.GetEnvironmentVariable("AzureClientId");
            this.clientSecret = Environment.GetEnvironmentVariable("AzureClientSecret");
            this.tenantId = Environment.GetEnvironmentVariable("AzureTenantId");
            this.adminName = Environment.GetEnvironmentVariable("AzureAdminName");
            this.adminAccess = Environment.GetEnvironmentVariable("AzureAdminAccess");
            this.azure = AutneticateAzure();
        }

        private IAzure AutneticateAzure()
        {
            AzureCredentials credentials =
                SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                    clientId: this.clientId,
                    clientSecret: this.clientSecret,
                    tenantId: this.tenantId,
                    environment: AzureEnvironment.AzureGlobalCloud);

            return Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }


        #region Resource Groups

        public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) =>
           await this.azure.ResourceGroups.ContainAsync(resourceGroupName);

        public async ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName)
        {
            return await this.azure.ResourceGroups
                .Define(name: resourceGroupName)
                .WithRegion(region: Region.EuropeWest)
                .CreateAsync();
        }

        public async ValueTask DeleteResourceGroupAsync(string resourceGroupName) =>
            await this.azure.ResourceGroups.DeleteByNameAsync(resourceGroupName);

        #endregion


        #region Service Plans 

        public async ValueTask<IAppServicePlan> CreatePlanAsync(
           string planName,
           IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.AppServicePlans
                .Define(planName)
                .WithRegion(Region.EuropeWest)
                .WithExistingResourceGroup(resourceGroup)
                .WithPricingTier(PricingTier.StandardS1)
                .WithOperatingSystem(Microsoft.Azure.Management.AppService.Fluent.OperatingSystem.Windows)
                .CreateAsync();
        }

        #endregion


        #region Storage

        public async ValueTask<ISqlServer> CreateSqlServerAsync(
            string sqlServerName,
            IResourceGroup resourceGroup)
        {
            return await this.azure.SqlServers
                .Define(sqlServerName)
                .WithRegion(Region.EuropeWest)
                .WithExistingResourceGroup(resourceGroup)
                .WithAdministratorLogin(this.adminName)
                .WithAdministratorPassword(this.adminAccess)
                .CreateAsync();
        }

        public async ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(
            string sqlDatabaseName,
            ISqlServer sqlServer)
        {
            return await this.azure.SqlServers.Databases
                .Define(sqlDatabaseName)
                .WithExistingSqlServer(sqlServer)
                .CreateAsync();
        }

        public SqlDatabaseAccess GetAdminAccess()
        {
            return new SqlDatabaseAccess
            {
                AdminName = this.adminName,
                AdminAccess = this.adminAccess
            };
        }

        #endregion


        #region WebApps

        public async ValueTask<IWebApp> CreateWebAppAsync(
           string webAppName,
           string databaseConnectionString,
           IAppServicePlan plan,
           IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.WebApps
                .Define(webAppName)
                .WithExistingWindowsPlan(plan)
                .WithExistingResourceGroup(resourceGroup)
                .WithNetFrameworkVersion(NetFrameworkVersion.Parse("v6.0"))
                .WithConnectionString(
                    name: "DefaultConnect",
                    value: databaseConnectionString,
                    type: ConnectionStringType.SQLAzure)
                .CreateAsync();
        }

        #endregion


    }


}
