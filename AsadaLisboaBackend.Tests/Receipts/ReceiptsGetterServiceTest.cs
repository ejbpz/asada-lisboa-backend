using System.Net;
using Microsoft.Extensions.Options;
using FluentAssertions;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Services.Receipts;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Tests.Receipts.Helpers;

namespace AsadaLisboaBackend.Tests.Receipts
{
    public class ReceiptsGetterServiceTest
    {
        private ReceiptsGetterService CreateService(
        HttpClient httpClient)
        {
            var options = Options.Create(new ReceiptOptions
            {
                DOMAIN_RECEIPTS = "https://test.com"
            });

            return new ReceiptsGetterService(
                httpClient,
                options);
        }

        [Fact]
        public async Task GetReceipt_Should_Return_WithDebt()
        {
            // Arrange
            var getStateHtml =
                HtmlFixtureHelper.Load("get-state.html");
            var receiptHtml =
                HtmlFixtureHelper.Load("receipt-with-debt.html");
            var httpClient = HttpClientMockHelper.Create(
                getStateHtml,
                receiptHtml);

            var service = CreateService(httpClient);

            // Act
            var result = await service.GetReceipt(12345);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("Juan Perez");
            result.ReceiptType.Should()
                .Be(ReceiptType.WithDebt);
            result.Table.Should().NotBeNull();
            result.Table!.Should().HaveCount(1);
            result.Chart1Url.Should()
                .Be("https://test.com/chart1.png");
        }

        [Fact]
        public async Task GetReceipt_Should_Return_WithoutDebt()
        {
            // Arrange
            var getStateHtml =
                HtmlFixtureHelper.Load("get-state.html");
            var receiptHtml =
                HtmlFixtureHelper.Load("receipt-without-debt.html");
            var httpClient = HttpClientMockHelper.Create(
                getStateHtml,
                receiptHtml);

            var service = CreateService(httpClient);

            // Act
            var result = await service.GetReceipt(12345);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("Juan Perez");
            result.ReceiptType.Should()
                .Be(ReceiptType.WithoutDebt);
            result.Table.Should().BeNull();
        }

        [Fact]
        public async Task GetReceipt_Should_Return_NotExists()
        {
            // Arrange
            var getStateHtml =
                HtmlFixtureHelper.Load("get-state.html");
            var receiptHtml =
                HtmlFixtureHelper.Load("receipt-not-found.html");
            var httpClient = HttpClientMockHelper.Create(
                getStateHtml,
                receiptHtml);

            var service = CreateService(httpClient);

            // Act
            var result = await service.GetReceipt(99999);

            // Assert
            result.Should().NotBeNull();
            result.ReceiptType.Should()
                .Be(ReceiptType.NotExists);
            result.UserName.Should().BeNull();
        }

        [Fact]
        public async Task GetReceipt_Should_Throw_When_HttpFails()
        {
            // Arrange
            var httpClient =
                HttpClientMockHelper.CreateError(
                    HttpStatusCode.InternalServerError);

            var service = CreateService(httpClient);

            // Act
            var action = async () =>
                await service.GetReceipt(123);

            // Assert
            await action.Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetReceiptDetails_Should_Map_Values_Correctly()
        {
            // Arrange
            var getStateHtml =
                HtmlFixtureHelper.Load("get-state.html");
            var consultDetailsHtml =
                HtmlFixtureHelper.Load("consult-details.html");
            var detailsHtml =
                HtmlFixtureHelper.Load("receipt-details.html");
            var httpClient = HttpClientMockHelper.Create(
                getStateHtml,
                consultDetailsHtml,
                detailsHtml);

            var service = CreateService(httpClient);

            // Act
            var result = await service
                .GetReceiptDetails(12345, 0);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("Juan Perez");
            result.Meter.Should().Be("12345");
            result.Total.Should().Be(15000);
            result.CubicMeters.Should().Be(20);
            result.CurrentReading.Should().Be(100);
            result.PreviousReading.Should().Be(80);
            result.Taxes.Should().Be(1950);
        }

        [Fact]
        public async Task GetReceiptDetails_Should_Return_Defaults_When_Fields_Are_Missing()
        {
            // Arrange
            var getStateHtml =
                HtmlFixtureHelper.Load("get-state.html");
            var consultDetailsHtml =
                HtmlFixtureHelper.Load("consult-details.html");
            var emptyDetailsHtml =
                "<html><body></body></html>";

            var httpClient = HttpClientMockHelper.Create(
                getStateHtml,
                consultDetailsHtml,
                emptyDetailsHtml);

            var service = CreateService(httpClient);

            // Act
            var result = await service
                .GetReceiptDetails(12345, 0);

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().BeNull();
            result.Total.Should().BeNull();
            result.CubicMeters.Should().BeNull();
        }
    }
}
