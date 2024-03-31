using Azure;
using DatabaseFunctions.Models;
using DatabaseFunctions.Models.Database;
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
    public class DatabaseGetItem
    {
        readonly ILogger<DatabaseGetItem> _logger;

        public DatabaseGetItem(ILogger<DatabaseGetItem> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseGetItem")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return await RetrieveFromDatabase(_logger, req);
        }

        async Task<HttpResponseData> RetrieveFromDatabase(ILogger logger, HttpRequestData req)
        {
            try
            {
                string requestBody = req.Query["TableNames"];

                if (requestBody == null)
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                logger.LogInformation($"Request body: {requestBody}");

                string[] tableNames = JsonConvert.DeserializeObject<string[]>(requestBody);

                if (tableNames == null)
                {
                    logger.LogError("Deserialization failed to assign tableNames");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                logger.LogInformation($"Table names: {string.Join(", ", tableNames)}");

                for (int i = 0; i < tableNames.Length; i++)
                {
                    tableNames[i] = $"[dbo].[{tableNames[i]}]";
                }

                string conditions = req.Query["Conditions"] ?? "";

                var data = DatabaseRetrieve.DatabaseGet(logger, AzureAccountInfo.builder, tableNames, conditions);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(data);
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
