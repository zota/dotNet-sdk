﻿namespace ZotapaySDK.Callback
{
    using System.Runtime.Serialization;
    using ZotapaySDK.Static;

    /// <summary>
    /// Callback notification object which will be send from ZotaPay
    /// </summary>
    public class MGCallback
    {
        /// <summary>
        /// Indicates wether the signature is validated. If false, ErrorMessage property will contain reason
        /// </summary>
        public bool IsVerified { get; set; }

        internal void validate(string endpoint, string secret)
        {
            string expected = Hasher.ToSHA256($"{endpoint}{this.OrderID}{this.merchantOrderID}{this.Status}{this.Amount}{this.CustomerEmail}{secret}");
            IsVerified = (expected == this.Signature);
        }

        /// <summary>
        /// Transaction type is SALE.
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Transaction status, see Order Statuses for a full list of statuses.
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>
        /// Contains the error description if the transaction is declined or yields an error.
        /// </summary>
        [DataMember(Name = "errorMessage", EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The merchant EndpointID that the order was sent through.
        /// </summary>
        [DataMember(Name = "endpointID", EmitDefaultValue = false)]
        public string EndpointID { get; set; }

        /// <summary>
        /// The transaction identifier that was generated by the payment processor.
        /// </summary>
        [DataMember(Name = "processorTransactionID", EmitDefaultValue = false)]
        public string ProcessorTransactionID { get; set; }

        /// <summary>
        /// Order unique identifier generated by Zotapay.
        /// </summary>
        [DataMember(Name = "orderID", EmitDefaultValue = false)]
        public string OrderID { get; set; }

        /// <summary>
        /// Merchant-defined unique order identifier.
        /// </summary>
        [DataMember(Name = "merchantOrderID", EmitDefaultValue = false)]
        public string merchantOrderID { get; set; }

        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        public string Amount { get; set; }

        /// <summary>
        /// The currency of the transaction.
        /// </summary>
        [DataMember(Name = "currency", EmitDefaultValue = false)]
        public string Currency { get; set; }

        /// <summary>
        /// End-user email address.
        /// </summary>
        [DataMember(Name = "customerEmail", EmitDefaultValue = false)]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Merchant-defined optional custom parameter.
        /// </summary>
        [DataMember(Name = "customParam", EmitDefaultValue = false)]
        public string CustomParam { get; set; }

        /// <summary>
        /// A Json object with additional information that was collected during the payment process on Zotapay's environemnt.
        /// </summary>
        [DataMember(Name = "extraData", EmitDefaultValue = false)]
        public object ExtraData { get; set; }

        /// <summary>
        /// A Json object with a copy of the original Deposit Request sent by merchant server to Zotapay.
        /// </summary>
        [DataMember(Name = "originalRequest", EmitDefaultValue = false)]
        public object OriginalRequest { get; set; }

        /// <summary>
        /// Request checksum encrypted with SHA-256, see Request Signature section below for more details.
        /// </summary>
        [DataMember(Name = "signature", EmitDefaultValue = false)]
        public string Signature { get; set; }
    }
}
