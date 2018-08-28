using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public class Chapter : ChapterBase
    {
        public int DisplayOrder { get; set; }
        public Guid ProjectId { get; set; }

        //AP-11172: Add IsDisabled to Chapters
        public bool IsDisabled { get; set; }
        public List<IssueType> IssueTypes { get; set; }

    }
}
