using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public abstract partial class IssueTypeBase : Entity
    {
        public string Code { get; set; }

        public string Description { get; set; }

    }
}
