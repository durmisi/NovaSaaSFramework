using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nova.SaaS.Admin.Api.Data;
using Nova.SaaS.Admin.Api.Services;
using System;
using System.Net.Http;
using Xunit.Abstractions;

namespace Nova.SaaS.Admin.Api.Tests
{
    public static class Setup
    {
        private static readonly WebApplicationFactory<Program> factory = new();

        public static HttpClient GetClient(ITestOutputHelper outputHelper) => factory
            .WithWebHostBuilder(b =>
            {
                b.ConfigureServices(services =>
                        {
                            services.RemoveAll<ILoggerProvider>();
                            services.RemoveAll<ILoggerFactory>();
                            services.RemoveAll<ILogger>();
                            services.AddLogging(builder =>
                            {
                                builder.ClearProviders();

                                builder
                                .AddConsole()
                                .AddDebug()
                                .AddProvider(new XUnitLoggerProvider(outputHelper, new XUnitLoggerOptions()
                                {
                                    IncludeScopes = true,
                                }));
                            });

                            
                            services.RemoveAll<IDataContextService>();
                            services.TryAddTransient<IDataContextService, TestDataContextService>();
                        });

            }).CreateClient();


        public static NovaSaasAdminDbContext CreateContext()
        {
            var service = (IDataContextService)factory.Services.GetService(typeof(IDataContextService));
            return service.CreateContext();
        }

        static Setup()
        {

        }
    }


    public class TestDataContextService : IDataContextService
    {
        private readonly ILoggerFactory loggerFactory;

        public TestDataContextService(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }
        public NovaSaasAdminDbContext CreateContext()
        {
            var databaseName = "NovaSaasAdminDb";

            var options = new DbContextOptionsBuilder<NovaSaasAdminDbContext>()
                .UseInMemoryDatabase(databaseName)
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory)
                .Options;

            return new NovaSaasAdminDbContext(options);
        }
    }

}