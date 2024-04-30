using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using DatabaseFunctions.Models.Items;
using DatabaseFunctions.Models.Information;

namespace DatabaseFunctions.Functions.HttpTrigger.DataAccess
{
    public class HttpDBItemUpdate
    {
        private readonly ILogger<HttpDBItemUpdate> _logger;

        public HttpDBItemUpdate(ILogger<HttpDBItemUpdate> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBItemUpdate")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return await Update(_logger, req);
        }

        async Task<HttpResponseData> Update(ILogger logger, HttpRequestData req)
        {
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var contentToUpdate = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(reqBody);

                if (contentToUpdate == null)
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

                ModelDBItemUpdate.UpdateAllRecords(logger, ModelDBAccountInfo.builder, tableName, contentToUpdate, conditions);

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
