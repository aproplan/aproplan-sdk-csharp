using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects
{
    public class LinkedIssueType: Entity
    {
        public Guid ContactId
        {
            get;
            set;
        }

        public Guid IssueTypeId { get; set; }
    }
}
