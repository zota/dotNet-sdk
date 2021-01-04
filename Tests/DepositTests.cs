namespace Tests
{
    using NUnit.Framework;
    using System.Net;
    using System.Threading.Tasks;
    using Zotapay.Models;
    using Zotapay.Models.Deposit;
    using Zotapay;

    public class Tests
    {
        [Test]
        public void DepositSuccess()
        {
            // Arrange
            MGDepositResult expectedResult = new MGDepositResult()
            {
                IsSuccess = true,
                Code = "200",
                Message = null,
                Data = new DepositResponseData()
                {
                    DepositUrl = "https://api.zotapay.com/api/v1/deposit/init/8b3a6b89697e8ac8f45d964bcc90c7ba41764acd/",
                    MerchantOrderID = "QvE8dZshpKhaOmHY",
                    OrderID = "8b3a6b89697e8ac8f45d964bcc90c7ba41764acd",
                }
            };
            string messageSuccess = "{ \"code\": \"200\", \"data\": { \"depositUrl\": \"https://api.zotapay.com/api/v1/deposit/init/8b3a6b89697e8ac8f45d964bcc90c7ba41764acd/\", \"merchantOrderID\": \"QvE8dZshpKhaOmHY\", \"orderID\": \"8b3a6b89697e8ac8f45d964bcc90c7ba41764acd\" } }";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.OK, messageSuccess);
            MGClient client = Mocks.GetMockedMGClient(httpClient: httpMock);
            var DepositRequest = new MGDepositRequest
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

            // Act
            MGDepositResult actual = client.InitDeposit(DepositRequest).Result;

            // Assert
            Assert.IsTrue(actual.IsSuccess);
            Assert.AreEqual(expectedResult.Data.DepositUrl, actual.Data.DepositUrl);
            Assert.AreEqual(expectedResult.Data.MerchantOrderID, actual.Data.MerchantOrderID);
            Assert.AreEqual(expectedResult.Data.OrderID, actual.Data.OrderID);
            Assert.AreEqual(expectedResult.Code, actual.Code);
            Assert.AreEqual(expectedResult.Message, actual.Message);
        }

        [Test]
        public void DepositUnsuccessful()
        {
            // Arrange
            MGDepositResult expectedResult = new MGDepositResult()
            {
                IsSuccess = false,
                Code = "400",
                Message = "endpoint currency mismatch",
                Data = null,
            };
            string responseMessage = "{ \"code\": \"400\", \"message\": \"endpoint currency mismatch\" } ";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.BadRequest, responseMessage);
            MGClient client = Mocks.GetMockedMGClient(httpClient: httpMock);
            var DepositRequest = Mocks.GetFullDepositRequest();

            // Act
            MGDepositResult actual = client.InitDeposit(DepositRequest).Result;

            // Assert
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual(expectedResult.Data, actual.Data);
            Assert.AreEqual(expectedResult.Code, actual.Code);
            Assert.AreEqual(expectedResult.Message, actual.Message);
        }

        [Test]
        public void DepositCatchesUnexpectedResponse()
        {
            // Arrange
            string responseMessage = "!@#$%^&*() -- not a valid json message, that will cause an exception -- !@#$%^&*()";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.InternalServerError, responseMessage);
            MGClient client = Mocks.GetMockedMGClient(httpClient: httpMock);
            var DepositRequest = Mocks.GetFullDepositRequest();
            string expectedErrorMessage = "Unexpected character encountered while parsing value: !. Path '', line 0, position 0.: \n" +
                "Raw Response: !@#$%^&*() -- not a valid json message, that will cause an exception -- !@#$%^&*()";

            // Act
            MGDepositResult actual = client.InitDeposit(DepositRequest).Result;

            // Assert
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual(expectedErrorMessage, actual.Message);
        }

        [Test]
        public void EmptyRequestShowsFullErrorMessage()
        {
            // Arrange
            MGClient clientMock = Mocks.GetMockedMGClient(null);
            MGDepositRequest depositRequest = new MGDepositRequest();
            string expectedErrorMessage = "The MerchantOrderID field is required. | The MerchantOrderDesc field is required. |" +
                " The OrderAmount field is required. | The OrderCurrency field is required. | The CustomerFirstName field is required. |" +
                " The CustomerLastName field is required. | The CustomerAddress field is required. | The CustomerCountryCode field is required. |" +
                " The CustomerCity field is required. | The CustomerZipCode field is required. | The CustomerPhone field is required. |" +
                " The CustomerIP field is required. | The RedirectUrl field is required. | The CheckoutUrl field is required.";

            // Act
            MGDepositResult actualDepositResult = clientMock.InitDeposit(depositRequest).Result;

            // Assert
            Assert.IsFalse(actualDepositResult.IsSuccess);
            Assert.AreEqual(expectedErrorMessage, actualDepositResult.Message);
        }

        [Test]
        public void TestDepositSignature()
        {
            // Arrange
            MGDepositRequest request = new MGDepositRequest()
            {
                MerchantOrderID = "12345",
                OrderAmount = "100.00",
                CustomerEmail = "customer@test.com",
            };
            string endpointId = "400009";
            string secret = "merchant-secret-@#$!";
            string expectedSignature = "e53b3ca225d4c1d6228c46159081295bda7cab19ca17369b469079a22e5629cb";

            // Act
            request.GenerateSignature(endpointId, secret);

            // Assert
            Assert.AreEqual(expectedSignature, request.Signature);
        }

        [Test]
        public void MGClientProperlyCreatedWithStringConstructor()
        {
            // Arrange
            string merchantSecret = "12345";
            string endpointId = "400009";
            string requestUrl = "https://google.com";
            string merchantId = "merchant_id";

            // Act
            MGClient client = new MGClient(merchantSecret, endpointId, requestUrl, merchantId);

            // Assert
            Assert.IsNotNull(client);
        }

        [Test]
        public void MGClientConstructorShouldFail()
        {
            // Arrange
            MGClient client = new MGClient(useConstantUrl: false);
            MGDepositRequest request = new MGDepositRequest();
            string expectedErrorMessage = "MGClient missing parameters: endpoint merchantSecret requestUrl merchantId";

            // Act
            MGDepositResult actual = client.InitDeposit(request).Result;

            // Assert
            Assert.IsFalse(actual.IsSuccess);
            Assert.AreEqual(expectedErrorMessage, actual.Message);
        }

        [Test]
        public async Task CardDepositShouldShowFullErrorMessage()
        {
            // Arrange
            MGDepositCardRequest DepositOrderRequest = Mocks.GetFullDepositCardRequest();
            MGClient clientWithConfig = Mocks.GetMockedMGClient(null);
            string expectedErrorMessage = "The CardExpirationYear field is required. | The CardCvv field is required. | The CardExpirationMonth field is required. | The CardHolderName field is required. | The CardNumber field is required.";

            // Act
            var actualResult = await clientWithConfig.InitCardDeposit(DepositOrderRequest);

            // Assert
            Assert.IsFalse(actualResult.IsSuccess);
            Assert.AreEqual(expectedErrorMessage, actualResult.Message);
        }
    }
}
