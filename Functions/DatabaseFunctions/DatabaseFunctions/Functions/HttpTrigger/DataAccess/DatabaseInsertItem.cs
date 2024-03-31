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
    public class DatabaseInsertItem
    {
        private readonly ILogger<DatabaseInsertItem> _logger;

        public DatabaseInsertItem(ILogger<DatabaseInsertItem> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseInsertItem")]
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

                var contentToSave = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(reqBody);

                if (contentToSave == null)
                {
                    logger.LogError("Couldn't find request body");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                DatabaseSend.DatabaseSaveAll(logger, AzureAccountInfo.builder, contentToSave);

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
