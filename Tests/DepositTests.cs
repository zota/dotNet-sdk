namespace Tests
{
    using NUnit.Framework;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using ZotapaySDK.Models;
    using ZotapaySDK.Static;

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "MISTER-MERCHANT");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "b9f9933d-364a-4653-b215-801b575ef164"); // TODO: swap this before going live
        }

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
            MGClient client = new MGClient(httpClient: httpMock);
            var DepositRequest = new MGDepositRequest
            {
                MerchantOrderID = "QvE8dZshpKhaOmHY",OrderAmount = "100.00",CustomerEmail = "customer@test.com",OrderCurrency = "USD",MerchantOrderDesc = "desc",CustomerFirstName = "John",
                CustomerLastName = "Doe",CustomerAddress = "The Moon, hill 42",CustomerCity = "Sofia",CustomerCountryCode = "BG",CustomerZipCode = "1303",
                CustomerPhone = "123",CustomerIP = "127.0.0.1",RedirectUrl = "https://example-merchant.com/payment/return",CheckoutUrl = "https://example-merchant.com/deposit"
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
            MGClient client = new MGClient(httpClient: httpMock);
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
            MGDepositResult expectedResult = new MGDepositResult()
            {
                IsSuccess = false,
                Code = "400",
                Message = "endpoint currency mismatch",
                Data = null,
            };
            string responseMessage = "!@#$%^&*() -- not a valid json message, that will cause an exception -- !@#$%^&*()";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.InternalServerError, responseMessage);
            MGClient client = new MGClient(httpClient: httpMock);
            var DepositRequest = Mocks.GetFullDepositRequest();

            // Act
            MGDepositResult actual = client.InitDeposit(DepositRequest).Result;

            // Assert
            Assert.IsFalse(actual.IsSuccess);
            Assert.IsFalse(string.IsNullOrWhiteSpace(actual.Message));
        }

        [Test]
        public void EmptyRequestShowsFullErrorMessage()
        {
            // Arrange
            MGClient clientMock = new MGClient();
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
            MGDepositRequest request = new MGDepositRequest() {
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
            string merchantId = "merchant-id";
            string merchantSecret = "12345";
            string endpointId = "400009";
            string requestUrl = "https://google.com";

            // Act
            MGClient client = new MGClient(merchantId, merchantSecret, endpointId, requestUrl);

            // Assert
            Assert.IsNotNull(client);
        }

        // TODO: rm
        [Test]
        public async Task DevToolTest()
        {
            /*
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var DepositOrderRequest = new MGDepositRequest
            {
                MerchantOrderID = unixTimestamp.ToString(),
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
                RedirectUrl = "httppppppppppppppppp",
                CheckoutUrl = "htppppppppppppp11111111"
            }; // The OrderCurrency field is required. | The CustomerEmail field is not a valid e-mail address.
            MGClient clientWithConfig = new MGClient(
                 merchantId: "MISTER-MERCHANT",
                 merchantSecret: "b9f9933d-364a-4653-b215-801b575ef164",
                 endpointId: "400009",
                 requestUrl: "https://kera.mereo.tech"
                 );
            var resp = await clientWithConfig.InitDeposit(DepositOrderRequest);
            // bad arguments: customerFirstName, customerLastName, customerAddress, customerCity, customerCountryCode, customerZipCode, customerPhone, customerIP, redirectUrl, checkoutUrl
            Assert.IsTrue(true);
            */
        }
    }
}
