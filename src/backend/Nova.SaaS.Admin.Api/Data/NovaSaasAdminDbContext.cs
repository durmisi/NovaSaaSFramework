using Microsoft.EntityFrameworkCore;
using Nova.SaaS.Admin.Api.Data.Models;

namespace Nova.SaaS.Admin.Api.Data
{

    public class NovaSaasAdminDbContext : DbContext
    {

        public NovaSaasAdminDbContext(DbContextOptions<NovaSaasAdminDbContext> options)
            : base(options)
        {

        }

        public DbSet<Value> Values { get; set; }
    }
}