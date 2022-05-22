using System.Linq;
using System.Threading.Tasks;

using FastEndpoints;

using Xunit;
using Xunit.Abstractions;

namespace Nova.SaaS.Admin.Api.Tests
{

    public class ValuesEndpointsTests
    {
        private readonly ITestOutputHelper outputHelper;

        public ValuesEndpointsTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public async Task GetValuesEndpointReturnsOK()
        {
            //using (var context = Setup.CreateContext())
            //{
            //    context.Values.Add(new Data.Models.Value()
            //    {
            //        Id = System.Guid.NewGuid(),
            //        Name = "Test Value",
            //        Created = System.DateTime.Now
            //    });

            //    context.SaveChanges();
            //}

            var (_, res) = await Setup.GetClient(outputHelper).GETAsync<
                Endpoints.Values.GetValues,
                Endpoints.Values.GetValues.GetValuesResponse>();

            Assert.NotNull(res);
            Assert.NotEmpty(res?.RequestInfo);
            Assert.NotEmpty(res?.Values);
            Assert.Equal(1, res?.Values.Count());
        }
    }
}