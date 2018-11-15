using Aproplan.Api.Http;
using Aproplan.Api.Model.Actors;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities
{
    public static class AproplanApiUtility
    {
        public static readonly Guid REQUESTER_ID = Guid.NewGuid();
        public static readonly string ROOT_URL = "https://api.aproplan.com/";
        public static readonly string API_VERSION = "13";

        public static ApiRequest CreateRequester(Guid? requesterId = null, string version = null, string rootUrl = null)
        {
            if (!requesterId.HasValue)
                requesterId = REQUESTER_ID;
            if (!string.IsNullOrEmpty(rootUrl))
                rootUrl = ROOT_URL;
            if (string.IsNullOrEmpty(version))
                version = API_VERSION;
            ApiRequest request = new ApiRequest(null, null, requesterId.Value, version, rootUrl);
            return request;
        }

        public static string BuildRestUrl(string restRoot, string resourceName, string v, Guid requesterId, Guid? token = null)
        {
            string url = string.Format("{0}{1}?v={2}&requesterid={3}&dateformat=iso", restRoot, resourceName, v, requesterId.ToString());
            if (token.HasValue)
                url += "&t=" + token.ToString();
            return url;
        }

        public static T GetRequestData<T>(Mock<HttpWebRequest> webRequest)
        {
            string jsonData = null;
            using (MemoryStream copy = new MemoryStream(((MemoryStream)webRequest.Object.GetRequestStream()).ToArray()))
            {
                copy.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(copy))
                {
                    jsonData = streamReader.ReadToEnd();
                }
            }
            return JsonConvert.DeserializeObject<T>(jsonData);            
        }

        public static User FakeLogin(ApiRequest request, DateTime? validityStart = null)
        {
            if (!validityStart.HasValue)
                validityStart = DateTime.Now;
            FakeWebRequest.Instance.Reset();
            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);
            dynamic json = new
            {
                UserInfo = UserUtility.CreateUser("john.smith@aproplan.com", "John Smith"),
                Token = Guid.NewGuid(),
                ValidityStart = validityStart.Value.ToUniversalTime().ToString("o"),
                ValidityLimit = validityStart.Value.ToUniversalTime().AddMinutes(10).ToString("o")
            };
            string content = JsonConvert.SerializeObject(json);

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "POST").Verifiable();
            mockWebRequest.SetupSet(r => r.ContentType = "application/json").Verifiable();

            return request.Login("john.smith@aproplan.com", "aproplan").GetAwaiter().GetResult();
        }

    }
}
