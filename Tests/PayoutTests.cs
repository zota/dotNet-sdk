﻿namespace Tests
{
    using NUnit.Framework;
    using System.Net;
    using Zotapay;
    using Zotapay.Models.Payout;
    using NUnit.Framework.Legacy;

    public class PayoutTests
    {
        [Test]
        public void PayoutOrderSuccess()
        {
            // Arrange
            MGPayoutResult expectedResult = new MGPayoutResult()
            {
                IsSuccess = true,
                Code = "200",
                Message = null,
                Data = new MGPayoutData { orderID = "123456", merchantOrderID = "merch-order" }
            };
            string messageSuccess = "{ \"code\": \"200\", \"data\": { \"orderID\": \"123456\", \"merchantOrderID\": \"merch-order\"} }";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.OK, messageSuccess);
            MGClient client = Mocks.GetMockedMGClient(httpClient: httpMock);
            var payoutRequest = new MGPayoutRequest
            {
                MerchantOrderID = "Q44mHY18",
                CustomerBankCode = "BBL",
                OrderAmount = "100.00",
                CustomerBankAccountNumber = "1234567",
                CustomerBankAccountName = "Don Jhoe",
                CustomerEmail = "customer@test.com",
                CustomerBankBranch = "bankBranch",
                CustomerBankProvince = "province",
                CustomerBankArea = "bankArea",
                CustomerBankRoutingNumber = "000",
                OrderCurrency = "USD",
                MerchantOrderDesc = "desc",
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerBankAddress = "The Moon, hill 42",
                CustomerBankZipCode = "1303",
                CustomerPhone = "123",
                CustomerIP = "127.0.0.1",
                RedirectUrl = "https://example-merchant.com/payment/return",
                CheckoutUrl = "https://example-merchant.com/deposit",
                CallbackUrl = "https://example-merchant.com/callback",
            };

            // Act
            MGPayoutResult actual = client.InitPayout(payoutRequest).Result;

            // ClassicAssert
            ClassicAssert.IsTrue(actual.IsSuccess);
            ClassicAssert.AreEqual(expectedResult.Code, actual.Code);
            ClassicAssert.AreEqual(expectedResult.Message, actual.Message);
            ClassicAssert.AreEqual(expectedResult.Data.orderID, actual.Data.orderID);
            ClassicAssert.AreEqual(expectedResult.Data.merchantOrderID, actual.Data.merchantOrderID);
        }

        [Test]
        public void PayoutOrderServerErrorHandled()
        {
            // Arrange
            MGPayoutResult expectedResult = new MGPayoutResult()
            {
                IsSuccess = true,
                Code = "500",
                Message = "internal server error",
                Data = null
            };
            string messageError = "{ \"code\": \"500\", \"message\": \"internal server error\" }";
            var httpMock = Mocks.GetMockedHttp(HttpStatusCode.InternalServerError, messageError);
            MGClient client = Mocks.GetMockedMGClient(httpClient: httpMock);
            var payoutRequest = new MGPayoutRequest
            {
                MerchantOrderID = "Q44mHY18",
                CustomerBankCode = "BBL",
                OrderAmount = "100.00",
                CustomerBankAccountNumber = "1234567",
                CustomerBankAccountName = "Don Jhoe",
                CustomerEmail = "customer@test.com",
                CustomerBankBranch = "bankBranch",
                CustomerBankProvince = "province",
                CustomerBankArea = "bankArea",
                CustomerBankRoutingNumber = "000",
                OrderCurrency = "USD",
                MerchantOrderDesc = "desc",
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerBankAddress = "The Moon, hill 42",
                CustomerBankZipCode = "1303",
                CustomerPhone = "123",
                CustomerIP = "127.0.0.1",
                RedirectUrl = "https://example-merchant.com/payment/return",
                CheckoutUrl = "https://example-merchant.com/deposit",
                CallbackUrl = "https://example-merchant.com/callback",
            };

            // Act
            MGPayoutResult actual = client.InitPayout(payoutRequest).Result;

            // ClassicAssert
            ClassicAssert.IsFalse(actual.IsSuccess);
            ClassicAssert.AreEqual(expectedResult.Code, actual.Code);
            ClassicAssert.AreEqual(expectedResult.Message, actual.Message);
            ClassicAssert.AreEqual(expectedResult.Data, actual.Data);
        }
    }
}
