using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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
        public async Task<IActionResult> SendToChannelOne([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var teamsWebhookURL = Environment.GetEnvironmentVariable("TeamsWebhookURL", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger1 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            var result = await _teamsService.PostTeamsAlert(requestBody, teamsWebhookURL);

            if (result != null)
            {
                return new ObjectResult(result.exception + "\n" + result.innerException)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkResult();
        }

        [Function("HttpTrigger2")]
        public async Task<IActionResult> SendToChannelTwo([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var teamsWebhookURL = Environment.GetEnvironmentVariable("TeamsWebhookURL2", EnvironmentVariableTarget.Process);

            _logger.LogInformation("HttpTrigger2 function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation($"Request: {requestBody}.");

            var result = await _teamsService.PostTeamsAlert(requestBody, teamsWebhookURL);

            if (result != null)
            {
                return new ObjectResult(result.exception + "\n" + result.innerException)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkResult();
        }
    }
}