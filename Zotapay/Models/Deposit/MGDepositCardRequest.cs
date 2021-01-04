namespace Zotapay.Models.Deposit
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Zotapay.Contracts;

    /// <summary>
    /// Card payment request with card data already collected
    /// </summary>
    public class MGDepositCardRequest : MGDepositRequest
    {
        /// <summary>
        /// Expiration Year(e.g "2020" or just "20")
        /// </summary>
        [Required, StringLength(4, MinimumLength = 2)]
        [DataMember(Name = "cardExpirationYear")]
        public string CardExpirationYear { get; set; }

        /// <summary>
        /// CVV / Security code
        /// </summary>
        [Required, StringLength(4, MinimumLength = 3)]
        [DataMember(Name = "cardCvv")]
        public string CardCvv { get; set; }

        /// <summary>
        /// Expiration month(e.g "02")
        /// </summary>
        [Required, StringLength(2, MinimumLength = 1)]
        [DataMember(Name = "cardExpirationMonth")]
        public string CardExpirationMonth { get; set; }

        /// <summary>
        /// Card holder name as appears on card
        /// </summary>
        [Required, StringLength(64, MinimumLength = 1)]
        [DataMember(Name = "cardHolderName")]
        public string CardHolderName { get; set; }

        /// <summary>
        /// Card number (PAN)
        /// </summary>
        [Required, StringLength(16, MinimumLength = 12)]
        [DataMember(Name = "cardNumber")]
        public string CardNumber { get; set; }

        public override IMGResult GetResultInstance()
        {
            return new MGDepositCardResult();
        }
    }
}
