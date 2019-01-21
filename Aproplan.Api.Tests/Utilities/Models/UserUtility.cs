using Aproplan.Api.Http;
using Aproplan.Api.Model.Actors;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities
{
    public static class UserUtility
    {
        public static User CreateUser(string alias, string displayName, string languageCode = "en")
        {
            return new User
            {
                DisplayName = displayName,
                Alias = alias,
                DefaultEmail = alias,
                Id = Guid.NewGuid(),
                LanguageCode = languageCode,
                Person = new Person
                {
                    Name = displayName,
                    LanguageCode = languageCode,
                    Id = Guid.NewGuid(),
                    Code = displayName
                },
            };
        }

        public static User MakeLogin(ApiRequest request)
        {
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

            FakeWebRequest.Instance.Reset();
            return user;
        }
    }
}
