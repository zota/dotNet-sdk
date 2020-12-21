using System;
using System.Threading;
using ZotapaySDK.Contracts;
using ZotapaySDK.Models;
using ZotapaySDK.Static;
using ZotapaySDK.Models.OrderStatusCheck;
using ZotapaySDK.Models.Payout;

namespace Examples
{
    class Example
    {
        static void Main(string[] args)
        {
            // "ZOTAPAY_MERCHANT_ID" 
            // "ZOTAPAY_MERCHANT_SECRET_KEY" 
            // "ZOTAPAY_ENDPOINT_ID" 
            // "ZOTAPAY_REQUEST_URL";

            // Environment variables are set in the only code for showcase purposes
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "merchant-id");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "merchant-secret-key");

            // ZotaPay's Meta Gate client constructed with environment variables
            MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);

            // Assemble deposit order data
            var DepositOrderRequest = new MGDepositRequest
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
            }

            // Redirect the end user to the payment url
            string paymentUrl = resp.Data.DepositUrl;

            // Initialize query status payload & send request
            var queryStatusCheckRequest = new MGQueryTxnRequest { MerchantOrderID = "QvE8dZshpKhaOmHY1", OrderID = "32453550" };
            MGQueryTxnResult orderResponse = client.CheckOrderStatus(queryStatusCheckRequest).Result;

            if (!orderResponse.IsSuccess) {
                // Check reason and handle failure
                string reason = orderResponse.Message;
                // ...
            }

            // Order status
            string status = orderResponse.Data.status;

            // Assemble payout order data
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
                CallbackUrl = "https://ens39ypv7jld8.x.pipedream.net",
            };

            // Create Meta-Gate client
            
            MGClient clientWithConfig = new MGClient(
                merchantSecret: "b9f9933d-364a-4653-b215-801b575ef164",
                endpointId: "400007",
                merchantId: "MISTER-MERCHANT",
                requestUrl: "https://secure.zotapay-stage.com"
                );

            // MGPayoutResult resp = clientWithConfig.InitPayout(payoutRequest).Result;
            // MGQueryTxnResult resp = clientWithConfig.CheckOrderStatus(QueryStatusCheckRequest).Result;
            // MGDepositResult resp = clientWithConfig.InitDeposit(DepositOrderRequest).Result;
            // MGDepositResult t =  isSuccess depositUrl 

            // resp.ApiResponse.DepositData 		
            // OrderID	"6948"	string
            // MerchantOrderID	"QvE8dZshpKhaOmHY1"	string /\/\ \\\\
            // DepositUrl	"https://kera.mereo.tech/api/v1/deposit/init/6948/0ca81b0354fd669b602b683ca11859635a1831d438ef276289ab653a310c8f76/"	string
            // 
            /*string rawCallback = "{\"type\":\"SALE\",\"status\":\"APPROVED\",\"errorMessage\":\"\",\"endpointID\":\"400009\",\"processorTransactionID\":\"279198e3-7277-4e28-a490-e02deec1a3cc\",\"orderID\":\"32453550\",\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"amount\":\"100.00\",\"currency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customParam\":\"\",\"extraData\":{\"dcc\":false,\"paymentMethod\":\"ONLINE\"},\"originalRequest\":{\"merchantOrderID\":\"QvE8dZshpKhaOmHY1\",\"merchantOrderDesc\":\"desc\",\"orderAmount\":\"100.00\",\"orderCurrency\":\"USD\",\"customerEmail\":\"customer@test.com\",\"customerFirstName\":\"John\",\"customerLastName\":\"Doe\",\"customerAddress\":\"The Moon, hill 42\",\"customerCountryCode\":\"BG\",\"customerCity\":\"Sofia\",\"customerZipCode\":\"1303\",\"customerPhone\":\"123\",\"customerIP\":\"127.0.0.1\",\"redirectUrl\":\"https://example-merchant.com/payment/return\",\"callbackUrl\":\"https://ens39ypv7jld8.x.pipedream.net\",\"checkoutUrl\":\"https://example-merchant.com/deposit\",\"signature\":\"0ca81b0354fd669b602b683ca11859635a1831d438ef276289ab653a310c8f76\",\"requestedAt\":\"0001-01-01T00:00:00Z\"},\"signature\":\"56e733fc383656e383e3d0f3c815399eb306388efd60c81f1c31def7f6806137\"}";         
            var callback = clientWithConfig.Parse(rawCallback);
            if (!callback.IsVerified)
            {
                Console.WriteLine(callback.ErrorMessage);
                return;
            }

            if (callback.Status == "APPROVED")
            {

            }*/

            Thread.Sleep(5000);
            Console.WriteLine(resp);
            Console.ReadLine();
        }
    }
}
