using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Documents
{
    public class Version : DocumentBase
    {
        public Guid DocumentId
        {
            get;
            set;
        }

        public int VersionIndex
        {
            get;
            set;
        }

        public Guid PhysicalFolderId { get; set; }

        public int PageNbr { get; set; }
    }
}
