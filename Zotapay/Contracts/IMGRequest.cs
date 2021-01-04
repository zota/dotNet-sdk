namespace Zotapay.Contracts
{
    public interface IMGRequest
    {
        public void GenerateSignature(string endpointId, string secret);

        public string GetRequestUrl(string baseUrl, string endpoint);

        public IMGResult GetResultInstance();

        internal void SetupPrivateMembers(string merchantId);

        internal System.Net.Http.HttpMethod GetMethod();
    };
}
