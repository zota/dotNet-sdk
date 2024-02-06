namespace Tests
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;

    public class CallbackTests
    {
        [Test]
        public void CallbackParseSuccess()
        {
            // Arrange
            var client = Mocks.GetMockedMGClient(null);
            var callback = "{\"type\":\"SALE\",\"status\":\"APPROVED\",\"errorMessage\":\"\",\"endpointID\":\"400009\",\"processorTransactionID\":\"279198e3-7277-4e28-a490-e02deec1a3cc\",\"orderID\":\"32453550\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"amount\":\"100.00\",\"currency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customParam\":\"\",\"extraData\":{\"dcc\":false,\"paymentMethod\":\"ONLINE\"},\"originalRequest\":{\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"merchantOrderDesc\":\"desc\",\"orderAmount\":\"100.00\",\"orderCurrency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customerFirstName\":\"John\",\"customerLastName\":\"Doe\",\"customerAddress\":\"The Moon, hill 42\",\"customerCountryCode\":\"BG\",\"customerCity\":\"Sofia\",\"customerZipCode\":\"1303\",\"customerPhone\":\"123\",\"customerIP\":\"127.0.0.1\",\"redirectUrl\":\"https://example-merchant.com/payment/return\",\"callbackUrl\":\"https://ens39ypv7jld8.x.pipedream.net\",\"checkoutUrl\":\"https://example-merchant.com/deposit\",\"signature\":\"0ca81b0354fd669b602b683ca11859635a1831d438ef276289ab653a310c8f76\",\"requestedAt\":\"0001-01-01T00:00:00Z\"},\"signature\":\"7df9a67035e2c2f145c51653bd25aa56658954dac1a4ce8f77ddc4f991ecab1a\"}";

            // Act
            var actual = client.Parse(callback);

            // ClassicAssert
            ClassicAssert.IsTrue(actual.IsVerified);
            ClassicAssert.IsEmpty(actual.ErrorMessage);
            ClassicAssert.AreEqual("APPROVED", actual.Status);
        }

        [Test]
        public void CallbackParseUnverifiedSignature()
        {
            // Arrange
            var client = Mocks.GetMockedMGClient(null);
            var callback = "{\"type\":\"SALE\",\"status\":\"APPROVED\",\"errorMessage\":\"\",\"endpointID\":\"400009\",\"processorTransactionID\":\"279198e3-7277-4e28-a490-e02deec1a3cc\",\"orderID\":\"32453550\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"amount\":\"100.00\",\"currency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customParam\":\"\",\"extraData\":{\"dcc\":false,\"paymentMethod\":\"ONLINE\"},\"originalRequest\":{\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"merchantOrderDesc\":\"desc\",\"orderAmount\":\"100.00\",\"orderCurrency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customerFirstName\":\"John\",\"customerLastName\":\"Doe\",\"customerAddress\":\"The Moon, hill 42\",\"customerCountryCode\":\"BG\",\"customerCity\":\"Sofia\",\"customerZipCode\":\"1303\",\"customerPhone\":\"123\",\"customerIP\":\"127.0.0.1\",\"redirectUrl\":\"https://example-merchant.com/payment/return\",\"callbackUrl\":\"https://ens39ypv7jld8.x.pipedream.net\",\"checkoutUrl\":\"https://example-merchant.com/deposit\",\"signature\":\"0ca81b0354fd669b602b683ca11859635a1831d438ef276289ab653a310c8f76\",\"requestedAt\":\"0001-01-01T00:00:00Z\"},\"signature\":\"7df9a67035e2c2f145c51653bd25aa56658954dac114ce8f77ddc4f991ecab1a\"}";
            var expectedErrorMessage = "Unverified signature, expected: 7df9a67035e2c2f145c51653bd25aa56658954dac1a4ce8f77ddc4f991ecab1a, got: 7df9a67035e2c2f145c51653bd25aa56658954dac114ce8f77ddc4f991ecab1a";

            // Act
            var actual = client.Parse(callback);

            // ClassicAssert
            ClassicAssert.IsFalse(actual.IsVerified);
            ClassicAssert.AreEqual(expectedErrorMessage, actual.ErrorMessage);
        }

        [Test]
        public void CallbackParseCatchInvalidInput()
        {
            // Arrange
            var client = Mocks.GetMockedMGClient(null);
            var callback = "invalid json message";
            var expectedErrorMessage = "Unexpected character encountered while parsing value: i. Path '', line 0, position 0.";

            // Act
            var actual = client.Parse(callback);

            // ClassicAssert
            ClassicAssert.IsFalse(actual.IsVerified);
            ClassicAssert.AreEqual(expectedErrorMessage, actual.ErrorMessage);
        }
    }
}
