namespace Zotapay.Models.Deposit
{
    using System.Runtime.Serialization;

    public class MGDepositCardResult : DepositResultBase
    {
        /// <summary>
        /// When code is 200, this parameter will include the following fields: depositUrl, merchantOrderID and orderID.
        /// </summary>
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public new DepositCardResponseData Data { get; set; }

    }
}
