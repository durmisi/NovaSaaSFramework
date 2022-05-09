using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nova.SaaS.Admin.App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public void OnGet()
        {
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var result = $"Url: {baseUrl}, Method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}";
            _logger.LogInformation(result);

            ViewData["result"] = result;
        }
    }
}