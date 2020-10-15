namespace ZotapaySDK.Models
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using static ZotapaySDK.Static.Constants;
    using Newtonsoft.Json;
    using System.Text;
    using System.Net.Http.Headers;
    using static ZotapaySDK.Models.UserAgent;
    using ZotapaySDK.Contracts;

    public class MGClient
    {
        private string merchantId;
        private string merchantSecret;
        private string endpoint;
        private string requestUrl;
        private static HttpClient http;

        /// <summary>
        /// Constructor for MGClient with string parameters
        /// </summary>
        /// <param name="merchantId">MerchantID as received from Zotapay</param>
        /// <param name="merchantSecret">MerchantSecretKey as received from Zotapay</param>
        /// <param name="endpointId">EndpointID as received from Zotapay</param>
        /// <param name="requestUrl">Base URL, either https://api.zotapay-sandbox.com or https://api.zotapay.com</param>
        /// <param name="httpClient">Http client will be set as static, default is new System.Net.Http.HttpClient</param>
        public MGClient(string merchantId, string merchantSecret, string endpointId, string requestUrl, HttpClient httpClient = null)
        {
            this.merchantId = merchantId;
            this.merchantSecret = merchantSecret;
            this.endpoint = endpointId;
            this.requestUrl = requestUrl;
            MGClient.http = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Constructor for MGClient with environment variables set
        /// </summary>
        /// <param name="useConstantUrl">Indicate wether to grab the url from constants, default is false</param>
        /// <param name="environment">Environment to be used, if constant url is true, can be Sandbox or Live, default is Live</param>
        /// <param name="baseUrl">When useConstantUrl is set to false, base url must be passed here manually</param>
        /// <param name="client">Http client will be set as static, default is System.Net.Http.HttpClient</param>
        public MGClient(bool useConstantUrl = true, MGEnvironment environment = MGEnvironment.Live, string baseUrl = "", HttpClient httpClient = null)
        {
            this.merchantId = Environment.GetEnvironmentVariable(ENV.MERCHANT_ID);
            this.merchantSecret = Environment.GetEnvironmentVariable(ENV.MERCHANT_SECRET_KEY);
            this.endpoint = Environment.GetEnvironmentVariable(ENV.ENDPOINT_ID);
            this.requestUrl = Environment.GetEnvironmentVariable(ENV.REQUEST_URL);
            this.requestUrl = baseUrl;
            if (useConstantUrl)
            {
                this.requestUrl = (environment == MGEnvironment.Live) ? URL.LIVE : URL.SANDBOX;
            } 

            MGClient.http = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// Requests to Zotapay API
        /// </summary>
        /// <param name="request">Object containing data required for deposit, payout or order status check request</param>
        /// <returns>Task with the request results</returns>
        private async Task<IMGResult> Send(IMGRequest request)
        {
            IMGResult result = request.GetResultInstance();

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
                result = (IMGResult)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, Type.GetType(result.GetType()));
                result.IsSuccess = ((result.Code == API.CODE_SUCCESS) && (response.StatusCode == System.Net.HttpStatusCode.OK));
            } 
            catch (Exception e)
            {
                // Indicate fail and reason
                result.IsSuccess = false;
                result.Message = e.Message;
            }

            return result;
        }

        /// <summary>
        /// Make a deposit request
        /// </summary>
        /// <param name="requestPayload">Deposit request payload</param>
        /// <returns>Task<MGDepositResult> containing Zotapay API response</returns>
        public async Task<MGDepositResult> InitDeposit(MGDepositRequest requestPayload)
        {
            var result = await Send(requestPayload);
            return (MGDepositResult)result;
        }

        private string ValidateMGClient() // TODO
        {
            return null;
        }
    }
}
