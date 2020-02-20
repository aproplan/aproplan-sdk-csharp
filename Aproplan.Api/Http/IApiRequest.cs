using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Actors;

namespace Aproplan.Api.Http
{
    public interface IApiRequest : IDisposable
    {
        string ApiRootUrl { get; }
        string ApiVersion { get; }
        User CurrentUser { get; }
        string Password { get; }
        Guid RequesterId { get; }
        RequestLoginState RequestLoginState { get; }
        TokenInfo TokenInfo { get; }
        string UserLogin { get; }

        event EventHandler<User> UserChanged;

        Task<T[]> CreateEntities<T>(T[] entities, Guid? projectId = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<T> CreateEntity<T>(T entity, Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null) where T : Entity;
        Task<User> CreateUser(User newUser, string password, Dictionary<string, string> queryParams);
        Task<bool> DeleteEntities<T>(IEnumerable<Guid> ids, Guid? projectId = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<bool> DeleteEntity<T>(Guid id, Guid? projectId = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<T> GetEntityById<T>(Guid id, Guid? projectId = null, PathToLoad pathToLoad = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<List<T>> GetEntityByIds<T>(Guid[] ids, Guid? projectId = null, PathToLoad pathToLoad = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<int> GetEntityCount<T>(Guid? projectId = null, Filter filter = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<List<Guid>> GetEntityIds<T>(Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<List<T>> GetEntityList<T>(Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null, Dictionary<string, string> queryParams = null) where T : Entity;
        Task<HttpResponse> GetRaw(string resourceName, Filter filter, PathToLoad pathToLoad, Guid? projectId = null, IDictionary<string, string> additionalParams = null);
        bool IsTokenValid();
        Task<User> Login();
        Task<User> Login(string login, string password);
        void Logout();
        Task<TokenInfo> RenewToken();
        Task<HttpResponse> Request(string uri, ApiMethod method);
        Task<HttpResponse> Request(string uri, ApiMethod method, IDictionary<string, string> queryParams);
        Task<HttpResponse> Request(string uri, ApiMethod method, IDictionary<string, string> queryParams, string data, bool isFile = false);
        Task<HttpResponse> Request(string uri, ApiMethod method, IDictionary<string, string> queryParams,
            Stream stream);
        Task<T[]> UpdateEntities<T>(T[] entities, Guid? projectId = null) where T : Entity;
        Task<T> UpdateEntity<T>(T entity, Guid? projectId = null, Filter filter = null, PathToLoad pathToLoad = null, Dictionary<string, string> queryParams = null) where T : Entity;
    }
}