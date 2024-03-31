// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Text;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using DatabaseFunctions.Functions.HttpTrigger;
using DatabaseFunctions.Models.Database;
using DatabaseFunctions.Models.Database.Items;
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
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(eventGridEvent.Data.ToString());

                if (data == null)
                {
                    return new BadRequestObjectResult("Didn't find the data");
                }        

                foreach (var key in data.Keys)
                {
                    _logger.LogInformation($"Data key: {key}, data value: {data[key]}");
                }
                
                var customProperties = data["properties"];

                if (customProperties == null)
                {
                    return new BadRequestObjectResult("Didn't find the properties");
                }

                _logger.LogInformation($"Custom Properties type: {customProperties.GetType()}");
                _logger.LogInformation($"Custom Properties: {customProperties}");

                var myProperties = JsonConvert.DeserializeObject<Dictionary<string, string>>(customProperties.ToString());

                _logger.LogInformation($"My props: {myProperties}");

                foreach (var key in myProperties.Keys)
                {
                    _logger.LogInformation($"My properties key: {key}, value: {myProperties[key]}");
                }

                ModelDBItemCreate.InsertRecord(_logger, ModelDBAccountInfo.builder, "Sensor", myProperties);

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
