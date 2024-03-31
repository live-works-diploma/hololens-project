using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DatabaseFunctions.Functions.HttpTrigger.DataAccess
{
    public class DatabaseDeleteItem
    {
        private readonly ILogger<DatabaseDeleteItem> _logger;

        public DatabaseDeleteItem(ILogger<DatabaseDeleteItem> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseDeleteItem")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
