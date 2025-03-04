namespace Zotapay.Models.Deposit
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using Zotapay.Contracts;

    /// <summary>
    /// Card payment request with card data already collected
    /// </summary>
    internal class CardDataRequest : IMGRequest
    {
        /// <summary>
        /// Expiration Year(e.g "2020" or just "20")
        /// </summary>
        [Required, StringLength(4, MinimumLength = 2)]
        [DataMember(Name = "cardExpirationYear")]
        internal string CardExpirationYear { get; set; }

        /// <summary>
        /// CVV / Security code
        /// </summary>
        [Required, StringLength(4, MinimumLength = 3)]
        [DataMember(Name = "cardCvv")]
        internal string CardCvv { get; set; }

        /// <summary>
        /// Expiration month(e.g "02")
        /// </summary>
        [Required, StringLength(2, MinimumLength = 1)]
        [DataMember(Name = "cardExpirationMonth")]
        internal string CardExpirationMonth { get; set; }

        /// <summary>
        /// Card holder name as appears on card
        /// </summary>
        [Required, StringLength(64, MinimumLength = 1)]
        [DataMember(Name = "cardHolderName")]
        internal string CardHolderName { get; set; }

        /// <summary>
        /// Card number (PAN)
        /// </summary>
        [Required, StringLength(16, MinimumLength = 12)]
        [DataMember(Name = "cardNumber")]
        internal string CardNumber { get; set; }

        void IMGRequest.GenerateSignature(string endpointId, string secret)
        {
            throw new System.NotImplementedException();
        }

        HttpMethod IMGRequest.GetMethod()
        {
            return HttpMethod.Post;
        }

        internal string DirectUrl;

        public string GetRequestUrl(string baseUrl, string endpoint)
        {
            if (DirectUrl != "")
            {
                return DirectUrl;
            }

            throw new System.MissingMemberException("DirectUrl is not set");
        }

        IMGResult IMGRequest.GetResultInstance()
        {
            return new MGDepositCardResult();
        }

        void IMGRequest.SetupPrivateMembers(string merchantId)
        {
            return;
        }
    }
}
