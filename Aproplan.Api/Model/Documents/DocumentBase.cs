using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Documents
{
    public partial class DocumentBase : SheetBase
    {
        public string ImageUrl
        {
            get;
            set;
        }

        public string SourceUrl
        {
            get;
            set;
        }

        public string OriginalImageName
        {
            get;
            set;
        }

        public string OriginalSourceName
        {
            get;
            set;
        }

        public User Author
        {
            get;
            set;
        }

        public string AuthorName
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public User UploadedBy
        {
            get;
            set;
        }

        public string UploadedByName
        {
            get;
            set;
        }

        public DateTime UploadedDate
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public int Scale
        {
            get;
            set;
        }

        public string References
        {
            get;
            set;
        }

        public int RotateAngle
        {
            get;
            set;
        }
        public FileType FileType
        {
            get;
            set;
        }

        public List<Page> Pages
        {
            get;
            set;
        }

        public Decimal TilesStorageSize
        {
            get;
            set;
        }

        public Decimal SourceStorageSize
        {
            get;
            set;
        }

        public Decimal DocStorageSize
        {
            get;
            set;
        }

        public int? Resolution
        {
            get;
            set;
        }
    }
}
