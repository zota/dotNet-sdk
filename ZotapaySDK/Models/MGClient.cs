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
        /// <param name="client">Http client will be set as static, default is System.Net.Http.HttpClient</param>
        public MGClient(string merchantId, string merchantSecret, string endpointId, string requestUrl, HttpClient client = null)
        {
            this.merchantId = merchantId;
            this.merchantSecret = merchantSecret;
            this.endpoint = endpointId;
            this.requestUrl = requestUrl;
            MGClient.http = client ?? new HttpClient();
        }

        /// <summary>
        /// Constructor for MGClient with environment variables set
        /// </summary>
        /// <param name="useConstantUrl">Indicate wether to grab the url from constants, default is false</param>
        /// <param name="environment">Environment to be used, if constant url is true, can be Sandbox or Live, default is Live</param>
        /// <param name="client">Http client will be set as static, default is System.Net.Http.HttpClient</param>
        public MGClient(bool useConstantUrl = false, MGEnvironment environment = MGEnvironment.Live, HttpClient client = null)
        {
            this.merchantId = Environment.GetEnvironmentVariable(ENV.MERCHANT_ID);
            this.merchantSecret = Environment.GetEnvironmentVariable(ENV.MERCHANT_SECRET_KEY);
            this.endpoint = Environment.GetEnvironmentVariable(ENV.ENDPOINT_ID);
            this.requestUrl = Environment.GetEnvironmentVariable(ENV.REQUEST_URL);
            if (useConstantUrl)
            {
                this.requestUrl = (environment == MGEnvironment.Live) ? URL.LIVE : URL.SANDBOX;
            }

            MGClient.http = client ?? new HttpClient();
        }

        /// <summary>
        /// Requests deposit to Zotapay API or returns reason of failure
        /// </summary>
        /// <param name="depositRequest">Object containing data required for deposit</param>
        /// <returns>Task with the request results</returns>
        public async Task<MGResult> InitDeposit(MGDepositRequest depositRequest)
        {
            MGResult mGResponse = new MGResult();

            // Validate deposit request
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(depositRequest, new ValidationContext(depositRequest), validationResults, validateAllProperties: true))
            {
                // Indicate failed validation and populate error message envelope
                mGResponse.IsSuccess = false;
                mGResponse.ErrorList = string.Join(" | ", validationResults);
                // return mGResponse; todo
            }
            depositRequest.generateDepositSignature(this.endpoint, this.merchantSecret);

            string requestUrl = this.getRequestURL(this.requestUrl, URL.PATH_DEPOSIT);
            try
            {
                // Create json payload string and wrap it as http content
                string payload = JsonConvert.SerializeObject(depositRequest);
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
                http.DefaultRequestHeaders.Add("Content-Type", "application/json");
                
                // request
                var response = await http.PostAsync(requestUrl, httpContent);

            } 
            catch (Exception e)
            {
                // Indicate fail and reason
                mGResponse.IsSuccess = false;
                mGResponse.ErrorList = e.Message;
            }


            return mGResponse;
        }

        /// <summary>
        /// Gets the full request url
        /// </summary>
        /// <param name="baseUrl">Base url of the domain</param>
        /// <param name="path">Full request path</param>
        /// <returns>The full request url</returns>
        private string getRequestURL(string baseUrl, string path)
        {
            string urlPath = string.Format(path, this.endpoint);
            return this.requestUrl + urlPath;
        }

        private string generateJSONPayload(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
