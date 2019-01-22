using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.AccessRights;
using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Http.Services
{
    public class SyncService: BaseService
    {
        public delegate void SyncBatchCallback<T>(SyncResult<T> batchData, ref bool breakCall) where T : Entity;
        #region Methods

        public async Task<SyncResult<CompanyUser>> SyncCompanyUsers(String continuationToken) { return await SyncEntity<CompanyUser>(continuationToken); }
        public async Task<SyncResult<User>> SyncUsers(string continuationToken) { return await SyncEntity<User>(continuationToken); }
        public async Task<SyncResult<AccessRightBase>> SyncAccessRights(string continuationToken) { return await SyncEntity<AccessRightBase>(continuationToken); }
        public async Task<SyncResult<FormTemplate>> SyncFormTemplates(string continuationToken) { return await SyncEntity<FormTemplate>(continuationToken); }
        public async Task<SyncResult<Project>> SyncProjects(String continuationToken) { return await this.SyncEntity<Project>(continuationToken); }

        public async Task<SyncResult<Note>> SyncNotes(Guid projectId, String continuationToken) { return await SyncEntity<Note>(continuationToken, projectId); }
        public async Task<SyncResult<NoteBase>> SyncNoteBaseVisibilityLostSync(Guid projectId, String continuationToken) { return await SyncEntity<NoteBase>(continuationToken, projectId, null, "notebasevisibilitylostsync"); }
        public async Task<SyncResult<Form>> SyncForms(Guid projectId, String continuationToken) { return await SyncEntity<Form>(continuationToken, projectId); }        
        public async Task<SyncResult<IssueType>> SyncIssueTypes(Guid projectId, String continuationToken) { return await SyncEntity<IssueType>(continuationToken, projectId); }
        public async Task<SyncResult<Chapter>> SyncChapters(Guid projectId, String continuationToken) { return await SyncEntity<Chapter>(continuationToken, projectId); }
        public async Task<SyncResult<SubCell>> SyncSubCells(Guid projectId, String continuationToken) { return await SyncEntity<SubCell>(continuationToken, projectId); }
        public async Task<SyncResult<ParentCell>> SyncParentCells(Guid projectId, String continuationToken) { return await SyncEntity<ParentCell>(continuationToken, projectId); }
        public async Task<SyncResult<Folder>> SyncFolders(Guid projectId, String continuationToken) { return await SyncEntity<Folder>(continuationToken, projectId); }

        public async Task<SyncResult<ContactDetails>> SyncContactDetails(Guid projectId, String continuationToken) { return await SyncEntity<ContactDetails>(continuationToken, projectId); }
        public async Task<SyncResult<NoteProjectStatus>> SyncProjectStatus(Guid projectId, String continuationToken) { return await SyncEntity<NoteProjectStatus>(continuationToken, projectId); }        
        public async Task<SyncResult<Meeting>> SyncMeetings(Guid projectId, String continuationToken) { return await SyncEntity<Meeting>(continuationToken, projectId); }
        public async Task<SyncResult<Document>> SyncAttachmentDocuments(Guid projectId, String continuationToken) { return await SyncEntity<Document>(continuationToken, projectId, null, "attachmentdocumentsync"); }
        public async Task<SyncResult<Document>> SyncFolderDocuments(Guid projectId, String continuationToken) { return await SyncEntity<Document>(continuationToken, projectId, null, "folderdocumentsync"); }



        public async Task<SyncResult<CompanyUser>> SyncAllCompanyUsers(String continuationToken, SyncBatchCallback<CompanyUser> callback = null) { return await SyncAllEntities<CompanyUser>(continuationToken, null, null, null, callback); }
        public async Task<SyncResult<User>> SyncAllUsers(string continuationToken, SyncBatchCallback<User> callback = null) { return await SyncAllEntities<User>(continuationToken, null, null, null, callback); }
        public async Task<SyncResult<AccessRightBase>> SyncAllAccessRights(string continuationToken, SyncBatchCallback<AccessRightBase> callback = null) { return await SyncAllEntities<AccessRightBase>(continuationToken, null, null, null, callback); }
        public async Task<SyncResult<FormTemplate>> SyncAllFormTemplates(string continuationToken, SyncBatchCallback<FormTemplate> callback = null) { return await SyncAllEntities<FormTemplate>(continuationToken, null, null, null, callback); }
        public async Task<SyncResult<Project>> SyncAllProjects(String continuationToken, SyncBatchCallback<Project> callback = null) { return await this.SyncAllEntities<Project>(continuationToken, null, null, null, callback); }

        public async Task<SyncResult<Note>> SyncAllNotes(Guid projectId, String continuationToken, SyncBatchCallback<Note> callback = null) { return await SyncAllEntities<Note>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<NoteBase>> SyncAllNoteBaseVisibilityLost(Guid projectId, String continuationToken, SyncBatchCallback<NoteBase> callback = null) { return await SyncAllEntities<NoteBase>(continuationToken, projectId, null, "notebasevisibilitylostsync", callback); }
        public async Task<SyncResult<Form>> SyncAllForms(Guid projectId, String continuationToken, SyncBatchCallback<Form> callback = null) { return await SyncAllEntities<Form>(continuationToken, projectId, null, null, callback); }        
        public async Task<SyncResult<IssueType>> SyncAllIssueTypes(Guid projectId, String continuationToken, SyncBatchCallback<IssueType> callback = null) { return await SyncAllEntities<IssueType>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<Chapter>> SyncAllChapters(Guid projectId, String continuationToken, SyncBatchCallback<Chapter> callback = null) { return await SyncAllEntities<Chapter>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<SubCell>> SyncAllSubCells(Guid projectId, String continuationToken, SyncBatchCallback<SubCell> callback = null) { return await SyncAllEntities<SubCell>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<ParentCell>> SyncAllParentCells(Guid projectId, String continuationToken, SyncBatchCallback<ParentCell> callback = null) { return await SyncAllEntities<ParentCell>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<Folder>> SyncAllFolders(Guid projectId, String continuationToken, SyncBatchCallback<Folder> callback = null) { return await SyncAllEntities<Folder>(continuationToken, projectId, null, null, callback); }

        public async Task<SyncResult<ContactDetails>> SyncAllContactDetails(Guid projectId, String continuationToken, SyncBatchCallback<ContactDetails> callback = null) { return await SyncAllEntities<ContactDetails>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<NoteProjectStatus>> SyncAllProjectStatus(Guid projectId, String continuationToken, SyncBatchCallback<NoteProjectStatus> callback = null) { return await SyncAllEntities<NoteProjectStatus>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<Meeting>> SyncAllMeetings(Guid projectId, String continuationToken, SyncBatchCallback<Meeting> callback = null) { return await SyncAllEntities<Meeting>(continuationToken, projectId, null, null, callback); }
        public async Task<SyncResult<Document>> SyncAllAttachmentDocuments(Guid projectId, String continuationToken, SyncBatchCallback<Document> callback = null) { return await SyncAllEntities<Document>(continuationToken, projectId, null, "attachmentdocumentsync", callback); }
        public async Task<SyncResult<Document>> SyncAllFolderDocuments(Guid projectId, String continuationToken, SyncBatchCallback<Document> callback = null) { return await SyncAllEntities<Document>(continuationToken, projectId, null, "folderdocumentsync", callback); }

        

        protected async Task<SyncResult<T>> SyncAllEntities<T>(String syncStamp, Guid? projectId = null, int? requestedBlockSize = null, String forcedResourceName = null, SyncBatchCallback<T> callback = null ) where T: Entity
        {
            SyncResult<T> allResult = new SyncResult<T>() { Data = new List<T>() };
            bool breakCall = false;
            SyncResult<T> result = null;
            string syncToken = syncStamp;
            while ((result == null || result.Data.Count > 0) && !breakCall)
            {
                result = await SyncEntity<T>(syncToken, projectId, requestedBlockSize, forcedResourceName);
                if (result.Data.Count > 0)
                {
                    allResult.Data.AddRange(result.Data);
                    callback?.Invoke(result, ref breakCall);
                }
                allResult.ContinuationToken = result.ContinuationToken;
                syncToken = allResult.ContinuationToken;
                if (string.IsNullOrEmpty(syncToken)) break;
            }
            return allResult;
        }

        /// <summary>
        /// To get all the changes of an entity since a specified point in time
        /// </summary>
        /// <typeparam name="T">Kind of entity to get</typeparam>
        /// <param name="syncStamp">This is the stamp from when the sync need to get differences. If not specified, it means to get data from the beginning. 
        /// Else it corresponds to the last sync done. Value returned in the SyncTimestamp property of the response headers</param>
        /// <param name="forcedResourceName">IF the end point to use is not really the name of the entity, you can specify the name of the resource endpoint to use</param>
        /// <returns>A SyncResult containing the data from the syncStamp you specified and until a new SyncStamp specified in ContinuationToken</returns>
        protected async Task<SyncResult<T>> SyncEntity<T>(String syncStamp, Guid? projectId = null, int? requestedBlockSize = null, String forcedResourceName = null) where T : Entity
        {
            string resourceName = forcedResourceName;
            if (resourceName == null)
            {
                resourceName = Requester.GetEntityResourceName<T>(GetEntityResourceType.Sync);
            }
            IDictionary<String, String> additionalParams = new Dictionary<String, String>();
            if(!string.IsNullOrEmpty(syncStamp))
                additionalParams["timestamp"] = syncStamp;
            if (requestedBlockSize.HasValue)
            {
                if (requestedBlockSize.Value <= 0) throw new ApiException("requestedBlockSize cannot be zero or negative");
                additionalParams["requestedBlockSize"] = requestedBlockSize.Value.ToString();

            }
            HttpResponse res = await Requester.GetRaw(resourceName, null, null, projectId, additionalParams);
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
        #endregion

        #region Constructor

        public SyncService(ApiRequest apiRequest) : base(apiRequest)
        {
            
        }

        #endregion
    }
}
