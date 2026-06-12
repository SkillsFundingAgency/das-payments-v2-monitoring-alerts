using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.Payments.Monitoring.Alerts.Function.Services;

namespace SFA.DAS.Payments.Monitoring.Alerts.Function
{
    public class SendTeamsAlert
    {
        private readonly ITeamsService _teamsService;
        private readonly ILogger<SendTeamsAlert> _logger;

        public SendTeamsAlert(ITeamsService teamsService, ILogger<SendTeamsAlert> logger)
        {
            _teamsService = teamsService;
            _logger = logger;
        }

        [Function("HttpTrigger1")]
        public async Task<IActionResult> SendToChannelOne(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var teamsWebhookURL =
                Environment.GetEnvironmentVariable("TeamsWebhookURL", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger1 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            var result = await _teamsService.PostTeamsAlert(requestBody, teamsWebhookURL, _logger);
            return result == null ? new OkObjectResult("") : new OkObjectResult(result.exception + "\n" + result.innerException);
        
        }

        [Function("HttpTrigger2")]
        public async Task<IActionResult> SendToChannelTwo(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var teamsWebhookURL =
                Environment.GetEnvironmentVariable("TeamsWebhookURL2", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger2 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            await _teamsService.PostTeamsAlert(requestBody, teamsWebhookURL,_logger);

            return new OkObjectResult("");
        }
    }
}