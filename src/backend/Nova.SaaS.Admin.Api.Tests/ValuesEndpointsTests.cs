using System.Linq;
using System.Threading.Tasks;

using FastEndpoints;
using Nova.SaaS.Admin.Api.Data;
using Xunit;
using Xunit.Abstractions;

namespace Nova.SaaS.Admin.Api.Tests
{

    public class ValuesEndpointsTests : TestBase
    {
        public ValuesEndpointsTests(ITestOutputHelper outputHelper)
            :base(outputHelper)
        {
            
        }


        public override void Seed(NovaSaasAdminDbContext context)
        {
            //context.Values.Add(new Data.Models.Value()
            //{
            //    Id = System.Guid.NewGuid(),
            //    Name = "Test Value",
            //    Created = System.DateTime.Now
            //});

            //context.SaveChanges();
        }

        [Fact]
        public async Task GetValuesEndpointReturnsOK()
        {
            var (_, res) = await base.GetClient().GETAsync<
                Endpoints.Values.GetValues,
                Endpoints.Values.GetValues.GetValuesResponse>();

            Assert.NotNull(res);
            Assert.NotEmpty(res?.RequestInfo);
            Assert.NotEmpty(res?.Values);
            Assert.Equal(1, res?.Values.Count());
        }
    }
}