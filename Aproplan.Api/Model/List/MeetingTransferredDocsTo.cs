using System;

namespace Aproplan.Api.Model.List
{
    public class MeetingTransferredDocsTo : Entity
    {
        public MeetingTransferredDocs MeetingTransferredDocs { get; set; }
        public Guid? UserId { get; set; }
        public string Tag { get; set; }
    }
}