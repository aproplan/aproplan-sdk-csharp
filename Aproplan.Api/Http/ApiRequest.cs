using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Actors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aproplan.Api.Http
{
    public enum ApiMethod
    {
        Get,
        Put,
        Delete,
        Post
    }
    public enum GetEntityResourceType
    {
        List,
        Ids, 
        Count
    }
    public class ApiRequest
    {
        #region Properties

        public string ApiVersion { get; } = "13";

        public string UserLogin { get; }

        public string Password { get; }

        public User CurrentUser { get; private set; }

        public TokenInfo TokenInfo { get; private set; }

        public Guid RequesterId { get; private set; } 

        public string ApiRootUrl { get; }

        #endregion

        #region Methods

        public async Task Login()
        {
            dynamic loginInfo = new
            {
                alias = UserLogin,
                pass = Password
            };
            //loginInfo.alias = UserLogin;
            //loginInfo.pass = Password;

            string data = JsonConvert.SerializeObject(loginInfo);

            string res = await Request(ApiRootUrl + "simpleloginsecure", ApiMethod.Post, null, data);
            JObject jsonLogin = (JObject) JsonConvert.DeserializeObject(res);
            CurrentUser = JsonConvert.DeserializeObject<User>(jsonLogin["UserInfo"].ToString());
            TokenInfo = new TokenInfo();
            TokenInfo.Token = Guid.Parse(jsonLogin["Token"].Value<string>());
            TokenInfo.ValidityStart = jsonLogin["ValidityStart"].Value<DateTime>();
            TokenInfo.ValidityLimit = jsonLogin["ValidityLimit"].Value<DateTime>();
            
        }

        public async Task<List<T>> GetEntityList<T>(Filter filter = null, PathToLoad pathToLoad = null) where T: Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.List);
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (filter != null)
                queryParams.Add("filter", Uri.EscapeDataString(filter.ToString()));
            if (pathToLoad != null)
                queryParams.Add("pathtoload", Uri.EscapeDataString(pathToLoad.ToString()));
            string res = await Request(ApiRootUrl + resourceName, ApiMethod.Get, queryParams);
            var entityList = JsonConvert.DeserializeObject<List<T>>(res);
            return entityList;
        }

        public async Task<T> GetEntityById<T>(Guid id, PathToLoad pathToLoad = null) where T: Entity
        {
            List<T> entities = await GetEntityList<T>(Filter.Eq("Id", id), pathToLoad);
                        
            if (entities.Count == 1)
                return entities[0];
            return null;
        }

        private string GetEntityResourceName<T>(GetEntityResourceType type) where T: Entity
        {
            string resourceName = typeof(T).Name.ToLowerInvariant();
            int len = resourceName.Length;

            switch (type)
            {
                case GetEntityResourceType.List:
                    if(resourceName[len - 1] == 'y')
                        resourceName = resourceName.Substring(0, len - 1) + "ie";
                    else if (resourceName[len - 1] == 's')
                        resourceName = resourceName.Substring(0, len - 1);
                    resourceName += "s";
                    break;
                case GetEntityResourceType.Ids:
                    if (resourceName[len - 1] == 'y')
                        resourceName = resourceName.Substring(0, len - 1) + "ie";
                    else if (resourceName[len - 1] == 's')
                        resourceName = resourceName.Substring(0, len - 1);
                        resourceName += "sids";
                    break;
                case GetEntityResourceType.Count:
                    resourceName += "count";
                    break;
            }
            return resourceName;
        }

        public async Task<string> Request(string uri, ApiMethod method, Dictionary<string, string> queryParams = null, string data = null)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);

            Dictionary<string, string> allQueryParams = BuildDefaultQueryParams();
            if (queryParams != null && queryParams.Count > 0)
                foreach (var keyPair in queryParams)
                    allQueryParams.Add(keyPair.Key, keyPair.Value);

            uriBuilder.Query = new FormUrlEncodedContent(allQueryParams).ReadAsStringAsync().Result;

            
            WebRequest request = WebRequest.Create(uriBuilder.Uri);
            request.Method = method.ToString().ToUpperInvariant();

            Stream stream;
            //stream = request.GetRequestStream();
            if (!String.IsNullOrEmpty(data))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                using (stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }
            }
            try
            {
                WebResponse response = await request.GetResponseAsync();
                using (stream = response.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string dataString = streamReader.ReadToEnd();
                    return dataString;
                }
            }
            catch (WebException ex)
            {
                throw;
            }
        }

        private Dictionary<string, string> BuildDefaultQueryParams()
        {
            var dic = new Dictionary<string, string>
            {
                { "v", ApiVersion },
                { "requesterid", RequesterId.ToString()},
                { "dateformat", "iso" }
            };
            if (TokenInfo != null)
                dic.Add("t", TokenInfo.Token.ToString());
            return dic;

        }

        #endregion

        #region Constructors

        public ApiRequest(string login, string password, Guid requesterId, string apiVersion = "13", string rootUrl = "https://app.aproplan.com")
        {
            UserLogin = login;
            Password = password;
            RequesterId = requesterId;
            ApiVersion = apiVersion;

            ApiRootUrl = rootUrl[rootUrl.Length - 1] == '/' ? rootUrl + "rest/": rootUrl + "/rest/";
        }

        #endregion

        #region Private members

        #endregion
    }
}
