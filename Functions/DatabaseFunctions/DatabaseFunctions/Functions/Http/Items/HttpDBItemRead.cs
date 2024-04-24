using Azure;
using DatabaseFunctions.Models.Items;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace DatabaseFunctions.Functions.HttpTrigger.DataAccess
{
    public class HttpDBItemRead
    {
        readonly ILogger<HttpDBItemRead> _logger;

        public HttpDBItemRead(ILogger<HttpDBItemRead> logger)
        {
            _logger = logger;
        }

        [Function("HttpDBItemRead")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return await RetrieveFromDatabase(_logger, req);
        }

        async Task<HttpResponseData> RetrieveFromDatabase(ILogger logger, HttpRequestData req)
        {
            try
            {
                string? requestBody = req.Query["TableNames"];

                if (requestBody == null)
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                logger.LogInformation($"Request body: {requestBody}");

                string[]? tableNames = JsonConvert.DeserializeObject<string[]>(requestBody);

                if (tableNames == null)
                {
                    logger.LogError("Deserialization failed to assign tableNames");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                logger.LogInformation($"Table names: {string.Join(", ", tableNames)}");

                string conditions = req.Query["Conditions"] ?? "";

                string? blobStorage = req.Query["BlobStorage"];                
                bool accessBlobStorage = blobStorage == null || blobStorage.ToLower() == "true";

                ModelDBItemRead model = new();

                var data = await model.GetData(logger, tableNames, conditions, accessBlobStorage);

                var response = req.CreateResponse(HttpStatusCode.OK);

                if (data != null)
                {
                    await response.WriteAsJsonAsync(data);
                }

                return response;
            }
            catch (JsonException ex)
            {
                string errorMessage = $"Error deserializing JSON: {ex.Message}";
                logger.LogError(errorMessage);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred: {ex.Message}";
                logger.LogError(errorMessage);
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
