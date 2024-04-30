using DatabaseFunctions.Models.Information;
using DatabaseFunctions.Models.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DatabaseFunctions.Functions.HttpTrigger.Tables
{
    public class HttpDBTableCreate
    {
        private readonly ILogger<HttpDBTableCreate> _logger;

        public HttpDBTableCreate(ILogger<HttpDBTableCreate> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBTableCreate")]
        public async Task<HttpResponseData> CreateTable([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return await CreateTables(_logger, req);
        }

        async Task<HttpResponseData> CreateTables(ILogger logger, HttpRequestData req)
        {
            try
            {
                string body = await new StreamReader(req.Body).ReadToEndAsync();

                Dictionary<string, List<string>> tableAndFields = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(body);
                if (tableAndFields == null)
                {
                    logger.LogError("Didn't deserialize body properly but didn't cause an error");
                    return req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                }

                bool success = ModelDBTableCreate.CreateTables(logger, ModelDBAccountInfo.builder, tableAndFields);

                if (!success)
                {
                    return req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                }

                return req.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            catch (JsonException ex)
            {
                logger.LogError($"Error deserializing json string: {ex}");
                return req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error creating new table: {ex}");
                return req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
