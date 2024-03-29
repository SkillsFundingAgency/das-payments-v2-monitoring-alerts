﻿using System.Net;
using Moq.Protected;
using Moq;
using SFA.DAS.Payments.Monitoring.Alerts.Function.TypedClients;
using FluentAssertions;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SFA.DAS.Payments.Monitoring.Alerts.Function.UnitTests.TypedClients
{
    public class SlackClientTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandlerOkStatus;

        [SetUp]
        public void Setup() 
        {
            _mockHttpMessageHandlerOkStatus = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _mockHttpMessageHandlerOkStatus
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{ ""responseProperty"": ""responseValue"" }")
                })
                .Verifiable();
        } 

        [Test]
        public async Task SlackClientPostAlertNullUrlThrowsArgumentNullException()
        {
            //Arrange
            var httpClient = new HttpClient(_mockHttpMessageHandlerOkStatus.Object);
            var slackClient = new SlackClient(httpClient);

            //Act
            var jsonPayload = @"{ ""property"": ""value"" }";
            Func<Task<HttpResponseMessage>> act = () => slackClient.PostAsJsonAsync(null, jsonPayload);

            //Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'requestUrl')");
        }

        [Test]
        public async Task SlackClientPostAlertNullPayloadThrowsArgumentNullException()
        {
            //Arrange
            var httpClient = new HttpClient(_mockHttpMessageHandlerOkStatus.Object);
            var slackClient = new SlackClient(httpClient);

            //Act
            Func<Task<HttpResponseMessage>> act = () => slackClient.PostAsJsonAsync("http://someurl.com/somepath", null);

            //Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'jsonPayload')");
        }

        [Test]
        public async Task SlackClientPostAlertValidArgumentsCallsHttpClient()
        {
            //Arrange
            var httpClient = new HttpClient(_mockHttpMessageHandlerOkStatus.Object);
            var slackClient = new SlackClient(httpClient);

            //Act
            var jsonPayload = @"{ ""property"": ""value"" }";
            var postUri = "http://someurl.com/somepath";
            var result = await slackClient.PostAsJsonAsync(postUri, jsonPayload);

            //Assert
            _mockHttpMessageHandlerOkStatus.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri(postUri)),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task SlackClientPostAlertValidArgumentsSuccesReturnsValue()
        {
            //Arrange
            var httpClient = new HttpClient(_mockHttpMessageHandlerOkStatus.Object);
            var slackClient = new SlackClient(httpClient);

            //Act
            var jsonPayload = @"{ ""property"": ""value"" }";
            var postUri = "http://someurl.com/somepath";
            var result = await slackClient.PostAsJsonAsync(postUri, jsonPayload);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);   
        }

        [Test]
        public async Task SlackClientPostAlertValidArgumentsFailedPostingThrowsHttpResponseException()
        {
            //Arrange
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(@"{ ""responseProperty"": ""responseValue"" }")
                })
                .Verifiable();

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var slackClient = new SlackClient(httpClient);

            //Act
            var jsonPayload = @"{ ""property"": ""value"" }";
            var postUri = "http://someurl.com/somepath";
            Func<Task<HttpResponseMessage>> act = async () => await slackClient.PostAsJsonAsync(postUri, jsonPayload);

            //Assert
            await act
                .Should()
                .ThrowAsync<HttpRequestException>()
                .WithMessage("Response status code does not indicate success: 500 (Internal Server Error).");
        }
    }
}