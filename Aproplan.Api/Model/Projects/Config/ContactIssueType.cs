using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public partial class ContactIssueType : Entity
    {
        public Guid ContactId
        {
            get;
            set;
        }

        public Guid IssueTypeId
        {
            get;
            set;
        }

        public IssueType IssueType
        {
            get;
            set;
        }
    }
}
