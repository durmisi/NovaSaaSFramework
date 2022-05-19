using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Nova.SaaS.Admin.Api.Tests
{
    public static class Setup
    {
        private static readonly WebApplicationFactory<Program> factory = new();

        public static HttpClient ApiClient { get; } = factory
            .WithWebHostBuilder(b =>
                b.ConfigureTestServices(s =>
                {

                })
            ).CreateClient();

       
        static Setup()
        {
            
        }
    }
}