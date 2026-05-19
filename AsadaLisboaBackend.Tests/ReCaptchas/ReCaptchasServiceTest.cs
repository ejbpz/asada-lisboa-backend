using AsadaLisboaBackend.Services.ReCaptchas;
using AsadaLisboaBackend.Tests.Helpers;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.OptionsPattern;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Text;

namespace AsadaLisboaBackend.Tests.ReCaptchas
{
    public class ReCaptchasServiceTest
    {
        private readonly Mock<ILogger<ReCaptchasService>> _loggerMock;

        public ReCaptchasServiceTest()
        {
            _loggerMock = new Mock<ILogger<ReCaptchasService>>();
        }

        private ReCaptchasService CreateService(
            HttpResponseMessage response,
            string secretKey = "SECRET_KEY")
        {
            var handler = new MockHttpMessageHandler(
                (_, _) => Task.FromResult(response));

            var httpClient = new HttpClient(handler);

            var options = Options.Create(new ReCaptchaOptions
            {
                SECRET_KEY = secretKey
            });

            return new ReCaptchasService(
                httpClient,
                _loggerMock.Object,
                options);
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Throw_When_Request_Is_Null()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var service = CreateService(response);

            // Act
            Func<Task> act = async () =>
                await service.ReCaptchaValidation(null!);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Throw_When_Request_Is_Whitespace()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var service = CreateService(response);

            // Act
            Func<Task> act = async () =>
                await service.ReCaptchaValidation("   ");

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Throw_When_SecretKey_Is_Invalid()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var service = CreateService(response, " ");

            // Act
            Func<Task> act = async () =>
                await service.ReCaptchaValidation("TOKEN");

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Return_False_When_Response_Is_Not_Success()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var service = CreateService(response);

            // Act
            var result = await service.ReCaptchaValidation("TOKEN");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Return_False_When_Response_Is_Null()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var service = CreateService(response);

            // Act
            var result = await service.ReCaptchaValidation("TOKEN");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Return_True_When_ReCaptcha_Is_Valid()
        {
            // Arrange
            var json = """
            {
                "success": true
            }
            """;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json")
            };

            var service = CreateService(response);

            // Act
            var result = await service.ReCaptchaValidation("TOKEN");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Return_False_When_ReCaptcha_Is_Invalid()
        {
            // Arrange
            var json = """
            {
                "success": false
            }
            """;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json")
            };

            var service = CreateService(response);

            // Act
            var result = await service.ReCaptchaValidation("TOKEN");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReCaptchaValidation_Should_Send_Correct_Request()
        {
            // Arrange
            HttpRequestMessage? capturedRequest = null;

            var handler = new MockHttpMessageHandler((request, _) =>
            {
                capturedRequest = request;

                return Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("""
                        {
                            "success": true
                        }
                        """,
                        Encoding.UTF8,
                        "application/json")
                    });
            });

            var httpClient = new HttpClient(handler);

            var options = Options.Create(new ReCaptchaOptions
            {
                SECRET_KEY = "SECRET"
            });

            var service = new ReCaptchasService(
                httpClient,
                _loggerMock.Object,
                options);

            // Act
            await service.ReCaptchaValidation("TOKEN");

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Method.Should().Be(HttpMethod.Post);
            capturedRequest.RequestUri!.ToString()
                .Should().Be(Constants.DOMAIN_RECAPTCHA);
        }
    }
}
