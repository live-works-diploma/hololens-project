using DatabaseFunctions.Models.Database.Items;
using DatabaseFunctions.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions.Functions.HttpTrigger.DataAccess
{
    public class HttpDBItemDelete
    {
        private readonly ILogger<HttpDBItemDelete> _logger;

        public HttpDBItemDelete(ILogger<HttpDBItemDelete> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBItemDelete")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return await Delete(_logger, req);
        }

        async Task<HttpResponseData> Delete(ILogger logger, HttpRequestData req)
        {
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var contentToDelete = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(reqBody);

                if (contentToDelete == null)
                {
                    logger.LogError("Couldn't find request body");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                ModelDBItemDelete.DatabaseDeleteAll(logger, ModelDBAccountInfo.builder, contentToDelete); 

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
