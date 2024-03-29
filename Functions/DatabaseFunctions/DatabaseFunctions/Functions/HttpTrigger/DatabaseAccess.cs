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

namespace DatabaseFunctions.Functions.HttpTrigger
{
    public class DatabaseAccess
    {
        readonly ILogger<DatabaseAccess> _logger;

        public DatabaseAccess(ILogger<DatabaseAccess> logger)
        {
            _logger = logger;
        }

        [Function("DatabaseAccess")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method == "POST")
            {
                return await SendToDatabase(_logger, req);
            }
            if (req.Method == "GET")
            {
                return await RetrieveFromDatabase(_logger, req);
            }

            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        async Task<HttpResponseData> RetrieveFromDatabase(ILogger logger, HttpRequestData req)
        {
            try
            {
                string requestBody = req.Query["TableNames"] ?? "";
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

                var data = DatabaseRetrieve.DatabaseGet(logger, AzureAccountInfo.builder, tableNames);

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

                DatabaseSend.DatabaseSave(logger, AzureAccountInfo.builder, contentToSave);

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
