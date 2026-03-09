using ExchangeRateApi.Controllers;
using ExchangeRateApi.Interfaces;
using ExchangeRateApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ExchangeRateTests
{
    public sealed class ExchangeControllerTests
    {
        [Fact]
        public async Task Convert_ReturnsOk_WithCalculatedResponse()
        {
            // Arrange
            var mockProvider = new Mock<IExchangeRateProvider>();
            var mockLogger = new Mock<ILogger<ExchangeRateController>>();

            mockProvider
                .Setup(p => p.GetRateAsync("AUD", "USD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(0.70m);

            var controller = new ExchangeRateController(mockProvider.Object, mockLogger.Object);

            var request = new ExchangeRequest
            {
                Amount = 5m,
                InputCurrency = "AUD",
                OutputCurrency = "USD"
            };

            var result = await controller.Convert(request, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsType<ExchangeResponse>(ok.Value);

            Assert.Equal(5m, payload.Amount);
            Assert.Equal("AUD", payload.InputCurrency);
            Assert.Equal("USD", payload.OutputCurrency);


            mockProvider.Verify(p => p.GetRateAsync("AUD", "USD", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
           