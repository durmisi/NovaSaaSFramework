using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nova.SaaS.Admin.Api.Data
{
    public class NovaSaasAdminDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NovaSaasAdminDbContext>
    {
        public NovaSaasAdminDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NovaSaasAdminDbContext>();
            optionsBuilder.UseSqlServer(
              "Server=(local);Database=NovaSaasAdmin;Trusted_Connection=True;");

            return new NovaSaasAdminDbContext(optionsBuilder.Options);
        }

    }
}