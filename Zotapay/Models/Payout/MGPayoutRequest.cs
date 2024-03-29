﻿namespace Zotapay.Models.Payout
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using Zotapay.Contracts;
    using Zotapay.Static;
    using static Zotapay.Static.Constants;

    /// <summary>
    /// Payout Request
    /// </summary>
    public class MGPayoutRequest : IMGRequest
    {
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
        [Required, StringLength(50)]
        [EmailAddress]
        [DataMember(Name = "customerEmail", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// End user first name
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "customerFirstName", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerFirstName { get; set; }

        /// <summary>
        /// End user last name
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "customerLastName", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerLastName { get; set; }

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
        [Required, StringLength(64)]
        [DataMember(Name = "customerIP", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerIP { get; set; }

        /// <summary>
        /// URL for end user redirection upon transaction completion, regardless of order status
        /// </summary>
        [StringLength(255)]
        [DataMember(Name = "redirectUrl", IsRequired = true, EmitDefaultValue = false)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// URL the order status will be sent to, see Callback section below for more details
        /// </summary>
        [StringLength(255)]
        [DataMember(Name = "callbackUrl", IsRequired = false, EmitDefaultValue = false)]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// End user bank code
        /// </summary>
        [Required, StringLength(11)]
        [DataMember(Name = "customerBankCode", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankCode { get; set; }

        /// <summary>
        /// End user bank account number
        /// </summary>
        [Required, StringLength(64)]
        [DataMember(Name = "customerBankAccountNumber", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankAccountNumber { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "customerBankAccountName", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankAccountName { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "customerBankBranch", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankBranch { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(128)]
        [DataMember(Name = "customerBankAddress", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankAddress { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(15)]
        [DataMember(Name = "customerBankZipCode", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankZipCode { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(64)]
        [DataMember(Name = "customerBankProvince", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankProvince { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [Required, StringLength(64)]
        [DataMember(Name = "customerBankArea", IsRequired = true, EmitDefaultValue = false)]
        public string CustomerBankArea { get; set; }

        /// <summary>
        /// End user bank account name
        /// </summary>
        [StringLength(64)]
        [DataMember(Name = "customerBankRoutingNumber", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerBankRoutingNumber { get; set; }

        /// <summary>
        /// Merchant-defined optional custom parameter
        /// </summary>
        [StringLength(128)]
        [DataMember(Name = "customParam", IsRequired = false, EmitDefaultValue = false)]
        public string CustomParam { get; set; }

        /// <summary>
        /// Merchant-defined optional custom parameter
        /// </summary>
        [StringLength(128)]
        [DataMember(Name = "checkoutUrl", IsRequired = false, EmitDefaultValue = false)]
        public string CheckoutUrl { get; set; }

        /// <summary>
        /// Request checksum encrypted with SHA-256
        /// </summary>
        [DataMember(Name = "signature", IsRequired = true, EmitDefaultValue = false)]
        public string Signature;

        /// <summary>
        /// End user two letter country code
        /// </summary>
        [StringLength(2)]
        [DataMember(Name = "customerCountryCode", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerCountryCode { get; set; }

        /// <summary>
        /// End user personal ID number
        /// </summary>
        [StringLength(20)]
        [DataMember(Name = "customerPersonalID", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerPersonalID { get; set; }

        /// <summary>
        /// End user bank account number control digit
        /// </summary>
        [StringLength(2)]
        [DataMember(Name = "customerBankAccountNumberDigit", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerBankAccountNumberDigit { get; set; }

        /// <summary>
        /// End user bank account type, either checking or savings
        /// </summary>
        [StringLength(8)]
        [DataMember(Name = "customerBankAccountType", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerBankAccountType { get; set; }

        /// <summary>
        /// End user bank swift code
        /// </summary>
        [StringLength(35)]
        [DataMember(Name = "customerBankSwiftCode", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerBankSwiftCode { get; set; }

        /// <summary>
        /// End user bank branch control digit
        /// </summary>
        [StringLength(2)]
        [DataMember(Name = "customerBankBranchDigit", IsRequired = false, EmitDefaultValue = false)]
        public string CustomerBankBranchDigit { get; set; }

        public void GenerateSignature(string endpointId, string secret)
        {
            string toSign = $"{endpointId}{this.MerchantOrderID}{this.OrderAmount}{this.CustomerEmail}{this.CustomerBankAccountNumber}{secret}";
            this.Signature = Hasher.ToSHA256(toSign);
        }

        public string GetRequestUrl(string baseUrl, string endpoint)
        {
            return baseUrl + string.Format(URL.PATH_PAYOUT, endpoint);
        }

        public IMGResult GetResultInstance()
        {
            return new MGPayoutResult();
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
