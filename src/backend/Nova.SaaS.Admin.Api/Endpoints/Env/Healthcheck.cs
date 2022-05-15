using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Nova.SaaS.Admin.Api.Endpoints.Env
{
    public class Healthcheck : EndpointWithoutRequest
    {
        private readonly ILogger<Healthcheck> _logger;
        private readonly HttpContext _context;

        public Healthcheck(ILogger<Healthcheck> logger, IHttpContextAccessor context)
        {
            if (context.HttpContext != null)
            {
                _context = context.HttpContext;
            }
            
            _logger = logger;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/healthcheck");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var msg = $"{_context.Request.Host} is healthy";
            _logger.LogInformation(msg);

            await SendAsync(msg, 200, ct);
        }

    }
}
