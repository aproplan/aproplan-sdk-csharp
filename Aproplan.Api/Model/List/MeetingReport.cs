using Aproplan.Api.Model.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.List
{
    public partial class MeetingReport : Entity
    {
        public Meeting Meeting { get; set; }
        public Document Document { get; set; }
        public Guid? VersionId { get; set; }
        public int Occurrence { get; set; }
        public int Revision { get; set; }
        public DateTime SentDate { get; set; }
    }
}
