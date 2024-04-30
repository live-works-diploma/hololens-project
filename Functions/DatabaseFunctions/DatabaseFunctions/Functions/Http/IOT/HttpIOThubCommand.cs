using DatabaseFunctions.Models.Information;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Functions.Http.IOT
{
    public class HttpIOThubCommand
    {
        private readonly ILogger<HttpIOThubCommand> _logger;

        public HttpIOThubCommand(ILogger<HttpIOThubCommand> logger)
        {
            _logger = logger;
        }

        [Function("HttpIOThubCommand")]
        public async Task<IActionResult> SendCommand([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

                if (data == null || !data.ContainsKey("command"))
                {
                    _logger.LogWarning("Invalid request: Missing or invalid 'command' in request body.");
                    return new BadRequestResult();
                }

                string commandToSend = data["command"];

                await SendMqttCommandAsync(commandToSend);

                _logger.LogInformation("Command sent successfully.");
                return new OkResult();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing JSON: {ex.Message}\nStack trace: {ex.StackTrace}");
                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Uncaught error: {ex.Message}\nStack trace: {ex.StackTrace}");
                return new StatusCodeResult(500);
            }
        }

        async Task SendMqttCommandAsync(string commandToSend)
        {
            using (var deviceClient = DeviceClient.CreateFromConnectionString(ModelIOTInfo.connectionString, TransportType.Mqtt))
            {
                var message = new Message(Encoding.UTF8.GetBytes(commandToSend));
                await deviceClient.SendEventAsync(message);
            }
        }
    }
}
