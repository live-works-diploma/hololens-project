using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DatabaseFunctions.Functions.Http.Tables
{
    public class HttpDBTableRead
    {
        private readonly ILogger<HttpDBTableRead> _logger;

        public HttpDBTableRead(ILogger<HttpDBTableRead> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBTableRead")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
