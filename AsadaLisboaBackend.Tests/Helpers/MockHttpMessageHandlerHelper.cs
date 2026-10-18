namespace AsadaLisboaBackend.Tests.Helpers;

public class MockHttpMessageHandlerHelper(
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return handler(request, cancellationToken);
    }
}