using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Nova.SaaS.Admin.Api.Endpoints.Values
{
    public class GetValues : EndpointWithoutRequest
    {
        private readonly ILogger<GetValues> _logger;
        private readonly string _baseUrl;
        private readonly HttpContext _context;

        public GetValues(ILogger<GetValues> logger, IHttpContextAccessor context)
        {
            if (context.HttpContext != null)
            {
                _context = context.HttpContext;
                var request = _context.Request;
                _baseUrl = $"{request.Scheme}://{request.Host}";
            }
            _logger = logger;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/values");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var ocelotReqId = _context.Request.Headers["OcRequestId"];
            if (string.IsNullOrEmpty(ocelotReqId))
            {
                ocelotReqId = "N/A";
            }
            var result = $"Url: {_baseUrl}, Method: {_context.Request.Method}, Path: {_context.Request.Path}, OcelotRequestId: {ocelotReqId}";
            _logger.LogInformation(result);

            await SendAsync(result, 200, ct);
        }

    }
}
