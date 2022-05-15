using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Nova.SaaS.Admin.Api.Endpoints.Env
{
    public class Status : EndpointWithoutRequest
    {
        private readonly ILogger<Healthcheck> _logger;
        private readonly HttpContext _context;

        public Status(ILogger<Healthcheck> logger, IHttpContextAccessor context)
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
            Routes("/status");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var msg = $"Running on {_context.Request.Host}";
            _logger.LogInformation(msg);

            await SendAsync(msg, 200, ct);
        }

    }
}
