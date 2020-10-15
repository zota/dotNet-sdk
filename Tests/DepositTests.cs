using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ZotapaySDK.Contracts;
using ZotapaySDK.Models;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DepositSuccess()
        {
           
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

        // TODO: rm
        [Test]
        public async Task DevToolTest()
        {
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
        }
    }
}

/*
 [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DepositSuccess()
        {
            // Arrange
            MGDepositRequest depositRequest = new MGDepositRequest();
            Mock<MGClient> client = new Mock<MGClient>();
            client.Setup(x => x.InitDeposit(depositRequest)).Returns(Task.FromResult(new MGResult() { IsSuccess = false, ErrorList = "", }));
            DepositData expectedData = new DepositData() { DepositUrl = "", MerchantOrderID = "", OrderID = "" };
            // ApiResponse expectedRes = new ApiResponse() { Code = "200", Data = expectedData, Message = "" };

            // Act
            MGResult result = client.Object.InitDeposit(depositRequest).Result;
            //await client.InitDeposit(depositRequest);

            // Assert
            Assert.AreEqual(result, "");
            Assert.AreEqual(1, 1);
        }

        [Test]
        public async Task NonFullParametersThrow()
        {
            // Arrange
            var expectedResponse = new MGResult() { ErrorList = "The MerchantOrderID field is required. | The MerchantOrderDesc field is required. | The OrderAmount field is required. | The OrderCurrency field is required." };

            MGDepositRequest depositRequest = new MGDepositRequest();
            MGClient client = new MGClient("merchantId", "secret1234!!", "endpoint", "sd");

            MGResult actualResult = await client.InitDeposit(depositRequest);
            Assert.AreEqual(actualResult.IsSuccess, expectedResponse.IsSuccess);
            Assert.AreEqual(actualResult.ErrorList, expectedResponse.ErrorList);
            Assert.AreEqual(1, 1);
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
            string secret = "b9f9933d-364a-4653-b215-801b575ef164";

            // Act
            request.generateDepositSignature(endpointId, secret);

            // Assert

            Assert.AreEqual("e5cf05a439b41d295c8fd208bc6b27c81cea211e6d28889158d3262065601ffb", request.Signature);
        }

        // TODO: rm
        [Test]
        public async Task DevToolTest()
        {
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
        }
 */
