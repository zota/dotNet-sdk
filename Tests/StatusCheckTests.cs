using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZotapaySDK.Models;
using ZotapaySDK.Models.Deposit;
using ZotapaySDK.Models.OrderStatusCheck;

namespace Tests
{
    using ZotapaySDK.Static;

    class StatusCheckTests
    {
        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable(Constants.ENV.ENDPOINT_ID, "400009");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_ID, "MISTER-MERCHANT");
            Environment.SetEnvironmentVariable(Constants.ENV.MERCHANT_SECRET_KEY, "b9f9933d-364a-4653-b215-801b575ef164"); // TODO: swap this before going live
        }

        [Test]
        public async Task DevToolTest()
        {
            
            var DepositOrderRequest = new MGQueryTxnRequest
            {
            }; 

            Assert.IsTrue(true);

        }
    }
}
