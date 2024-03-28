using System;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace TestApp
{
    public class TestFunction
    {
        private readonly ILogger<TestFunction> _logger;

        public TestFunction(ILogger<TestFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(TestFunction))]
        public void Run([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            try
            {
                var eventData = JsonConvert.DeserializeObject<MyEventData>(eventGridEvent.Data.ToString());


                var properties = eventData.Properties;
                _logger.LogInformation($"Properties found: {properties}");

                foreach (var property in properties.Keys)
                {
                    _logger.LogInformation($"Properties found (key: {property}, value: {properties[property]})");
                }

                _logger.LogInformation($"Received event with subject: {eventGridEvent.Subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing event: {ex}");
            }
        }
    }

    public class MyEventData
    {
        public Dictionary<string, string> Properties { get; set; }
        public Dictionary<string, string> SystemProperties { get; set; }
        public string Body { get; set; }
    }
}
