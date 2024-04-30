using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions.Functions.Http.Tables
{
    public class HttpDBTableUpdateAdd
    {
        private readonly ILogger<HttpDBTableUpdateAdd> _logger;

        public HttpDBTableUpdateAdd(ILogger<HttpDBTableUpdateAdd> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBTableUpdateAdd")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Admin, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return await Add(_logger, req);
        }

        async Task<HttpResponseData> Add(ILogger logger, HttpRequestData req)
        {
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var tablesToAdd = JsonConvert.DeserializeObject(reqBody);

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (JsonException ex)
            {
                logger.LogError($"Error deserializing JSON: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Delete Table Column: {ex}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
