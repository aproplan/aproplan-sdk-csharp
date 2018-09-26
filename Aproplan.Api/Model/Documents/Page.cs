using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Documents
{
    public partial class Page : SheetBase
    {
        public Guid DocumentId
        {
            get;
            set;
        }

        public Guid VersionId
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }
    }
}
