using DatabaseFunctions.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DatabaseFunctions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly DatabaseControl _databaseControl;
        private readonly string _connectionString;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
            _databaseControl = new DatabaseControl();
            _connectionString = "Server=tcp:iot-test-server.database.windows.net,1433;Initial Catalog=iot-sensor-data;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
        }

        [Function("DatabaseInteraction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext context)            
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                if (req.Method == "GET")
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    List<string> expectedTypes = JsonConvert.DeserializeObject<List<string>>(requestBody) ?? new List<string>();

                    await _databaseControl.GetFromDatabase(response, _connectionString, expectedTypes);
                }
                else if (req.Method == "POST")
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Dictionary<string, List<Dictionary<string, string>>> contentToSave = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(requestBody)
                        ?? new Dictionary<string, List<Dictionary<string, string>>>();

                    await _databaseControl.SaveToDatabase(_connectionString, contentToSave);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing request.");
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

        [Function("DatabaseCreateTable")]
        public async Task<HttpResponseData> CreateTable([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            DatabaseControlAdv databaseAdvControl = new();
            var response = req.CreateResponse(HttpStatusCode.OK);
            string connectionString = "Server=tcp:northmetro-tafe-server.database.windows.net,1433;Initial Catalog=northmetro-tafe-iotsensors;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";

            string json = await new StreamReader(req.Body).ReadToEndAsync();
            Dictionary<string, List<string>> tableAndFields = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json) ?? new Dictionary<string, List<string>>();

            await databaseAdvControl.CreateTables(connectionString, tableAndFields);

            return response;
        }
    }
}
