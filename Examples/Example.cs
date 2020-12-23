namespace Examples
{
    using System;
    using ZotapaySDK.Models;
    using ZotapaySDK.Static;
    using ZotapaySDK.Models.OrderStatusCheck;
    using ZotapaySDK.Models.Payout;
    using ZotapaySDK.Callback;

    class Example
    {
        static void Main(string[] args)
        {
            // MGClient client = CreateClientWithEnvVarExample();
            MGClient client = CreateClientWithExplicitConfig();

            DepositExample(client);
            QueryOrderExample(client);
            PayoutExample(client);
            ParseCallbackExample(client);
        }

        public static MGClient CreateClientWithExplicitConfig()
        {
            // Credentials are hardcoded for showcase purposes - do not use hardcoded credentials on live environment
            MGClient clientWithConfig = new MGClient(
                merchantSecret: "merchant-secret-key",
                endpointId: "400007",
                merchantId: "merchant-id",
                requestUrl: "https://secure.zotapay-stage.com"
                );
            return clientWithConfig;
        }

        public static MGClient CreateClientWithEnvVarExample()
        {
            /*
                The following are to be set as environment variables:
                "ZOTAPAY_MERCHANT_ID" 
                "ZOTAPAY_MERCHANT_SECRET_KEY" 
                "ZOTAPAY_ENDPOINT_ID" 
                "ZOTAPAY_REQUEST_URL"; 
            */

            // Environment variables set in the code only for showcase purposes - do not use hardcoded credentials on live environment
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "merchant-id");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "merchant-secret-key");

            // ZotaPay's Meta Gate client constructed with environment variables
            MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox); // Constants.MGEnvironment.Live
            return client;
        }

        public static void ParseCallbackExample(MGClient client) {
            string exampleRawJson = "{\"type\":\"SALE\",\"status\":\"APPROVED\",\"errorMessage\":\"\",\"endpointID\":\"400009\",\"processorTransactionID\":\"279198e3-7277-4e28-a490-e02deec1a3cc\",\"orderID\":\"32453550\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"amount\":\"100.00\",\"currency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customParam\":\"\",\"extraData\":{\"dcc\":false,\"paymentMethod\":\"ONLINE\"},\"originalRequest\":{\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"merchantOrderDesc\":\"desc\",\"orderAmount\":\"100.00\",\"orderCurrency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customerFirstName\":\"John\",\"customerLastName\":\"Doe\",\"customerAddress\":\"The Moon, hill 42\",\"customerCountryCode\":\"BG\",\"customerCity\":\"Sofia\",\"customerZipCode\":\"1303\",\"customerPhone\":\"123\",\"customerIP\":\"127.0.0.1\",\"redirectUrl\":\"https://example-merchant.com/payment/return\",\"callbackUrl\":\"https://ens39ypv7jld8.x.pipedream.net\",\"checkoutUrl\":\"https://example-merchant.com/deposit\",\"signature\":\"0ca81b0354fd669b602b683ca11859635a1831d438ef276289ab653a310c8f76\",\"requestedAt\":\"0001-01-01T00:00:00Z\"},\"signature\":\"7df9a67035e2c2f145c51653bd25aa56658954dac114ce8f77ddc4f991ecab1a\"}";
            MGCallback callback = client.Parse(exampleRawJson);

            // handle errors or unverified signatures
            if (!callback.IsVerified)
            {
                string reason = callback.ErrorMessage;
                // ...
                return;
            }

            string orderStatus = callback.Status;
            Console.WriteLine(orderStatus);
        }

        public static void PayoutExample(MGClient client) {
            // Assemble payout order data
            MGPayoutRequest payoutRequest = new MGPayoutRequest
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
                CallbackUrl = "https://ens39ypv7jld8.x.pipedream.net",
            };

            // Initiate deposit order request
            MGPayoutResult resp = client.InitPayout(payoutRequest).Result;

            // Check the request status
            if (!resp.IsSuccess)
            {
                // handle unsuccessful request
                string reasonForFailure = resp.Message;
                // ...
                return;
            }

            // Capture ZotaPay order id
            string zotapayOrderId = resp.Data.orderID;
            Console.WriteLine(zotapayOrderId);
        }

        public static void QueryOrderExample(MGClient client) {
            // Initialize query status payload & send request
            var queryStatusCheckRequest = new MGQueryTxnRequest { MerchantOrderID = "QvE8dZshpKhaOmHY1", OrderID = "32453550" };
            MGQueryTxnResult orderResponse = client.CheckOrderStatus(queryStatusCheckRequest).Result;

            if (!orderResponse.IsSuccess)
            {
                // Check reason and handle failure
                string reason = orderResponse.Message;
                // ...
                return;
            }

            // Order status
            string status = orderResponse.Data.status;
            Console.WriteLine(status);
        }

        public static void DepositExample(MGClient client) {
            // Assemble deposit order data
            MGDepositRequest DepositOrderRequest = new MGDepositRequest
            {
                MerchantOrderID = "QvE8dZshpKhaOmHY1",
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
                CheckoutUrl = "https://example-merchant.com/deposit",
                CallbackUrl = "https://example-merchant.com/payment/callback",
            };

            // Initiate deposit order request
            MGDepositResult resp = client.InitDeposit(DepositOrderRequest).Result;

            // Check the request status
            if (!resp.IsSuccess)
            {
                // handle unsuccessful request
                string reasonForFailure = resp.Message;
                // ...
                return;
            }

            // Redirect the end user to the payment url
            string paymentUrl = resp.Data.DepositUrl;
            Console.WriteLine(paymentUrl);
        }
    }
}
