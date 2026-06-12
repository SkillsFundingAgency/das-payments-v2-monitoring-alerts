using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.Payments.Monitoring.Alerts.Function.Services;

namespace SFA.DAS.Payments.Monitoring.Alerts.Function
{
    public class SendSlackAlert
    {
        private readonly ISlackService _slackService;
        private readonly ILogger<SendSlackAlert> _logger;

        public SendSlackAlert(ISlackService slackService, ILogger<SendSlackAlert> logger)
        {
            _slackService = slackService;
            _logger = logger;
        }

        [Function("HttpTrigger1")]
        public async Task<IActionResult> SendToChannelOne(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var slackChannelUri =
                Environment.GetEnvironmentVariable("SlackChannelUri", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger1 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            await _slackService.PostSlackAlert(requestBody, slackChannelUri);

            return new OkObjectResult("");
        }

        [Function("HttpTrigger2")]
        public async Task<IActionResult> SendToChannelTwo(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var slackChannelUri =
                Environment.GetEnvironmentVariable("SlackChannelUri2", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger2 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            await _slackService.PostSlackAlert(requestBody, slackChannelUri);

            return new OkObjectResult("");
        }
    }
}