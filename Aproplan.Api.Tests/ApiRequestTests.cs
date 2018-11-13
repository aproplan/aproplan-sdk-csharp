using System;
using System.Dynamic;
using System.Net;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model.Actors;
using Aproplan.Api.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Aproplan.Api.Tests
{
    [TestClass]
    public class ApiRequestTests
    {

        [TestMethod]
        public void LoginWithGoodCredentialsReturnsUser()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();

            FakeWebRequest.Instance.Reset();
            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);
            dynamic json = new
            {
                UserInfo = UserUtility.CreateUser("john.smith@aproplan.com", "John Smith"),
                Token = Guid.NewGuid(),
                ValidityStart = DateTime.Now.ToUniversalTime().ToString("o"),
                ValidityLimit = DateTime.Now.ToUniversalTime().AddMinutes(10).ToString("o")
            };
            string content = JsonConvert.SerializeObject(json);

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "POST").Verifiable();
            mockWebRequest.SetupSet(r => r.ContentType = "application/json").Verifiable();

            User user = request.Login("john.smith@aproplan.com", "aproplan").GetAwaiter().GetResult();
            UriBuilder uriBuilder = new UriBuilder("https://api.aproplan.com/rest/simpleloginsecure");

            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "simpleloginsecure", request.ApiVersion, request.RequesterId);
            dynamic requestData = AproplanApiUtility.GetRequestData<ExpandoObject>(mockWebRequest);

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(json.UserInfo.Id, user.Id);
            Assert.AreEqual("john.smith@aproplan.com", requestData.alias);
            Assert.AreEqual("aproplan", requestData.pass);
            mockWebRequest.Verify();
        }

        [TestMethod]        
        public void LoginWithBadCredentialsReturnsUser()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();

            FakeWebRequest.Instance.Reset();
            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse("");
            mockWebRequest.SetupSet(r => r.Method = "POST").Verifiable();
            mockWebRequest.SetupSet(r => r.ContentType = "application/json").Verifiable();

            try
            {
                User user = request.Login("john.smith@aproplan.com", "aproplan").GetAwaiter().GetResult();
                Assert.Fail("No exception thrown");
            }
            catch (Exception ex)
            {
                ApiException apiException = ex as ApiException;
                if (apiException != null)
                {
                    Assert.AreEqual("Your login or password is not correct", apiException.Message);
                    Assert.AreEqual("INVALID_CREDENTIALS", apiException.Code);
                    Assert.AreEqual(401, apiException.ReturnCode);
                }
                else
                    Assert.Fail("Wrong exception thrown");
            }
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoginWithNoCredentials()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();

            request.Login().GetAwaiter().GetResult();
        }
    }
}
