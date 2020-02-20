using Aproplan.Api.Model.Projects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Http.Services
{
    public class ProjectService : BaseService
    {
        #region Methods

        /// <summary>
        /// Retrieve the list of folders of a project whith the hierarchy structure.
        /// </summary>
        /// <param name="projectId">The project is for which the folder structure is requested</param>
        /// <param name="folderType">The type of the folder to retrieve (FolderType)</param>
        /// <returns>The list of folder's ids and its level in the hierarchy corresponding to the criteria</returns>
        public async Task<List<FolderIdLevel>> GetFoldersIdHierarchy(Guid projectId, string folderType = null)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "projectid", projectId.ToString().ToLowerInvariant()}
            };

            if (!String.IsNullOrEmpty(folderType))
                queryParams.Add("foldertype", folderType);

            string dataJson = (await Requester.Request(Requester.ApiRootUrl + "projectfolderidsandlevels", ApiMethod.Get, queryParams)).Data;
            return JsonConvert.DeserializeObject<List<FolderIdLevel>>(dataJson);
        }

        #endregion

        #region Constructor
        public ProjectService(IApiRequest apiRequest) : base(apiRequest)
        {

        }

        #endregion
    }
}
