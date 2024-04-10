using DatabaseFunctions.Models.Database;
using DatabaseFunctions.Models.Database.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions.Functions.Http.Tables
{
    public class HttpDBTableUpdate
    {
        private readonly ILogger<HttpDBTableUpdateAdd> _logger;

        public HttpDBTableUpdate(ILogger<HttpDBTableUpdateAdd> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBTableUpdate")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Admin, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string operation = req.Query["Operation"];

            if (operation == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            Action<ILogger, Dictionary<string, List<string>>> howToInteract;

            if (operation == "Add")
            {
                howToInteract = async (logger, data) =>
                {
                    await ModelDBTableUpdateRemove.RemoveAll(data);
                };
            }

            if (operation == "Remove")
            {
                howToInteract = async (logger, data) =>
                {
                    
                };
            }

            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return await Interact(_logger, req, howToInteract);
        }

        async Task<HttpResponseData> Interact<T>(ILogger logger, HttpRequestData req, Action<ILogger, T> howToInteract)
        {
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var tablesToRemove = JsonConvert.DeserializeObject<T>(reqBody);

                howToInteract(logger, tablesToRemove);

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
