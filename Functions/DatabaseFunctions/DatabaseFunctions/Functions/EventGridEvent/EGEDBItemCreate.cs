// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Text;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using DatabaseFunctions.Functions.HttpTrigger;
using DatabaseFunctions.Models.Information;
using DatabaseFunctions.Models.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DatabaseFunctions.Functions.GridEventTrigger
{
    public class EGEDBItemCreate
    {
        private readonly ILogger<EGEDBItemCreate> _logger;

        public EGEDBItemCreate(ILogger<EGEDBItemCreate> logger)
        {
            _logger = logger;
        }

        [Function("EGEDBItemCreate")]
        public IActionResult IotDeviceSendData([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received event from Event Grid");

            try
            {
                _logger.LogInformation(eventGridEvent.Data.ToString());

                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(eventGridEvent.Data.ToString());

                if (data == null)
                {
                    return new BadRequestObjectResult("Didn't find the data");
                }

                foreach (var key in data.Keys)
                {
                    _logger.LogInformation($"Data key: {key}, data value: {data[key]}");
                }

                var bodyBase64 = data["body"].ToString();
                var bodyBytes = Convert.FromBase64String(bodyBase64);
                var bodyString = Encoding.UTF8.GetString(bodyBytes);

                _logger.LogInformation($"Decoded body: {bodyString}");

                Dictionary<string, string> body = JsonConvert.DeserializeObject<Dictionary<string, string>>(bodyString);

                _logger.LogInformation($"Body: {body}");

                foreach (var key in body.Keys)
                {
                    _logger.LogInformation($"Body key: {key}, value: {body[key]}");
                }

                ModelDBItemCreate.InsertRecord(_logger, ModelDBAccountInfo.builder, "TelemetryData", body);

                return new OkResult();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing object: {ex}");
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting temp: {ex}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
