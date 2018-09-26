using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.List
{
    public partial class MeetingTransferredDocs : Entity
    {
        public List<MeetingTransferredDocsTo> UsersTo { get; set; }
        public Meeting Meeting { get; set; }
        public string Name { get; set; }
        public User From { get; set; }
        public string FromTag { get; set; }
        public DateTime TransferredDate { get; set; }
        public string Approved { get; set; }

    }
}
