using Aproplan.Api.Model.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Documents
{
    public enum DocumentStatus
    {
        NotUploaded = 0,
        Uploaded = 1,
        Processed = 2
    }

    public enum ProcessingStatus
    {
        Unknown,
        ToUpload,
        UploadFailed,
        ToGenerateReport,
        GeneratingReport,
        GenerationReportFailed,
        ToGenerateTiles,
        TilesProcessing,
        TilesProcessingFailed,
        FullyCompleted,
        ToGenerateCacheFile,
        GenerateCacheFileError
    }

    public enum DocumentIndexStatus
    {
        NotIndexed = 0,
        FullIndexed = 1,
        PartialIndexed = 2,
        CanNotIndexed = 3,
        ToRemoveIndex = 4,
        UpdateVersion = 5
    }


    public partial class Document : DocumentBase
    {

        public Folder Folder
        {
            get;
            set;
        }

        public Guid FolderId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int PagesCount
        {
            get;
            set;
        }

        public bool HasDifference
        {
            get;
            set;
        }

        public int NotesCount
        {
            get;
            set;
        }

        public int MeetingPointsCount
        {
            get;
            set;
        }

        public DocumentStatus Status
        {
            get;
            set;
        }
        public DocumentStatus SourceFileStatus
        {
            get;
            set;
        }

        public DocumentIndexStatus IndexStatus
        {
            get;
            set;
        }

        public Guid PhysicalFolderId
        {
            get;
            set;
        }

        public string FolderPath { get; set; }

        public List<Version> Versions { get; set; }

        public int VersionCount { get; set; }

        public bool IsArchived { get; set; }

        /// <summary>
        /// Calculated Property
        /// To know the document is report or other type
        /// </summary>
        public bool IsReport { get; set; }

        public int DisplayOrder { get; set; }


        public int ConvertRetryCount { get; set; }

        public bool IsReportRequestError { get; set; }

        public ProcessingStatus ProcessingStatus
        {
            get;
            set;
        }

        public int? NumPageProcessing
        {
            get;
            set;
        }

        public int? ProjectNumSeq { get; set; }
    }
}
