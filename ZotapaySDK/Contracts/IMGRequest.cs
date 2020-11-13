﻿namespace ZotapaySDK.Contracts
{
    public interface IMGRequest
    {
        public void GenerateSignature(string endpointId, string secret);

        public string GetRequestUrl(string baseUrl, string endpoint);

        public IMGResult GetResultInstance();

        internal virtual void SetupPrivateMembers(string merchantId) { }
    };
}
