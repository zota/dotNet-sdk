using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZotapaySDK.Models;
using ZotapaySDK.Static;

namespace Examples
{

    
    class Program
    {
        public static void Main(string[] args)
        {
            // Environment variables set in the code for showcase purposes
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "000");
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

            
            MGClient client = new MGClient(useConstantUrl: true, environment: Constants.MGEnvironment.Sandbox);
            var resp = client.InitDeposit(DepositOrderRequest).ConfigureAwait(false);

            Console.ReadLine();
        }
    }
}
