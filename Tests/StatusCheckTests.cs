namespace Tests
{
    using NUnit.Framework;
    using System.Net;
    using ZotapaySDK.Models.OrderStatusCheck;

    class StatusCheckTests
    {
        [Test]
        public void QueryRequestShouldHandleError()
        {
            // Arrange
            var queryRequest = new MGQueryTxnRequest { };
            var client = Mocks.GetMockedMGClient(null);
            var expectedMessage = "The OrderID field is required. | The MerchantOrderID field is required.";

            // Act 
            MGQueryTxnResult actual = client.CheckOrderStatus(queryRequest).Result;

            // Assert
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public void QueryRequestSuccess()
        {
            // Arrange
            var queryRequest = new MGQueryTxnRequest
            {
                OrderID = "1234567",
                MerchantOrderID = "merch-order-id",
            };
            string messageSuccess = "{\"code\":\"200\",\"data\":{\"type\":\"SALE\",\"status\":\"APPROVED\",\"errorMessage\":\"\",\"endpointID\":\"1050\",\"processorTransactionID\":\"\",\"orderID\":\"8b3a6b89697e8ac8f45d964bcc90c7ba41764acd\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY\",\"amount\":\"500.00\",\"currency\":\"THB\",\"customerEmail\":\"customer@email-address.com\",\"customParam\":\"{\\\"UserId\\\":\\\"e139b447\\\"}\",\"extraData\":\"\",\"request\":{\"merchantID\":\"EXAMPLE-MERCHANT-ID\",\"orderID\":\"8b3a6b89697e8ac8f45d964bcc90c7ba41764acd\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY\",\"timestamp\":\"1564617600\"}}}";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.OK, messageSuccess);
            var client = Mocks.GetMockedMGClient(httpClient: httpMock);

            // Act
            var actual = client.CheckOrderStatus(queryRequest).Result;

            // Assert
            Assert.IsTrue(actual.IsSuccess);
            Assert.AreEqual("APPROVED", actual.Data.status);
        }
    }
}
