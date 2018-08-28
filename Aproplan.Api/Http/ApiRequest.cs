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
    public class ApiRequest: IDisposable
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

        /// <summary>
        /// To login to aproplan with credentials supplied in the constructor. 
        /// When the connection succededd, the CurrentUser property and TokenInfo will contains info about the connection
        /// </summary>
        /// <returns>The user connected</returns>
        public async Task<User> Login()
        {
            dynamic loginInfo = new
            {
                alias = UserLogin,
                pass = Password
            };

            string data = JsonConvert.SerializeObject(loginInfo);

            string res = await Request(ApiRootUrl + "simpleloginsecure", ApiMethod.Post, null, data);
            JObject jsonLogin = (JObject) JsonConvert.DeserializeObject(res);
            CurrentUser = JsonConvert.DeserializeObject<User>(jsonLogin["UserInfo"].ToString());
            TokenInfo = new TokenInfo();
            TokenInfo.Token = Guid.Parse(jsonLogin["Token"].Value<string>());
            TokenInfo.ValidityStart = jsonLogin["ValidityStart"].Value<DateTime>();
            TokenInfo.ValidityLimit = jsonLogin["ValidityLimit"].Value<DateTime>();
            return CurrentUser;
            
        }

        /// <summary>
        /// To renew the token to use for all api call. The renew must be done before the invalid datetime otherwise, use the login method
        /// </summary>
        /// <returns>The new token with validity time range</returns>
        public async Task<TokenInfo> RenewToken()
        {
            dynamic loginInfo = new
            {
                alias = UserLogin,
                pass = Password
            };
            string data = JsonConvert.SerializeObject(loginInfo);

            string res = await Request(ApiRootUrl + "renewtoken", ApiMethod.Get, null, null);
            TokenInfo = JsonConvert.DeserializeObject<TokenInfo>(res);
            return TokenInfo;

        }

        /// <summary>
        /// To retrieve the list of an entity into APROPLAN depending of a filter, pathtoload and sortorder
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="filter">The filter to apply</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The list of entities corresponding to criteria</returns>
        public async Task<List<T>> GetEntityList<T>(Filter filter = null, PathToLoad pathToLoad = null) where T: Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.List);
            string res = await GetRaw(resourceName, filter, pathToLoad);
            var entityList = JsonConvert.DeserializeObject<List<T>>(res);
            return entityList;
        }

        /// <summary>
        /// To retrieve an entity by its id
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="id">The id of the entity to retrieve</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The entity corresponding to the id</returns>
        public async Task<T> GetEntityById<T>(Guid id, PathToLoad pathToLoad = null) where T: Entity
        {
            List<T> entities = await GetEntityList<T>(Filter.Eq("Id", id), pathToLoad);
                        
            if (entities.Count == 1)
                return entities[0];
            return null;
        }

        /// <summary>
        /// To retrieve a list of entities corresponding to a list of ids
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="ids">The list of id to retrieve of the specific entity type</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The list of entities corresponding to the list of id</returns>
        public async Task<List<T>> GetEntityByIds<T>(Guid[] ids, PathToLoad pathToLoad = null) where T: Entity
        {
            if (ids.Length == 1)
            {
                T entity = await GetEntityById<T>(ids[0], pathToLoad);
                return new List<T>() { entity };
            }
            object[] idsObj = new object[ids.Length];
            int i = 0;
            foreach(Guid id in ids)
            {
                idsObj[i++] = id;
            }
            List<T> entities = await GetEntityList<T>(Filter.In("Id", idsObj));
            return entities;
        }

        /// <summary>
        /// To retrieve the list of id of a type of entity into APROPLAN depending of a filter, pathtoload and sortorder
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="filter">The filter to apply</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The list of ids corresponding to criteria</returns>
        public async Task<List<Guid>> GetEntityIds<T>(Filter filter = null, PathToLoad pathToLoad = null) where T: Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.Ids);
            string res = await GetRaw(resourceName, filter, pathToLoad);
            var ids = JsonConvert.DeserializeObject<List<Guid>>(res);
            return ids;
        }

        /// <summary>
        /// Retrieve the count of entities corresponding to the criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<int> GetEntityCount<T>(Filter filter = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.Count);
            string res = await GetRaw(resourceName, filter, null);
            return int.Parse(res);
        }

        /// <summary>
        /// Call a resource of the APROPLAN API with the GET method
        /// </summary>
        /// <param name="resourceName">The resource name to get</param>
        /// <param name="filter">The filter to apply to the get</param>
        /// <param name="pathToLoad">The linked property to load in the same time</param>
        /// <returns></returns>
        public async Task<string> GetRaw(string resourceName, Filter filter, PathToLoad pathToLoad)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (filter != null)
                queryParams.Add("filter", filter.ToString());
            if (pathToLoad != null)
                queryParams.Add("pathtoload", pathToLoad.ToString());
            return await Request(ApiRootUrl + resourceName, ApiMethod.Get, queryParams);
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

        /// <summary>
        /// To make a call to the APROPLAN api
        /// </summary>
        /// <param name="uri">The url to call</param>
        /// <param name="method">The method to use to make the call</param>
        /// <param name="queryParams">The query params collection to use</param>
        /// <param name="data">The data to send through the request</param>
        /// <param name="isFile">To know if the data is a file path or not</param>
        /// <returns>The response of the request as a string</returns>
        public async Task<string> Request(string uri, ApiMethod method, Dictionary<string, string> queryParams = null, string data = null, bool isFile = false)
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
                if (!isFile)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(data);
                    request.ContentType = "application/json";
                    request.ContentLength = byteArray.Length;
                    using (stream = request.GetRequestStream())
                    {
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                else
                {
                    using (FileStream fileStream = File.Open(data, FileMode.Open, FileAccess.Read))
                    {

                        request.ContentType = "application/" + Path.GetExtension(data).Substring(1);
                        request.ContentLength = fileStream.Length;
                        fileStream.CopyTo(request.GetRequestStream());
                    }
                }
            }
            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (stream = response.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string dataString = streamReader.ReadToEnd();
                        return dataString;
                    }
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

        public void Dispose()
        {
            throw new NotImplementedException();
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
