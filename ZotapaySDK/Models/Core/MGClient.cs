namespace ZotapaySDK.Models
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.Text;
    using ZotapaySDK.Contracts;
    using ZotapaySDK.Models.Deposit;
    using static ZotapaySDK.Static.Constants;
    using static ZotapaySDK.Models.UserAgent;
    using ZotapaySDK.Models.OrderStatusCheck;

    /// <summary>
    /// Zotapay engine for all the integration methods
    /// </summary>
    public class MGClient
    {
        private string merchantSecret;
        private string endpoint;
        private string requestUrl;
        private static HttpClient http;
        private string rawResponse;
        private string merchantId;

        /// <summary>
        /// Constructor for MGClient with string parameters
        /// </summary>
        /// <param name="merchantId">MerchantID as received from Zotapay</param>
        /// <param name="merchantSecret">MerchantSecretKey as received from Zotapay</param>
        /// <param name="endpointId">EndpointID as received from Zotapay</param>
        /// <param name="requestUrl">Base URL, either https://api.zotapay-sandbox.com or https://api.zotapay.com</param>
        /// <param name="httpClient">Http client will be set as static, default is new System.Net.Http.HttpClient</param>
        public MGClient(string merchantSecret, string endpointId, string requestUrl, string merchantId, HttpClient httpClient = null)
        {
            this.merchantSecret = merchantSecret;
            this.endpoint = endpointId;
            this.requestUrl = requestUrl;
            this.merchantId = merchantId;
            MGClient.http = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Constructor for MGClient with environment variables set
        /// </summary>
        /// <param name="useConstantUrl">Indicate wether to grab the url from constants, default is false</param>
        /// <param name="environment">Environment to be used, if constant url is true, can be Sandbox or Live, default is Live</param>
        /// <param name="baseUrl">When useConstantUrl is set to false, base url must be passed here manually</param>
        /// <param name="httpClient">Http client will be set as static, default is System.Net.Http.HttpClient</param>
        public MGClient(bool useConstantUrl = true, MGEnvironment environment = MGEnvironment.Live, string baseUrl = "", HttpClient httpClient = null)
        {
            this.merchantSecret = Environment.GetEnvironmentVariable(ENV.MERCHANT_SECRET_KEY);
            this.endpoint = Environment.GetEnvironmentVariable(ENV.ENDPOINT_ID);
            this.merchantId = Environment.GetEnvironmentVariable(ENV.MERCHANT_ID);
            this.requestUrl = Environment.GetEnvironmentVariable(ENV.REQUEST_URL);
            this.requestUrl = baseUrl;
            if (useConstantUrl)
            {
                this.requestUrl = (environment == MGEnvironment.Live) ? URL.LIVE : URL.SANDBOX;
            }
            MGClient.http = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Requests to Zotapay API. Accepts and returns request/result interfaces, which are exposed to the user
        /// through concrete type wrappers for the different services: deposit, payout, status check, etc.
        /// </summary>
        /// <param name="request">Object containing data required for deposit, payout or order status check request</param>
        /// <returns>Task with the request results</returns>
        private async Task<IMGResult> Send(IMGRequest request)
        {
            // Create the correct type of result
            IMGResult result = request.GetResultInstance();

            // Validate MGClient
            string MGClientErrorMessage = ValidateMGClient();
            if (!string.IsNullOrEmpty(MGClientErrorMessage)) 
            {
                result.IsSuccess = false;
                result.Message = MGClientErrorMessage;
                return result;
            }

            // Validate request
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true))
            {
                // Indicate failed validation and populate error message envelope
                result.IsSuccess = false;
                result.Message = string.Join(" | ", validationResults);
                return result;
            }

            // Get request specific data
            request.SetupPrivateMembers(this.merchantId);
            request.GenerateSignature(this.endpoint, this.merchantSecret);
            string requestUrl = request.GetRequestUrl(this.requestUrl, this.endpoint);
            
            try
            {
                // Create json payload string and wrap it as http content
                string payload = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                http.DefaultRequestHeaders.Add("User-Agent", GetUserAgentHeader());

                // Request & parse response async
                var response = await http.PostAsync(requestUrl, httpContent);
                this.rawResponse = response.Content.ReadAsStringAsync().Result;
                result = (IMGResult)JsonConvert.DeserializeObject(
                        this.rawResponse, 
                        Type.GetType(result.ToString())
                    );
                result.IsSuccess = ((result.Code == API.CODE_SUCCESS) && (response.StatusCode == System.Net.HttpStatusCode.OK));
                return result;
            } 
            catch (Exception e)
            {
                // Indicate fail and reason
                result.IsSuccess = false;
                string respMsgFormat = "{0}: \nRaw Response: {1}";
                result.Message = string.IsNullOrEmpty(this.rawResponse) ? e.Message : string.Format(respMsgFormat, e.Message, this.rawResponse);
                return result;
            }
        }

        /// <summary>
        /// Make a deposit request
        /// </summary>
        /// <param name="requestPayload">Deposit request payload</param>
        /// <returns>Task<MGDepositResult> containing Zotapay API response</returns>
        public async Task<MGDepositResult> InitDeposit(MGDepositRequest requestPayload)
        {
            if (requestPayload.GetType() != typeof(MGDepositRequest)) 
            {
                return new MGDepositResult { 
                    IsSuccess = false, 
                    Message = "Got MGDepositCardRequest instead of MGDepositRequest. Please use MGClient.InitCardDeposit for credit card integrations." 
                };
            }
            var result = await Send(requestPayload);
            return (MGDepositResult)result;
        }

        /// <summary>
        /// Make a deposit credit card request
        /// </summary>
        /// <param name="requestPayload">Deposit request payload with card data</param>
        /// <returns>Task<DepositCardResponseData> containing Zotapay API response</returns>
        public async Task<MGDepositCardResult> InitCardDeposit(MGDepositCardRequest requestPayload)
        {
            var result = await Send(requestPayload);
            return (MGDepositCardResult)result;
        }

        /// <summary>
        /// Make an order status check request
        /// </summary>
        /// <param name="requestPayload">Status check request payload</param>
        /// <returns>Task<MGQueryTxnResult> containing Zotapay API response</returns>
        public async Task<MGQueryTxnResult> CheckOrderStatus(MGQueryTxnRequest requestPayload)
        {
            var result = await Send(requestPayload);
            return (MGQueryTxnResult)result;
        }

        /// <summary>
        /// Validates MGClient instance for missing properties
        /// </summary>
        /// <returns>null if valid or error message string</returns>
        private string ValidateMGClient()
        {
            string errorMessage = "";
            errorMessage += string.IsNullOrWhiteSpace(this.endpoint) ? "endpoint " : "";
            errorMessage += string.IsNullOrWhiteSpace(this.merchantSecret) ? "merchantSecret " : "";
            errorMessage += string.IsNullOrWhiteSpace(this.requestUrl) ? "requestUrl " : "";
            errorMessage += string.IsNullOrWhiteSpace(this.merchantId) ? "merchantId " : "";
            if (!string.IsNullOrWhiteSpace(errorMessage)) 
            {
                errorMessage = "MGClient missing parameters: " + errorMessage;
            }
            return errorMessage.Trim();
        }
    }
}
