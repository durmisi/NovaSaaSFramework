using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nova.SaaS.Admin.Api.Data;

namespace Nova.SaaS.Admin.Api.Services
{
    public class NovaSaasAdminDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public NovaSaasAdminDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NovaSaasAdminDbContext Create()
        {
            //var connectionString = _configuration.GetConnectionString("DefaultConnection");

            string dbPath = "novaadminapp.db";

            DbContextOptions<NovaSaasAdminDbContext> options = new DbContextOptionsBuilder<NovaSaasAdminDbContext>()
                //.UseSqlServer(connectionString, sql => sql.MigrationsAssembly("Nova.SaaS.Admin.Api"))
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new NovaSaasAdminDbContext(options);
        }
    }
}
