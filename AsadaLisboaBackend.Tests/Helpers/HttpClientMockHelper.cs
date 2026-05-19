using System.Net;
using System.Text;
using Moq;
using Moq.Protected;

namespace AsadaLisboaBackend.Tests.Receipts.Helpers
{
    public static class HttpClientMockHelper
    {
        public static HttpClient Create(params string[] htmlResponses)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            var setupSequence = handlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());

            foreach (var html in htmlResponses)
            {
                setupSequence.ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        html,
                        Encoding.UTF8,
                        "text/html")
                });
            }

            return new HttpClient(handlerMock.Object);
        }

        public static HttpClient CreateError(HttpStatusCode statusCode)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode
                });

            return new HttpClient(handlerMock.Object);
        }
    }
}