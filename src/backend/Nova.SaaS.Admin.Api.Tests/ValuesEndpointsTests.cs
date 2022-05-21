using System.Threading.Tasks;

using FastEndpoints;

using Xunit;

namespace Nova.SaaS.Admin.Api.Tests
{

    public class ValuesEndpointsTests
    {
        [Fact]
        public async Task GetValuesEndpointReturnsOK()
        {
            var (_, res) = await Setup.ApiClient.GETAsync<
                Endpoints.Values.GetValues,
                Endpoints.Values.GetValues.GetValuesResponse>();

            Assert.NotNull(res);
            Assert.NotEmpty(res?.RequestInfo);
            Assert.NotEmpty(res?.Values);
        }
    }
}