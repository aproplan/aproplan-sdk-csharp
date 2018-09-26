using Aproplan.Api.Model.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.List
{
    public partial class MeetingDocument : Entity
    {
        public Meeting Meeting { get; set; }
        public Document Document { get; set; }
        public int DisplayOrder { get; set; }
        // public bool IsInHistory { get; set; }
    }
}
