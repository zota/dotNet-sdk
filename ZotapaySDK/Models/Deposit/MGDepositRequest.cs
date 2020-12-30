namespace ZotapaySDK.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using ZotapaySDK.Contracts;
    using ZotapaySDK.Static;
    using static ZotapaySDK.Static.Constants;

    /// <summary>
    /// Deposit request
    /// </summary>
    [DataContract]
    public class MGDepositRequest : IMGRequest
    {
        // todo try default without dataMember
        // OR use strategy from newtonsoft

        /// <summary>
	    /// Default constructor
		/// </summary>
        public MGDepositRequest() { }

        /// <summary>
        /// Merchant-defined unique order identifier
        /// </summary>
        [Required, StringLength(128, MinimumLength = 1)]
        [DataMember(Name = "merchantOrderID")]
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// Brief order description
        /// </summary>
        [Required, StringLength(128)] 
        [DataMember(Name = "merchantOrderDesc", IsRequired = true, EmitDefaultValue = false)]
        public string MerchantOrderDesc { get; set; }

        /// <summary>
        /// Amount to be charged, must be specified with delimiter, e.g. 1.50 for USD is 1 dollar and 50 cents
        /// </summary>
        [Required, StringLength(24)]
        [DataMember(Name = "orderAmount", IsRequired = true, EmitDefaultValue = false)]
        public string OrderAmount { get; set; }

        /// <summary>
        /// Currency to be charged in, three-letter ISO 4217 currency code
        /// See https://doc.zotapay.com/deposit/1.0/#currency-codes for a full list of currency codes
        /// </summary>
        [Required, StringLength(3)]
        [DataMember(Name = "orderCurrency", IsRequired = true, EmitDefaultValue = false)]
        public string OrderCurrency { get; set; }

        /// <summary>
        /// End user email address
        /// </summary>
        [StringLength(50)]
        [EmailAddress]
        [DataMember(Name = "customerEmail", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// End user first name
        /// </summary>
        [Required, StringLength(50)]
        [DataMember(Name = "customerFirstName", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerFirstName { get; set; }

        /// <summary>
        /// End user last name
        /// </summary>
        [Required, StringLength(50)]
        [DataMember(Name = "customerLastName", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerLastName { get; set; }

        /// <summary>
        /// End user address
        /// </summary>
        [Required, StringLength(50)]
        [DataMember(Name = "customerAddress", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerAddress { get; set; }

        /// <summary>
        /// End user country, two-letter ISO 3166-1 Alpha-2 country code
        /// See https://doc.zotapay.com/deposit/1.0/#country-codes for a full list of country codes
        /// </summary>
        [Required, StringLength(2)]
        [DataMember(Name = "customerCountryCode", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerCountryCode { get; set; }

        /// <summary>
        /// End user city
        /// </summary>
        [Required, StringLength(50)]
        [DataMember(Name = "customerCity", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerCity { get; set; }

        /// <summary>
        /// Required for US, CA and AU countries. End user state/province, two-letter state code
        /// see https://doc.zotapay.com/deposit/1.0/#state-codes for a full list of state codes
        /// </summary>
        [StringLength(3)]
        [DataMember(Name = "customerState", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerState { get; set; }

        /// <summary>
        /// End user postal code
        /// </summary>
        [Required, StringLength(10)]
        [DataMember(Name = "customerZipCode", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerZipCode { get; set; }

        /// <summary>
        /// End user full international telephone number, including country code
        /// </summary>
        [Phone]
        [Required, StringLength(15)]
        [DataMember(Name = "customerPhone", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerPhone { get; set; }

        /// <summary>
        /// End user IPv4/IPv6 address
        /// </summary>
        [Required, StringLength(20)]
        [DataMember(Name = "customerIP", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerIP { get; set; }

        /// <summary>
        /// End user bank code
        /// </summary>
        [StringLength(8)]
        [DataMember(Name = "customerBankCode", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankCode { get; set; }

        /// <summary>
        /// URL for end user redirection upon transaction completion, regardless of order status
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "redirectUrl", IsRequired = true, EmitDefaultValue = false)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// URL the order status will be sent to, see Callback section below for more details
        /// </summary>
        [StringLength(128)]
        [DataMember(Name = "callbackUrl", IsRequired = false, EmitDefaultValue = false)]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// The original URL from where the end-user started the deposit request (a URL in Merchants' website)
        /// </summary>
        [Required, StringLength(256)]
        [DataMember(Name = "checkoutUrl", IsRequired = true, EmitDefaultValue = false)]
        public string CheckoutUrl { get; set; }

        /// <summary>
        /// Merchant-defined optional custom parameter
        /// </summary>
        [StringLength(128)]
        [DataMember(Name = "customParam", IsRequired = false, EmitDefaultValue = false)]
        public string CustomParam { get; set; }

        /// <summary>
        /// Preferred payment form language (ISO 639-1 code)
        /// </summary>
        [StringLength(2)]
        [DataMember(Name = "language", IsRequired = false, EmitDefaultValue = false)]
        public string Language { get; set; }

        /// <summary>
        /// Request checksum encrypted with SHA-256
        /// </summary>
        [DataMember(Name = "signature", IsRequired = true, EmitDefaultValue = false)]
        public string Signature;

        /// <summary>
        /// Sets the Signature property with the generated SHA-256 deposit auth signature
        /// </summary>
        /// <param name="endpointId">Zotapay supplied endpoint id</param>
        /// <param name="secret">Zotapay supplied secret key</param>
        /// <returns>The computed hash in hexadecimal formatted string</returns>
        public void GenerateSignature(string endpointId, string secret)
        {
            // string to sign
            string toSign = $"{endpointId}{this.MerchantOrderID}{this.OrderAmount}{this.CustomerEmail}{secret}";
            this.Signature = Hasher.ToSHA256(toSign);
        }

        /// <summary>
        /// Gets the full request url
        /// </summary>
        /// <param name="baseUrl">Base url of the domain</param>
        /// <param name="path">Full request path</param>
        /// <returns>The full deposit request url</returns>
        public string GetRequestUrl(string baseUrl, string endpoint)
        {
            string urlPath = string.Format(URL.PATH_DEPOSIT, endpoint);
            return baseUrl + urlPath;
        }

        public virtual IMGResult GetResultInstance()
        {
            return new MGDepositResult();
        }

        HttpMethod IMGRequest.GetMethod()
        {
            return HttpMethod.Post;
        }

        void IMGRequest.SetupPrivateMembers(string merchantId)
        {
            return;
        }
    }
}
