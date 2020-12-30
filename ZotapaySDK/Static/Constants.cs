using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ZotapaySDK.Static
{
    /// <summary>
    /// Zotapay constants class
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Environment variables keys constants
        /// </summary>
        public static class ENV
        {
            /// <summary>
            /// MerchantID as received from Zotapay
            /// </summary>
            public const string MERCHANT_ID = "ZOTAPAY_MERCHANT_ID";

            /// <summary>
            /// MerchantSecretKey as received from Zotapay
            /// </summary>
            public const string MERCHANT_SECRET_KEY = "ZOTAPAY_MERCHANT_SECRET_KEY";

            /// <summary>
            /// EndpointID as received from Zotapay
            /// </summary>
            public const string ENDPOINT_ID = "ZOTAPAY_ENDPOINT_ID";

            /// <summary>
            /// https://api.zotapay-sandbox.com or https://api.zotapay.com
            /// </summary>
            public const string REQUEST_URL = "ZOTAPAY_REQUEST_URL";
        }

        /// <summary>
        /// URLs and path constatns
        /// </summary>
        public static class URL
        {
            private const string BASE_URL = "/api/v1";

            /// <summary>
            /// Sandbox environment, used for integration and testing purposes. 
            /// </summary>
            public const string SANDBOX = "https://api.zotapay-sandbox.com";

            /// <summary>
            /// Live environment.
            /// </summary>
            public const string LIVE = "https://api.zotapay.com";

            /// <summary>
            /// Deposit path with an endpoint id 
            /// </summary>
            public const string PATH_DEPOSIT = BASE_URL + "/deposit/request/{0}/";

            /// <summary>
            /// Order status check path
            /// </summary>
            public const string PATH_STATUS_CHECK = BASE_URL + "/query/order-status/";

            /// <summary>
            /// Order status check path
            /// </summary>
            public const string PATH_PAYOUT = BASE_URL + "/payout/request/{0}/";

        }

        /// <summary>
        /// Zotapay API constants
        /// </summary>
        public static class API
        {
            /// <summary>
            /// Code indicating successful api request
            /// </summary>
            public const string CODE_SUCCESS = "200";
        }

        /// <summary>
        /// Setting to indicate MG environment to be used
        /// </summary>
        public enum MGEnvironment
        {
            Sandbox = 0,
            Live = 1,
        }
    }
}