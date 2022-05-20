using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

var githubPipeline = new GithubPipeline
{
    Name = "Nova.SaaS.Admin.Api Build Pipeline",

    OnEvents = new Events
    {
        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "master" }
        },

        Push = new PushEvent
        {
            Branches = new string[] { "master" }
        }
    },

    Jobs = new Jobs
    {
        Build = new BuildJob
        {
            RunsOn = BuildMachines.Windows2022,


            Steps = new List<GithubTask>
            {
                new CheckoutTaskV2
                {
                    Name = "Checking Out Code"
                },

                new SetupDotNetTaskV1
                {
                    Name = "Installing .NET",

                    TargetDotNetVersion = new TargetDotNetVersion
                    {
                        DotNetVersion = "6.0.300",
                        IncludePrerelease = true
                    }
                },

                new RestoreTask
                {
                    Name = "Restoring Nuget Packages",
                    Run = "dotnet restore NovaSaaSFramework.sln"
                },

                new DotNetBuildTask
                {
                    Name = "Building Project",
                    Run = "dotnet build NovaSaaSFramework.sln -c Release"
                },

                //new TestTask
                //{
                //    Name = "Running Tests"
                //}
            },
        }
    }
};

var client = new ADotNetClient();

client.SerializeAndWriteToFile(
    adoPipeline: githubPipeline,
    path: "../../../../../.github/workflows/dotnet.yml");