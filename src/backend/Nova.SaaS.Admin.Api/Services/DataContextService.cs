using Microsoft.Extensions.Configuration;
using Nova.SaaS.Admin.Api.Data;

namespace Nova.SaaS.Admin.Api.Services
{
    public interface IDataContextService
    {
        NovaSaasAdminDbContext CreateContext();
    }

    public class DataContextService : IDataContextService
    {
        private readonly IConfiguration _configuration;

        public DataContextService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public NovaSaasAdminDbContext CreateContext()
        {
            return new NovaSaasAdminDbContextFactory(_configuration)
                .Create();
        }

    }
}
