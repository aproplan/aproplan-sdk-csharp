using Aproplan.Api.Model.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Http.Services
{
    public enum UploadFileType
    {
        Working, 
        Source
    }
    
    public class DocumentService: BaseService
    {
        private enum UploadTarget
        {
            Document,
            Version
        }

        private enum UploadAction
        {
            Add, 
            Join
        }

        #region Methods

        /// <summary>
        /// To upload a new document as working file. File type accepted are: image and pdf. Working file means that you will able to view it through APROPLAN
        /// </summary>
        /// <param name="filePath">The path of the file to upload</param>
        /// <param name="folderId">The folder id where the document must be uploaded in the project</param>
        /// <param name="nameDocument">The name of the document to set into APROPLAN</param>
        /// <returns></returns>
        public async Task<Document> UploadNewDocument(Guid projectId, string filePath, Guid folderId, string nameDocument = null)
        {
            return await UploadDocumentOrVersion(projectId, UploadFileType.Working, filePath, folderId, nameDocument);
        }
        
        public async Task<Document> UploadNewDocument(Guid projectId, Stream stream, Guid folderId, string nameDocument = null)
        {
            return await UploadDocumentOrVersion(projectId, UploadFileType.Working, stream, folderId, nameDocument);
        }

        /// <summary>
        /// To upload and join a source file to an existing document. 
        /// </summary>
        /// <param name="filePath">The path of the file to upload</param>
        /// <param name="document">The document on which the source file must be attached</param>
        /// <returns></returns>
        public async Task<Document> UploadSourceToDocument(Guid projectId, string filePath, Document document)
        {
            
            return await UploadDocumentOrVersion(projectId, UploadFileType.Source, filePath, null, null, document.Id, document.VersionCount == 0 ? UploadTarget.Document : UploadTarget.Version, UploadAction.Join);
        }
        
        public async Task<Document> UploadSourceToDocument(Guid projectId, Stream stream, Document document)
        {
            
            return await UploadDocumentOrVersion(projectId, UploadFileType.Source, stream, null, null, document.Id, document.VersionCount == 0 ? UploadTarget.Document : UploadTarget.Version, UploadAction.Join);
        }

        /// <summary>
        /// To upload a new version to an existing document. 
        /// </summary>
        /// <param name="documentId">The document id on which the new version will be added. The document on which a new version must be uploaded must be completely processed before to upload it</param>
        /// <param name="filePath">The path of the file to upload</param>
        /// <param name="documentName">The new name of the document to set into APROPLAN if need to be changed</param>
        /// <param name="uploadFileType">To know if the new version is for working file or source file</param>
        /// <returns></returns>
        public async Task<Document> UploadVersion(Guid projectId, Guid documentId, string filePath, string documentName = null, UploadFileType uploadFileType = UploadFileType.Working)
        {
            return await UploadDocumentOrVersion(projectId, uploadFileType, filePath, null, documentName, documentId, UploadTarget.Version, UploadAction.Add);
        }
        
        public async Task<Document> UploadVersion(Guid projectId, Guid documentId, Stream stream, string documentName = null, UploadFileType uploadFileType = UploadFileType.Working)
        {
            return await UploadDocumentOrVersion(projectId, uploadFileType, stream, null, documentName, documentId, UploadTarget.Version, UploadAction.Add);
        }

        private async Task<Document> UploadDocumentOrVersion(Guid projectId, UploadFileType uploadFileType, 
                    string filePath, Guid? folderId = null, string nameDocument = null, Guid? documentId = null, 
                    UploadTarget target = UploadTarget.Document, UploadAction action = UploadAction.Add)
        {
            Dictionary<string, string> queryParams = BuildQueryParamsUploadDoc(projectId, uploadFileType, filePath,
                folderId, nameDocument, documentId, target, action);

            

            string docJson = (await Requester.Request(Requester.ApiRootUrl + "uploaddocument", ApiMethod.Post, queryParams, filePath, true)).Data;
            return JsonConvert.DeserializeObject<Document>(docJson);
        }
        
        private async Task<Document> UploadDocumentOrVersion(Guid projectId, UploadFileType uploadFileType, 
            Stream stream, Guid? folderId = null, string nameDocument = null, Guid? documentId = null, 
            UploadTarget target = UploadTarget.Document, UploadAction action = UploadAction.Add)
        {
            Dictionary<string, string> queryParams = BuildQueryParamsUploadDoc(projectId, uploadFileType, string.Empty,
                folderId, nameDocument, documentId, target, action);

            

            string docJson = (await Requester.Request(Requester.ApiRootUrl + "uploaddocument", ApiMethod.Post, queryParams, stream)).Data;
            return JsonConvert.DeserializeObject<Document>(docJson);
        }

        private Dictionary<string, string> BuildQueryParamsUploadDoc(Guid projectId, UploadFileType uploadFileType, 
            string filePath, Guid? folderId = null, string nameDocument = null, Guid? documentId = null, 
            UploadTarget target = UploadTarget.Document, UploadAction action = UploadAction.Add)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "file", uploadFileType.ToString().ToLowerInvariant()},
                { "action", action.ToString().ToLowerInvariant()},
                { "target", target.ToString().ToLowerInvariant()},
                { "projectid", projectId.ToString().ToLowerInvariant() }
            };
            if(target == UploadTarget.Document && action == UploadAction.Add)
            {
                if (String.IsNullOrEmpty(nameDocument) && !string.IsNullOrEmpty(filePath))
                    nameDocument = Path.GetFileNameWithoutExtension(filePath);
            }

            if (!String.IsNullOrEmpty(nameDocument))
                queryParams.Add("name", nameDocument);

            if (folderId.HasValue)
                queryParams.Add("folderid", folderId.Value.ToString());
            if (documentId.HasValue)
                queryParams.Add("parentdocid", documentId.Value.ToString());
            return queryParams;
        }

        #endregion

        #region Constructor
        public DocumentService(IApiRequest apiRequest): base(apiRequest)
        {

        }

        #endregion
    }
}
