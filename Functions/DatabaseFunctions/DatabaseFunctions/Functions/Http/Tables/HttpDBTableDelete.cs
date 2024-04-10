using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DatabaseFunctions.Functions.Http.Tables
{
    public class HttpDBTableDelete
    {
        private readonly ILogger<HttpDBTableDelete> _logger;

        public HttpDBTableDelete(ILogger<HttpDBTableDelete> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBTableDelete")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
