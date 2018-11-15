using System;
using System.Dynamic;
using System.Net;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model.Actors;
using Aproplan.Api.Tests.Utilities;
using NUnit.Framework;
using Moq;
using Newtonsoft.Json;

namespace Aproplan.Api.Tests
{    
    [TestFixture]
    public class LoginTests
    {

        [TestCase]
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
            Assert.AreEqual(json.UserInfo.Id, request.CurrentUser.Id);
            Assert.AreEqual(DateTime.Parse(json.ValidityStart).ToLocalTime(), request.TokenInfo.ValidityStart);
            Assert.AreEqual(DateTime.Parse(json.ValidityLimit).ToLocalTime(), request.TokenInfo.ValidityLimit);

            mockWebRequest.Verify();
        }

        [TestCase]        
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

        [TestCase]
        public void LoginWithNoCredentials()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                request.Login().GetAwaiter().GetResult();
            });
            Assert.AreEqual("UserLogin", ex.ParamName);
            
        }

        [TestCase]
        public void LoginWithNoPasswordCredentials()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();
            
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                request.Login("john.smith@aproplan.com", null).GetAwaiter().GetResult();
            });
            Assert.AreEqual("Password", ex.ParamName);

        }

        [TestCase]
        public void Logout()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();
            AproplanApiUtility.FakeLogin(request);

            request.Logout();

            Assert.IsNull(request.CurrentUser);
            Assert.IsNull(request.TokenInfo);
        }

        [TestCase]
        public void RenewTokenValid()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();
            DateTime currentTokenStart = DateTime.Now.AddMinutes(-6);
            AproplanApiUtility.FakeLogin(request, currentTokenStart);
            Guid oldToken = request.TokenInfo.Token;
            dynamic json = new
            {
                Token = Guid.NewGuid(),
                ValidityStart = DateTime.Now.ToUniversalTime().ToString("o"),
                ValidityLimit = DateTime.Now.ToUniversalTime().AddMinutes(10).ToString("o")
            };
            string content = JsonConvert.SerializeObject(json);

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();
            
            var tokenInfo = request.RenewToken().GetAwaiter().GetResult();
            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "renewtoken", request.ApiVersion, request.RequesterId, oldToken);
            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[1].ToString());
            Assert.AreEqual(json.Token, request.TokenInfo.Token);
            Assert.AreEqual(DateTime.Parse(json.ValidityStart).ToLocalTime(), request.TokenInfo.ValidityStart);
            Assert.AreEqual(DateTime.Parse(json.ValidityLimit).ToLocalTime(), request.TokenInfo.ValidityLimit);
        }

        [TestCase]
        public void RenewTokenWhileInvalidPeriod()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();
            DateTime currentTokenStart = DateTime.Now.AddMinutes(-11);
            AproplanApiUtility.FakeLogin(request, currentTokenStart);
            Guid oldToken = request.TokenInfo.Token;
            dynamic json = new
            {
                Token = Guid.NewGuid(),
                ValidityStart = DateTime.Now.ToUniversalTime().ToString("o"),
                ValidityLimit = DateTime.Now.ToUniversalTime().AddMinutes(10).ToString("o")
            };
            string content = JsonConvert.SerializeObject(json);

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            try
            {
                var tokenInfo = request.RenewToken().GetAwaiter().GetResult();
                Assert.Fail("An exception should be thrown");
            }
            catch (Exception ex)
            {
                ApiException apiException = ex as ApiException;
                if(apiException != null)
                {
                    Assert.AreEqual("Your current token is invalid, use login method instead", apiException.Message);
                    Assert.AreEqual("TOKEN_EXPIRED", apiException.Code);
                }
                else
                {
                    Assert.Fail("Wrong exception thrown");
                }
            }
            
        }
    }
}
