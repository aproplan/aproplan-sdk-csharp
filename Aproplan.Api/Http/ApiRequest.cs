using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Http
{
    public class ApiRequest : ApiRequestBase
    {

        public ApiRequest(string login, string password, Guid requesterId, string apiVersion = "13", string rootUrl = "https://app.aproplan.com") : base(login, password, requesterId, apiVersion, rootUrl){}

        public ApiRequest(string login, string password, Guid requesterId, string rootUrl = null) : base(login, password, requesterId, ApiRequestBase.DefaultApiVersion, rootUrl){}

        public ApiRequest(Guid requesterId, string rootUrl = null) : base(requesterId, rootUrl) { }


        public async Task<SyncResult<Note>> SyncNotes(String continuationToken)  { return await this.SyncEntity<Note>(continuationToken); }
        public async Task<SyncResult<Form>> SyncForms(String continuationToken)  { return await this.SyncEntity<Form>(continuationToken); }
        public async Task<SyncResult<FormTemplate>> SyncFormTemplates(String continuationToken)  { return await this.SyncEntity<FormTemplate>(continuationToken); }
        public async Task<SyncResult<IssueType>> SyncIssueTypes(String continuationToken)  { return await this.SyncEntity<IssueType>(continuationToken); }
        public async Task<SyncResult<Chapter>> SyncChapters(String continuationToken)  { return await this.SyncEntity<Chapter>(continuationToken); }
        public async Task<SyncResult<SubCell>> SyncSubCells(String continuationToken)  { return await this.SyncEntity<SubCell>(continuationToken); }
        public async Task<SyncResult<ParentCell>> SyncParentCells(String continuationToken)  { return await this.SyncEntity<ParentCell>(continuationToken); }
        public async Task<SyncResult<Folder>> SyncFolders(String continuationToken)  { return await this.SyncEntity<Folder>(continuationToken); }
        public async Task<SyncResult<Project>> SyncProjects(String continuationToken)  { return await this.SyncEntity<Project>(continuationToken); }
        public async Task<SyncResult<ContactDetails>> SyncContactDetails(String continuationToken)  { return await this.SyncEntity<ContactDetails>(continuationToken); }
        public async Task<SyncResult<NoteProjectStatus>> SyncProjectStatus(String continuationToken)  { return await this.SyncEntity<NoteProjectStatus>(continuationToken); }
        public async Task<SyncResult<User>> SyncUsers(String continuationToken)  { return await this.SyncEntity<User>(continuationToken); }
        public async Task<SyncResult<Meeting>> SyncMeetings(String continuationToken) { return await this.SyncEntity<Meeting>(continuationToken); }
        public async Task<SyncResult<Document>> SyncAttachmentDocuments(String continuationToken)  { return await this.SyncEntity<Document>(continuationToken, "attachmentdocumentsync"); }
        public async Task<SyncResult<Document>> SyncFolderDocuments(String continuationToken)  { return await this.SyncEntity<Document>(continuationToken, "folderdocumentsync"); }
    }
}
