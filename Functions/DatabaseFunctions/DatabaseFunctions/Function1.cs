using DatabaseFunctions.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly DatabaseControl _databaseControl;
        private readonly SqlConnectionStringBuilder builder;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
            _databaseControl = new DatabaseControl(logger);

            builder = new SqlConnectionStringBuilder();

            builder.DataSource = "databasefunctionsdbserver.database.windows.net";
            builder.UserID = "";
            builder.Password = "";
            builder.InitialCatalog = "DatabaseFunctions_db";
        }

        [Function("DatabaseInteraction")]
        public async Task<HttpResponseData> DataInteraction([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext context)            
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            if (req.Method == "GET")
            {
                _logger.LogInformation("Triggered GET method.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                List<string> expectedTypes = new List<string>();
                
                bool success = true; 
                string failMessage = "Unknown - most likely can't connect to database";

                try
                {
                    expectedTypes = JsonConvert.DeserializeObject<List<string>>(requestBody) ?? new List<string>();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error deserializing request body: {requestBody}");
                    failMessage = ex.Message;
                    success = false;

                }

                if (success)
                {
                    try
                    {
                        success = await _databaseControl.GetFromDatabase(builder, expectedTypes);
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        failMessage = ex.Message;
                    }
                }

                if (!success)
                {
                    _logger.LogError(failMessage, "Error Retrieving From Database.");
                    response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteStringAsync($"Reason for fail: {failMessage}");
                }
            }
            else if (req.Method == "POST")
            {
                _logger.LogInformation("Triggered POST method.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                

                bool success = true;
                string failMessage = "Unknown - most likely can't connect to database";
                Dictionary<string, List<Dictionary<string, string>>> contentToSave = new Dictionary<string, List<Dictionary<string, string>>>();

                try
                {
                    contentToSave = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(requestBody) ?? contentToSave;
                }
                catch (Exception ex)
                {
                    success = false;
                    _logger.LogError($"Error deserializing request body: {requestBody}");
                    failMessage = ex.Message;
                }

                if (success)
                {
                    try
                    {
                        success = await _databaseControl.SaveToDatabase(builder, contentToSave);
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        failMessage = ex.Message;
                    }
                }

                if (!success)
                {
                    _logger.LogError(failMessage, "Error Saving To Database.");
                    response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteStringAsync($"Reason for fail: {failMessage}");
                }
            }

            return response;          
        }

        [Function("DatabaseCreateTable")]
        public async Task<HttpResponseData> CreateTable([HttpTrigger(AuthorizationLevel.Admin, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            DatabaseControlAdv databaseAdvControl = new();
            var response = req.CreateResponse(HttpStatusCode.OK);

            bool success = true;
            string failMessage = "Unknown - most likely can't connect to database";

            string body = await new StreamReader(req.Body).ReadToEndAsync();
            Dictionary<string, List<string>> tableAndFields = new Dictionary<string, List<string>>();         

            try
            {
                tableAndFields = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(body) ?? tableAndFields;
            }
            catch (Exception ex)
            {
                _logger.LogError($"couldn't deserialize json body: {body}");
                success = false;
                failMessage = ex.Message;
            }

            if (success)
            {
                try
                {
                    success = await databaseAdvControl.CreateTables(_logger, builder, tableAndFields);
                }
                catch (Exception ex)
                {
                    success = false;
                    failMessage = ex.Message;
                }
            }

            if (!success)
            {
                _logger.LogError(failMessage, "Error deserializing JSON or creating tables.");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"Reason for fail: {failMessage}");
            }

            return response;
        }
    }
}
