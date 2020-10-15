using System;
using System.Threading;
using ZotapaySDK.Contracts;
using ZotapaySDK.Models;
using ZotapaySDK.Static;

namespace Examples
{
    class Program
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
                MerchantOrderID = "abc",
                OrderAmount = "100.00",
                CustomerEmail = "abc",
                OrderCurrency = null,
            };

            // Create Meta-Gate client
            MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);
            MGClient clientWithConfig = new MGClient(
                merchantId: "MISTER-MERCHANT",
                merchantSecret: "b9f9933d-364a-4653-b215-801b575ef164",
                endpointId: "400009",
                requestUrl: "https://kera.mereo.tech"
                );

            MGDepositResult resp = clientWithConfig.InitDeposit(DepositOrderRequest).Result;

            // MGDepositResult t =  isSuccess depositUrl 

            // resp.ApiResponse.DepositData
            Thread.Sleep(5000);
            Console.WriteLine(resp);
            Console.ReadLine();
        }
    }
}
