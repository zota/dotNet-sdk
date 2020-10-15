using System;
using System.Collections.Generic;
using System.Text;

namespace ZotapaySDK.Contracts
{
    public interface IMGRequest
    {
        public void GenerateSignature(string endpointId, string secret);

        public string GetRequestUrl(string baseUrl, string endpoint);

        internal IMGResult GetResultInstance();
    };
}
