using System;
using System.Collections.Generic;
using System.Net;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model.IdentificationFiles;
using Aproplan.Api.Model.List;
using Aproplan.Api.Tests.Utilities;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Aproplan.Api.Tests
{
    [TestFixture]
    public class GetEntitiesTests
    {
        [TestCase]
        public void GetEntitiesWithoutLoginOK()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();

            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);

            List<Country> fakeCountries = new List<Country>
            {
                new Country { Iso = "BEL", Iso2 = "BE", Name = "Belgium" },
                new Country { Iso = "ITA", Iso2 = "IT", Name = "Italy" }
            };

            string content = JsonConvert.SerializeObject(fakeCountries, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();            

            List<Country> countries = request.GetEntityList<Country>().GetAwaiter().GetResult();


            UriBuilder uriBuilder = new UriBuilder("https://api.aproplan.com/rest/countries");
            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "countries", request.ApiVersion, request.RequesterId);
            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(2, countries.Count);
            Assert.AreEqual(fakeCountries[0].Id, countries[0].Id);
            Assert.AreEqual(fakeCountries[1].Id, countries[1].Id);
            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntitiesWithoutLoginNOK()
        {
            ApiRequest request = AproplanApiUtility.CreateRequester();

            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);
            string content = "";

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            try
            {
                request.GetEntityList<Meeting>().GetAwaiter().GetResult();
                Assert.Fail("An exception should occur");
            }
            catch(Exception ex)
            {
                ApiException apiException = ex as ApiException;
                if(apiException != null)
                {
                    Assert.AreEqual("Cannot call API without to be connected", apiException.Message);
                    Assert.AreEqual("NOT_CONNECTED", apiException.Code);
                }
                else
                {
                    Assert.Fail("Wrong exception thrown", ex);
                }
                
            }

            
        }
    }


}
