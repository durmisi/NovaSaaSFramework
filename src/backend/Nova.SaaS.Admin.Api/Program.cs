global using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nova.Api.Core.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
}).ConfigureLogging((builderContext, logging) =>
{
    logging.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
});

var configuration = builder.Configuration;

builder.Services.AddFastEndpoints();

builder.Services.AddConsul(configuration.GetServiceConfig());
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.RoutingOptions = o => o.Prefix = "api";
});

app.Run();

//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System.IO;

//namespace Nova.SaaS.Admin.Api
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .UseContentRoot(Directory.GetCurrentDirectory())
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                })
//                .ConfigureAppConfiguration((hostingContext, config) =>
//                {
//                    config
//                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
//                        .AddJsonFile("appsettings.json", false, true)
//                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true)
//                        .AddEnvironmentVariables();
//                })
//                .ConfigureLogging((builderContext, logging) =>
//                {
//                    logging.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
//                    logging.AddConsole();
//                    logging.AddDebug();
//                    logging.AddEventSourceLogger();
//                });
//    }
//}
