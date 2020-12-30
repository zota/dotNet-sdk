namespace Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using ZotapaySDK.Models;
    using Moq;
    using Moq.Protected;
    using ZotapaySDK.Models.Deposit;

    /// <summary>
    /// Static class with pre-defined mock objects
    /// </summary>
    public static class Mocks
    {
        public static MGClient GetMockedMGClient(HttpClient httpClient)
        {
            return new MGClient(
                merchantSecret: "merchant-secret",
                merchantId: "merchant-id",
                requestUrl: "https://example.com",
                endpointId: "1234",
                httpClient: httpClient);
        }

        /// <summary>
        /// Mock of http client to have a predefined server response & status code
        /// </summary>
        /// <param name="statusCode">Desired status code to be responded</param>
        /// <param name="responseText">Response message text</param>
        /// <returns>System.Net.HttpClient</returns>
        public static HttpClient GetMockedHttp(HttpStatusCode statusCode, string responseText)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseText),
            };
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            return new HttpClient(handlerMock.Object);
        }

        /// <summary>
        /// Full deposit payload
        /// </summary>
        /// <returns>MGDepositRequest with everything set</returns>
        public static MGDepositRequest GetFullDepositRequest()
        {
            return new MGDepositRequest
            {
                MerchantOrderID = "QvE8dZshpKhaOmHY",
                OrderAmount = "100.00",
                CustomerEmail = "customer@test.com",
                OrderCurrency = "USD",
                MerchantOrderDesc = "desc",
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerAddress = "The Moon, hill 42",
                CustomerCity = "Sofia",
                CustomerCountryCode = "BG",
                CustomerZipCode = "1303",
                CustomerPhone = "123",
                CustomerIP = "127.0.0.1",
                RedirectUrl = "https://example-merchant.com/payment/return",
                CheckoutUrl = "https://example-merchant.com/deposit"
            };
        }

        /// <summary>
        /// Full deposit credit card payload
        /// </summary>
        /// <returns>MGDepositRequest with everything set</returns>
        public static MGDepositCardRequest GetFullDepositCardRequest()
        {
            return new MGDepositCardRequest
            {
                MerchantOrderID = "QvE8dZshpKhaOmHY",
                OrderAmount = "100.00",
                CustomerEmail = "customer@test.com",
                OrderCurrency = "USD",
                MerchantOrderDesc = "desc",
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerAddress = "The Moon, hill 42",
                CustomerCity = "Sofia",
                CustomerCountryCode = "BG",
                CustomerZipCode = "1303",
                CustomerPhone = "123",
                CustomerIP = "127.0.0.1",
                RedirectUrl = "https://example-merchant.com/payment/return",
                CheckoutUrl = "https://example-merchant.com/deposit"
            };
        }
    }
}
