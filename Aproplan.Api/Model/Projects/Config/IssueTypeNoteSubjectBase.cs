using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public abstract partial class IssueTypeNoteSubjectBase : Entity
    {
        public string Subject { get; set; }
        public int DisplayOrder { get; set; }
        /// <summary>
        /// AP-11043: We need a way to save the default description linked to a subject that is linked to a sub category (IssueType)
        /// </summary>
        public string DefaultDescription { get; set; }
    }
}
