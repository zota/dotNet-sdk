using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ZotapaySDK.Contracts;

namespace ZotapaySDK.Models.Deposit
{
    public class MGDepositCardResult : DepositResultBase
    {
        /// <summary>
        /// When code is 200, this parameter will include the following fields: depositUrl, merchantOrderID and orderID.
        /// </summary>
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public new DepositCardResponseData Data { get; set; }

    }
}
