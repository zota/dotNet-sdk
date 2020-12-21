using System;
using System.Threading;
using ZotapaySDK.Contracts;
using ZotapaySDK.Models;
using ZotapaySDK.Static;
using ZotapaySDK.Models.OrderStatusCheck;

namespace Examples
{
    class Example
    {
        static void Main(string[] args)
        {

            // dotnet add package ZotapaySDK // nuget

            // Environment variables set in the code for showcase purposes
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "MISTER-MERCHANT");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "b9f9933d-364a-4653-b215-801b575ef164");

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
                CallbackUrl = "https://ens39ypv7jld8.x.pipedream.net",
            };

            var QueryStatusCheckRequest = new MGQueryTxnRequest { MerchantOrderID = "QvE8dZshpKhaOmHY1", OrderID = "32453550" };

            // Create Meta-Gate client
            MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);
            MGClient clientWithConfig = new MGClient(
                merchantSecret: "b9f9933d-364a-4653-b215-801b575ef164",
                endpointId: "400009",
                merchantId: "sample_merchant_id",
                requestUrl: "https://secure.zotapay-stage.com"
                );

            MGQueryTxnResult resp = clientWithConfig.CheckOrderStatus(QueryStatusCheckRequest).Result;

            // MGDepositResult t =  isSuccess depositUrl 

            // resp.ApiResponse.DepositData
            Thread.Sleep(5000);
            Console.WriteLine(resp);
            Console.ReadLine();
        }
    }
}
