using NovaSaasFramework.Provision.Services;

ICloudManagementProcessingService cloudManagementProcessingService =
                new CloudManagementProcessingService();

await cloudManagementProcessingService.ProcessAsync();
