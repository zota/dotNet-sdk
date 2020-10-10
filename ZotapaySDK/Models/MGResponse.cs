namespace ZotapaySDK.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// MGResponse class represents the Zotapay API response
    /// </summary>
    public class MGResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MGResponse() { }

        /// <summary>
        /// A status code representing the acceptance of the request by Zotapay server.
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string Code { get; set; }

        /// <summary>
        /// When code is other than 200, this parameter may hold information about the reason / error.
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        /// <summary>
        /// When code is 200, this parameter will include the following fields: depositUrl, merchantOrderID and orderID.
        /// </summary>
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public ResponseData Data { get; set; }
    }
}
