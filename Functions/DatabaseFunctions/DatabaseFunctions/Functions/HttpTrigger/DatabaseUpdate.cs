using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace DatabaseFunctions.Functions.HttpTrigger
{
    public class DatabaseUpdate
    {
        private readonly ILogger<DatabaseUpdate> _logger;

        public DatabaseUpdate(ILogger<DatabaseUpdate> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseUpdate")]
        public HttpResponseData UpdateData([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return UpdateData(_logger, req);
        }

        HttpResponseData UpdateData(ILogger logger, HttpRequest req)
        {


            throw new NotImplementedException();
        }
    }
}
