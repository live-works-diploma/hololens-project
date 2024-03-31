using DatabaseFunctions.Models.Database;
using DatabaseFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions.Functions.HttpTrigger.DataAccess
{
    public class DatabaseUpdateItem
    {
        private readonly ILogger<DatabaseUpdateItem> _logger;

        public DatabaseUpdateItem(ILogger<DatabaseUpdateItem> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseUpdateItem")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return await SendToDatabase(_logger, req);
        }

        async Task<HttpResponseData> SendToDatabase(ILogger logger, HttpRequestData req)
        {
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var contentToSave = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(reqBody);

                if (contentToSave == null)
                {
                    logger.LogError("Couldn't find request body");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                string tableName = req.Query["TableName"] ?? "";

                if (tableName == "")
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                string conditions = req.Query["ConditionColumn"] ?? "id";

                DatabaseSend.UpdateAllRecords(logger, AzureAccountInfo.builder, tableName, contentToSave, conditions);

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (JsonException ex)
            {
                logger.LogError($"Error deserializing JSON: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error sending data to database: {ex}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
