// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Text;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DatabaseFunctions.Functions.GridEventTrigger
{
    public class IotHubSendData
    {
        private readonly ILogger<IotHubSendData> _logger;

        public IotHubSendData(ILogger<IotHubSendData> logger)
        {
            _logger = logger;
        }

        [Function("IotDeviceSendData")]
        public async Task<IActionResult> IotDeviceSendData([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received event from Event Grid");

            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(eventGridEvent.Data.ToString());

                if (data == null)
                {
                    return new BadRequestObjectResult("Didn't find the data");
                }

                var bodyBase64 = data["body"].ToString();
                var bodyBytes = Convert.FromBase64String(bodyBase64);
                var bodyMessage = Encoding.UTF8.GetString(bodyBytes);

                var bodyData = JsonConvert.DeserializeObject<Dictionary<string, object>>(bodyMessage);
                var mainMessage = bodyData.ContainsKey("MainMessage") ? bodyData["MainMessage"].ToString() : "Main message not found";

                var customProperties = data["properties"];

                foreach (var key in data.Keys)
                {
                    _logger.LogInformation($"Data key: {key}, data value: {data[key]}");
                }

                if (customProperties == null)
                {
                    return new BadRequestObjectResult("Didn't find the properties");
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting temp: {ex}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
