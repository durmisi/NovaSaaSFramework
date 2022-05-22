using System;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using Nova.SaaS.Admin.Api.Data;
using Nova.SaaS.Admin.Api.Services;

using MartinCostello.Logging.XUnit;

using Xunit.Abstractions;

namespace Nova.SaaS.Admin.Api.Tests
{
    public class TestBase : IDisposable
    {
        private static readonly WebApplicationFactory<Program> factory = new();
        private readonly ITestOutputHelper outputHelper;
        private bool disposedValue;

        private string databaseName;

        public TestBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.databaseName = $"{Guid.NewGuid():N}.db";
        }

        public HttpClient GetClient()
        {
            return factory.WithWebHostBuilder(b =>
            {
                b.ConfigureServices(services =>
                        {
                            services.RemoveAll<ILoggerProvider>();
                            services.RemoveAll<ILoggerFactory>();
                            services.RemoveAll<ILogger>();
                            services.AddLogging(builder =>
                            {
                                builder.ClearProviders();
                                builder.AddProvider(new XUnitLoggerProvider(outputHelper, new XUnitLoggerOptions()
                                {
                                    IncludeScopes = true,
                                }));
                            });

                            services.RemoveAll<IDataContextService>();
                            services.TryAddTransient<IDataContextService>(x =>
                            {
                                var xUnitLoggerFactory = new LoggerFactory();
                                xUnitLoggerFactory.AddProvider(new XUnitLoggerProvider(outputHelper, new XUnitLoggerOptions()
                                {
                                    IncludeScopes = true,
                                }));

                                var dataContextService = new TestDataContextService(xUnitLoggerFactory, databaseName);
                                using (var context = dataContextService.CreateContext())
                                {
                                    context.Database.EnsureCreated();

                                    Seed(context);
                                }

                                return dataContextService;
                            });
                        });

            }).CreateClient();
        }

        public NovaSaasAdminDbContext CreateContext()
        {
            var service = factory.Services.GetRequiredService<IDataContextService>();
            return service.CreateContext();
        }

        public virtual void Seed(NovaSaasAdminDbContext context)
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    try
                    {
                        using (var context = CreateContext())
                        {
                            context.Database.EnsureDeleted();
                        }
                    }
                    catch (Exception ex)
                    {
                        outputHelper.WriteLine(ex.Message);
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~TestBase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


    public class TestDataContextService : IDataContextService
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly string databaseName;

        public TestDataContextService(ILoggerFactory loggerFactory, string databaseName)
        {
            this.loggerFactory = loggerFactory;
            this.databaseName = databaseName;
        }

        public NovaSaasAdminDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<NovaSaasAdminDbContext>()
                .UseSqlite($"Data Source={databaseName}", sql => sql.MigrationsAssembly(typeof(IDataContextService).Assembly.GetName().Name))
                //.UseInMemoryDatabase(databaseName)
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory)
                .Options;

            var context = new NovaSaasAdminDbContext(options);

            return context;
        }
    }

}