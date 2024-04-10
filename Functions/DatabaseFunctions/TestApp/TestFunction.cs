using System;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Azure;
using System.Text;

namespace TestApp
{
    public class TestFunction
    {
        private readonly ILogger<TestFunction> _logger;

        public TestFunction(ILogger<TestFunction> logger)
        {
            _logger = logger;
        }

        [Function("TestDisplayData")]
        public void DebugInformation([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation($"Event received: {eventGridEvent.EventType}");
            _logger.LogInformation($"Event data: {eventGridEvent.Data}");
            _logger.LogInformation($"Event subject: {eventGridEvent.Subject}");
            _logger.LogInformation($"Event timestamp: {eventGridEvent.EventTime}");         
        }

        [Function("TestEventGridTrigger")]
        public async Task<IActionResult> TriggerEvent([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation("Received event from Event Grid");

            try
            {
                _logger.LogInformation($"\nEvent grid data: {eventGridEvent.Data.ToObjectFromJson()}");
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

                //_logger.LogInformation($"Data: {data}\n");
                //_logger.LogInformation($"Properties: {customProperties}");

                // Send main message and custom properties to IoT Hub or other processing logic

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
