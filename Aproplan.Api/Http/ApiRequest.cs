using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        Count,
        Sync
    }

    public enum RequestLoginState
    {
        NotConnected,
        Connected,
        Renewing,
        Connecting
    }

    public class ApiRequest: IDisposable
    {
        public event EventHandler<User> UserChanged;
        #region Properties

        public string ApiVersion { get; } = "13";

        public string UserLogin { get; private set; }

        public string Password { get; private set; }

        public User CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    UserChanged?.Invoke(this, _currentUser);
                }
            }
        }

        public TokenInfo TokenInfo { get; private set; }

        public Guid RequesterId { get; private set; }

        public string ApiRootUrl { get; }

        public RequestLoginState RequestLoginState
        {
            get
            {
                if (CurrentUser == null)
                {
                    if (!_isRenewingToken && !_isConnecting)
                        return RequestLoginState.NotConnected;
                    if (_isConnecting)
                        return RequestLoginState.Connecting;
                }
                else if (CurrentUser != null)
                {
                    if (_isRenewingToken) return RequestLoginState.Renewing;
                    if (_isConnecting) return RequestLoginState.Connecting;
                    return RequestLoginState.Connected;
                }
                return RequestLoginState.NotConnected;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// To login to aproplan with credentials supplied in the constructor. 
        /// When the connection succededd, the CurrentUser property and TokenInfo will contains info about the connection
        /// </summary>
        /// <returns>The user connected</returns>
        public async Task<User> Login()
        {
            if (string.IsNullOrEmpty(UserLogin)) throw new ArgumentNullException("UserLogin");
            if (string.IsNullOrEmpty(Password)) throw new ArgumentNullException("Password");
            dynamic loginInfo = new
            {
                alias = UserLogin,
                pass = Password
            };

            User user = null;
            string url = ApiRootUrl + "simpleloginsecure";
            try
            {
                _isConnecting = true;
                string data = JsonConvert.SerializeObject(loginInfo);

                string res = (await Request(url, ApiMethod.Post, null, data)).Data;
                if (String.IsNullOrEmpty(res))
                {
                    throw new ApiException("Your login or password is not correct", "INVALID_CREDENTIALS", null, url, 401, "POST");
                }
                JObject jsonLogin = (JObject)JsonConvert.DeserializeObject(res, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
                TokenInfo = new TokenInfo();
                TokenInfo.Token = Guid.Parse(jsonLogin["Token"].Value<string>());
                TokenInfo.ValidityStart = jsonLogin["ValidityStart"].Value<DateTime>();
                TokenInfo.ValidityLimit = jsonLogin["ValidityLimit"].Value<DateTime>();


                user = JsonConvert.DeserializeObject<User>(jsonLogin["UserInfo"].ToString(), new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
                TimeSpan tokenValidityPeriod = TokenInfo.ValidityLimit.ToUniversalTime().Subtract(DateTime.Now.ToUniversalTime());
                RenewTokenLoop(Convert.ToInt32(tokenValidityPeriod.TotalMilliseconds - 60000 * 2));
            }
            finally
            {
                _isConnecting = false;
                CurrentUser = user;
            }

            return CurrentUser;
        }

        public async Task<User> Login(string login, string password)
        {
            UserLogin = login;
            Password = password;
            return await Login();
        }

        public void Logout()
        {
            if (_renewTimer != null)
                _renewTimer.Dispose();
            CurrentUser = null;
            TokenInfo = null;
        }

        private void RenewTokenLoop(int msDuration)
        {
            if (_renewTimer != null)
                _renewTimer.Dispose();
            _renewTimer = new Timer(async (obj) =>
            {
                await RenewToken();
            }, null, msDuration, Timeout.Infinite);
        }

        /// <summary>
        /// To renew the token to use for all api call. The renew must be done before the invalid datetime otherwise, use the login method
        /// </summary>
        /// <returns>The new token with validity time range</returns>
        public async Task<TokenInfo> RenewToken()
        {
            try
            {
                Console.WriteLine("Renew token call...");
                string res = (await Request(ApiRootUrl + "renewtoken", ApiMethod.Get, null, null)).Data;
                TokenInfo = JsonConvert.DeserializeObject<TokenInfo>(res, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
                Console.WriteLine("Renew token :" + TokenInfo.ValidityLimit.ToShortTimeString());
                TimeSpan tokenValidityPeriod = TokenInfo.ValidityLimit.ToUniversalTime().Subtract(DateTime.Now.ToUniversalTime());
                RenewTokenLoop(Convert.ToInt32(tokenValidityPeriod.TotalMilliseconds - 60000 * 2));
            }
            catch (Exception)
            {
                TokenInfo = null;
            }
            finally
            {
                _isRenewingToken = false;
            }


            return TokenInfo;

        }

        /// <summary>
        /// To retrieve the list of an entity into APROPLAN depending of a filter, pathtoload and sortorder
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="filter">The filter to apply</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The list of entities corresponding to criteria</returns>
        public async Task<List<T>> GetEntityList<T>(Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.List);
            string res = (await GetRaw(resourceName, filter, pathToLoad, projectId)).Data;
            var entityList = JsonConvert.DeserializeObject<List<T>>(res, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            return entityList;
        }

        /// <summary>
        /// To retrieve an entity by its id
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="id">The id of the entity to retrieve</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The entity corresponding to the id</returns>
        public async Task<T> GetEntityById<T>(Guid id, Guid? projectId = null, PathToLoad pathToLoad = null) where T : Entity
        {
            List<T> entities = await GetEntityList<T>(projectId, Filter.Eq("Id", id), pathToLoad);

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
        public async Task<List<T>> GetEntityByIds<T>(Guid[] ids, Guid? projectId = null, PathToLoad pathToLoad = null) where T : Entity
        {
            if (ids.Length == 1)
            {
                T entity = await GetEntityById<T>(ids[0], projectId, pathToLoad);
                return new List<T>() { entity };
            }
            object[] idsObj = new object[ids.Length];
            int i = 0;
            foreach (Guid id in ids)
            {
                idsObj[i++] = id;
            }
            List<T> entities = await GetEntityList<T>(projectId, Filter.In("Id", idsObj));
            return entities;
        }

        /// <summary>
        /// To retrieve the list of id of a type of entity into APROPLAN depending of a filter, pathtoload and sortorder
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve</typeparam>
        /// <param name="filter">The filter to apply</param>
        /// <param name="pathToLoad">To know which property to load from the entity</param>
        /// <returns>The list of ids corresponding to criteria</returns>
        public async Task<List<Guid>> GetEntityIds<T>(Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.Ids);
            string res = (await GetRaw(resourceName, filter, pathToLoad, projectId)).Data;
            var ids = JsonConvert.DeserializeObject<List<Guid>>(res, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            return ids;
        }

        /// <summary>
        /// Retrieve the count of entities corresponding to the criteria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<int> GetEntityCount<T>(Guid? projectId = null, Filter filter = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.Count);
            string res = (await GetRaw(resourceName, filter, null, projectId)).Data;
            return int.Parse(res);
        }

        /// <summary>
        /// To create new user in APROPLAN. You need to be connected to create a user. It must be used for invitation
        /// </summary>
        /// <param name="newUser">The user to create</param>
        /// <param name="password">The password of the new user to apply</param>
        /// <param name="queryParams">Parameters to send in the create user call</param>
        /// <returns>The new user created</returns>
        public async Task<User> CreateUser(User newUser, string password, Dictionary<string, string> queryParams)
        {
            // EDIT: This method is private, since it cannot work without a captcha response
            string url = ApiRootUrl + GetEntityResourceName<User>(GetEntityResourceType.List);
            dynamic data = new
            {
                user = newUser,
                pass = password
            };

            HttpResponse response = await Request(url, ApiMethod.Post, queryParams, JsonConvert.SerializeObject(data));
            return JsonConvert.DeserializeObject<User>(response.Data, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
        }

        /// <summary>
        /// To create a batch of a same kind of entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to create</typeparam>
        /// <param name="entities">The list of entities to create/push in APROPLAN</param>
        /// <returns>The list of entities created</returns>
        public async Task<T[]> CreateEntities<T>(T[] entities, Guid? projectId = null, Dictionary<string, string> queryParams = null) where T : Entity
        {
            return await CreateOrUpdateEntities<T>(entities, true, projectId, queryParams);
        }

        /// <summary>
        /// To create new entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to create</typeparam>
        /// <param name="entity">The entity to create</param>
        /// <returns>The entity created</returns>
        public async Task<T> CreateEntity<T>(T entity, Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null) where T : Entity
        {
            T[] entities = null;
            if (entity != null)
            {
                entities = new[] { entity };
            }


            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (filter != null)
                queryParams.Add("filter", filter.ToString());
            if (pathToLoad != null)
                queryParams.Add("pathtoload", pathToLoad.ToString());

            T[] newEntities = await CreateEntities<T>(entities, projectId, queryParams);
            return newEntities.Length == 0 ? null : newEntities[0];
        }

        /// <summary>
        /// To update a batch of a same kind of entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to update</typeparam>
        /// <param name="entities">The list of entities to update in APROPLAN</param>
        /// <returns>The list of entities updated</returns>
        public async Task<T[]> UpdateEntities<T>(T[] entities, Guid? projectId = null) where T : Entity
        {

            return await CreateOrUpdateEntities<T>(entities, false, projectId);
        }


        /// <summary>
        /// To update new entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to update</typeparam>
        /// <param name="entity">The entity to update</param>
        /// <returns>The entity updated</returns>
        public async Task<T> UpdateEntity<T>(T entity, Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null) where T : Entity
        {
            T[] entities = null;
            if (entity != null)
            {
                entities = new[] { entity };
            }


            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (filter != null)
                queryParams.Add("filter", filter.ToString());
            if (pathToLoad != null)
                queryParams.Add("pathtoload", pathToLoad.ToString());

            T[] newEntities = await UpdateEntities<T>(entities, projectId);
            return newEntities.Length == 0 ? null : newEntities[0];
        }

        /// <summary>
        /// To delete a list of entities by its ids
        /// </summary>
        /// <typeparam name="T">The type of entity to delete</typeparam>
        /// <param name="ids">The list of id of the entities to delete</param>
        /// <returns>A boolean to specify that the delete was successfull</returns>
        public async Task<bool> DeleteEntities<T>(IEnumerable<Guid> ids, Guid? projectId = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.List);
            string url = ApiRootUrl + resourceName;
            IDictionary<String, String> queryParams = new Dictionary<String, String>();
            if (projectId.HasValue)
                queryParams.Add("projectid", projectId.Value.ToString());
            await Request(url, ApiMethod.Delete, queryParams, JsonConvert.SerializeObject(ids, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            }));
            return true;
        }

        /// <summary>
        /// To delete an entity by its id
        /// </summary>
        /// <typeparam name="T">The type of entity to delete</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns>A boolean to specify that the delete was successfull</returns>
        public async Task<bool> DeleteEntity<T>(Guid id, Guid? projectId = null) where T : Entity
        {
            return await DeleteEntities<T>(new[] { id }, projectId);
        }

        public async Task<SyncResult<Note>> SyncNotes(Guid projectId, String continuationToken) { return await this.SyncEntity<Note>(continuationToken, projectId); }
        public async Task<SyncResult<Form>> SyncForms(Guid projectId, String continuationToken) { return await this.SyncEntity<Form>(continuationToken, projectId); }
        public async Task<SyncResult<FormTemplate>> SyncFormTemplates(Guid projectId, String continuationToken) { return await this.SyncEntity<FormTemplate>(continuationToken, projectId); }
        public async Task<SyncResult<IssueType>> SyncIssueTypes(Guid projectId, String continuationToken) { return await this.SyncEntity<IssueType>(continuationToken, projectId); }
        public async Task<SyncResult<Chapter>> SyncChapters(Guid projectId, String continuationToken) { return await this.SyncEntity<Chapter>(continuationToken, projectId); }
        public async Task<SyncResult<SubCell>> SyncSubCells(Guid projectId, String continuationToken) { return await this.SyncEntity<SubCell>(continuationToken, projectId); }
        public async Task<SyncResult<ParentCell>> SyncParentCells(Guid projectId, String continuationToken) { return await this.SyncEntity<ParentCell>(continuationToken, projectId); }
        public async Task<SyncResult<Folder>> SyncFolders(Guid projectId, String continuationToken) { return await this.SyncEntity<Folder>(continuationToken, projectId); }
        public async Task<SyncResult<Project>> SyncProjects(String continuationToken) { return await this.SyncEntity<Project>(continuationToken); }
        public async Task<SyncResult<ContactDetails>> SyncContactDetails(Guid projectId, String continuationToken) { return await this.SyncEntity<ContactDetails>(continuationToken, projectId); }
        public async Task<SyncResult<NoteProjectStatus>> SyncProjectStatus(Guid projectId, String continuationToken) { return await this.SyncEntity<NoteProjectStatus>(continuationToken, projectId); }
        public async Task<SyncResult<User>> SyncUsers(Guid projectId, String continuationToken) { return await this.SyncEntity<User>(continuationToken, projectId); }
        public async Task<SyncResult<Meeting>> SyncMeetings(Guid projectId, String continuationToken) { return await this.SyncEntity<Meeting>(continuationToken, projectId); }
        public async Task<SyncResult<Document>> SyncAttachmentDocuments(Guid projectId, String continuationToken) { return await this.SyncEntity<Document>(continuationToken, projectId, "attachmentdocumentsync"); }
        public async Task<SyncResult<Document>> SyncFolderDocuments(Guid projectId, String continuationToken) { return await this.SyncEntity<Document>(continuationToken, projectId, "folderdocumentsync"); }

        /// <summary>
        /// To get all the changes of an entity since a specified point in time
        /// </summary>
        /// <typeparam name="T">Kind of entity to get</typeparam>
        /// <param name="syncStamp">This is the stamp from when the sync need to get differences. If not specified, it means to get data from the beginning. 
        /// Else it corresponds to the last sync done. Value returned in the SyncTimestamp property of the response headers</param>
        /// <param name="forcedResourceName">IF the end point to use is not really the name of the entity, you can specify the name of the resource endpoint to use</param>
        /// <returns>A SyncResult containing the data from the syncStamp you specified and until a new SyncStamp specified in ContinuationToken</returns>
        protected async Task<SyncResult<T>> SyncEntity<T>(String syncStamp, Guid? projectId = null, String forcedResourceName = null) where T : Entity
        {
            string resourceName = forcedResourceName;
            if (resourceName == null)
            {
                resourceName = GetEntityResourceName<T>(GetEntityResourceType.Sync);
            }
            IDictionary<String, String> additionalParams = new Dictionary<String, String>();
            additionalParams["timestamp"] = syncStamp;
            HttpResponse res = await GetRaw(resourceName, null, null, projectId, additionalParams);
            var entityList = JsonConvert.DeserializeObject<List<T>>(res.Data, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            return new SyncResult<T>
            {
                Data = entityList,
                ContinuationToken = res.Headers["SyncTimestamp"]
            };
        }

        private async Task<T[]> CreateOrUpdateEntities<T>(T[] entities, bool isCreation, Guid? projectId = null, IDictionary<string, string> queryParams = null) where T : Entity
        {
            string resourceName = GetEntityResourceName<T>(GetEntityResourceType.List);
            string url = ApiRootUrl + resourceName;
            string response = null;
            if (projectId.HasValue)
            {
                if (queryParams == null) queryParams = new Dictionary<string, string>();
                queryParams.Add("projectid", projectId.Value.ToString());
            }
            ApiMethod method = isCreation ? ApiMethod.Post : ApiMethod.Put;
            if (entities != null && entities.Length > 0)
                response = (await Request(url, method, queryParams, JsonConvert.SerializeObject(entities, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                }))).Data;

            T[] resEntities = null;
            if (!String.IsNullOrEmpty(response))
                resEntities = JsonConvert.DeserializeObject<T[]>(response, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            return resEntities;
        }


        /// <summary>
        /// Call a resource of the APROPLAN API with the GET method
        /// </summary>
        /// <param name="resourceName">The resource name to get</param>
        /// <param name="filter">The filter to apply to the get</param>
        /// <param name="pathToLoad">The linked property to load in the same time</param>
        /// <param name="additionalHeaders">Additional headers to add to the HTTP request</param>
        /// <returns></returns>
        public async Task<HttpResponse> GetRaw(string resourceName, Filter filter, PathToLoad pathToLoad, Guid? projectId = null, IDictionary<String, String> additionalParams = null)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (filter != null)
                queryParams.Add("filter", filter.ToString());
            if (pathToLoad != null)
                queryParams.Add("pathtoload", pathToLoad.ToString());
            if (projectId.HasValue)
                queryParams.Add("projectid", projectId.Value.ToString());

            if (additionalParams != null)
            {
                foreach (String key in additionalParams.Keys)
                {
                    queryParams.Add(key, additionalParams[key]);
                }
            }

            return await Request(ApiRootUrl + resourceName, ApiMethod.Get, queryParams);
        }       


        private string GetEntityResourceName<T>(GetEntityResourceType type) where T : Entity
        {
            string resourceName = typeof(T).Name.ToLowerInvariant();
            int len = resourceName.Length;

            switch (type)
            {
                case GetEntityResourceType.List:
                    if (resourceName[len - 1] == 'y')
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
                case GetEntityResourceType.Sync:
                    resourceName += "sync";
                    break;
            }
            return resourceName;
        }

        private bool IsTokenValid()
        {
            bool retVal = TokenInfo != null && DateTime.Now < TokenInfo.ValidityLimit;
            return retVal;
        }


        /// <summary>
        /// To make a call to the APROPLAN api
        /// </summary>
        /// <param name="uri">The url to call</param>
        /// <param name="method">The method to use to make the call</param>
        /// <param name="queryParams">The query params collection to use</param>
        /// <param name="data">The data to send through the request</param>
        /// <param name="isFile">To know if the data is a file path or not</param>
        /// <param name="additionalHeaders">Additional headers to add to the HTTP call</param>
        /// <returns>The response of the request as a string</returns>
        public async Task<HttpResponse> Request(string uri, ApiMethod method, IDictionary<string, string> queryParams = null, string data = null, bool isFile = false)
        {
            int nb = 0;
            if (uri != _resourceRenew)
            {
                if ((RequestLoginState == RequestLoginState.NotConnected || !IsTokenValid()) && !_resourcesWithoutConnection.Contains(new Tuple<ApiMethod, String>(method, uri)))
                    throw new ApiException("Cannot call API without to be connected", "NOT_CONNECTED", null, uri, -1, method.ToString(), null);
            }
            if (!_resourcesLogin.Contains(uri) && uri != _resourceRenew)
            {
                while (RequestLoginState == RequestLoginState.Connecting || RequestLoginState == RequestLoginState.Renewing && nb < 10)
                {
                    Thread.Sleep(300);
                    nb++;
                }
            }
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
                        return new HttpResponse
                        {
                            Data = dataString,
                            Headers = response.Headers
                        };
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string url = ex.Response.ResponseUri.ToString();
                    string resMethod = method.ToString();
                    string errorCode = null;
                    string errorId = null;
                    int statusCode = 0;
                    string message = "An error occured while the api call";
                    HttpWebResponse httpResponse = ex.Response as HttpWebResponse;
                    if (httpResponse != null)
                    {
                        resMethod = httpResponse.Method;
                        statusCode = (int)httpResponse.StatusCode;
                    }
                    using (var streamRes = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string res = streamRes.ReadToEnd();
                        if (ex.Response.ContentType.Contains("application/json"))
                        {
                            JArray json = JsonConvert.DeserializeObject<JArray>(res, new JsonSerializerSettings
                            {
                                DateTimeZoneHandling = DateTimeZoneHandling.Local
                            });
                            JToken item = json.First;
                            IEnumerable<JProperty> properties = item.Children<JProperty>();
                            var element = properties.FirstOrDefault(x => x.Name == "Message");
                            if (element != null)
                                message = element.Value.ToString();

                            element = properties.FirstOrDefault(x => x.Name == "ErrorCode");
                            if (element != null)
                                errorCode = element.Value.ToString();
                            element = properties.FirstOrDefault(x => x.Name == "ErrorGuid");
                            if (element != null)
                                errorId = element.Value.ToString();

                        }
                    }
                    throw new ApiException(message, errorCode, errorId, url, statusCode, resMethod, ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                    throw;
                string url = uri;
                string resMethod = method.ToString();
                throw new ApiException("An error occured", null, null, url, 0, resMethod, ex);
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
            if (_renewTimer != null)
                _renewTimer.Dispose();
        }



        #endregion

        #region Constructors

        public ApiRequest(string login, string password, Guid requesterId, string apiVersion = "13", string rootUrl = "https://app.aproplan.com")
        {
            // Force TLS 1.2
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (requesterId == Guid.Empty) throw new ArgumentNullException("requesterId");
            UserLogin = login;
            Password = password;
            RequesterId = requesterId;
            ApiVersion = apiVersion;
            if (string.IsNullOrEmpty(rootUrl))
                rootUrl = DefaultApiRootUrl;
            if (string.IsNullOrEmpty(apiVersion))
                apiVersion = DefaultApiVersion;

            ApiRootUrl = rootUrl[rootUrl.Length - 1] == '/' ? rootUrl + "rest/" : rootUrl + "/rest/";
            _resourcesWithoutConnection = new List<Tuple<ApiMethod, string>>
            {
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "countries"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "countriesids"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "countrycount"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "professions"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "professionsids"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "professioncount"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "languages"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "languagesids"),
                new Tuple<ApiMethod, String>(ApiMethod.Get, ApiRootUrl + "languagecount"),
                new Tuple<ApiMethod, String>(ApiMethod.Post, ApiRootUrl + "loginwithfullinfosecure"),
                new Tuple<ApiMethod, String>(ApiMethod.Post, ApiRootUrl + "loginsecure"),
                new Tuple<ApiMethod, String>(ApiMethod.Post, ApiRootUrl + "simpleloginsecure"),
                new Tuple<ApiMethod, String>(ApiMethod.Post, ApiRootUrl + "users")
            };
            _resourcesLogin = new List<string>
            {
                ApiRootUrl + "loginwithfullinfosecure",
                ApiRootUrl + "loginsecure",
                ApiRootUrl + "simpleloginsecure"
            };


            _resourceRenew = ApiRootUrl + "renewtoken";
        }

        public ApiRequest(string login, string password, Guid requesterId, string rootUrl = null) : this(login, password, requesterId, DefaultApiVersion, rootUrl)
        {

        }

        public ApiRequest(Guid requesterId, string rootUrl = null) : this(null, null, requesterId, DefaultApiVersion, rootUrl)
        {

        }

        #endregion


        #region Private members

        private User _currentUser;
        Timer _renewTimer;
        bool _isRenewingToken = false;
        bool _isConnecting = false;
        readonly List<Tuple<ApiMethod, string>> _resourcesWithoutConnection;
        readonly List<string> _resourcesLogin;
        readonly string _resourceRenew;

        public static string DefaultApiVersion = "13";
        private static string DefaultApiRootUrl = "https://api.aproplan.com/";

        #endregion
    }
}
