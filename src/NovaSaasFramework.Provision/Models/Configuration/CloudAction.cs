// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

namespace NovaSaasFramework.Provision.Models.Configuration
{
    public class CloudAction
    {
        public List<string> Environments { get; set; }
    }

    public class CloudManagementConfiguration
    {
        public string ProjectName { get; set; }
        public CloudAction Up { get; set; }
        public CloudAction Down { get; set; }
    }
}
