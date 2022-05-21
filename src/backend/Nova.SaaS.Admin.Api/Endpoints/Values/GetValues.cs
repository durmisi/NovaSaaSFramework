using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nova.SaaS.Admin.Api.Data.Models;
using Nova.SaaS.Admin.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nova.SaaS.Admin.Api.Endpoints.Values
{
    public class GetValues : EndpointWithoutRequest<GetValues.GetValuesResponse>
    {
        private readonly ILogger<GetValues> _logger;
        private readonly string _baseUrl;
        private readonly HttpContext _context;

        private readonly IDataContextService _dataContextService;

        public GetValues(ILogger<GetValues> logger, IHttpContextAccessor context, IDataContextService dataContextService)
        {
            if (context.HttpContext != null)
            {
                _context = context.HttpContext;
                var request = _context.Request;
                _baseUrl = $"{request.Scheme}://{request.Host}";
            }
            _logger = logger;
            _dataContextService = dataContextService;
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

            var requestInfo = $"Url: {_baseUrl}, Method: {_context.Request.Method}, Path: {_context.Request.Path}, OcelotRequestId: {ocelotReqId}";
            _logger.LogInformation(requestInfo);

            using (var context = _dataContextService.CreateContext())
            {
                await context.Values.AddAsync(new Value()
                {
                    Id = System.Guid.NewGuid(),
                    Name = requestInfo,
                    Created = System.DateTime.UtcNow
                });

                await context.SaveChangesAsync(cancellationToken: ct);
            }

            IEnumerable<Value> values;
            using (var context = _dataContextService.CreateContext())
            {
                values = await context.Values
                    .OrderByDescending(x => x.Created)
                    .ToListAsync(cancellationToken: ct);
            }

            var result = new GetValuesResponse
            {
                RequestInfo = requestInfo,
                Values = values
            };

            await SendAsync(result, 200, ct);
        }

        public class GetValuesResponse
        {
            public string RequestInfo { get; set; }
            public IEnumerable<Value> Values { get; set; }
        }

    }
}
