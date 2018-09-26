using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.AccessRights
{
    public partial class MeetingAccessRight : AccessRightBase
    {
        public MeetingAccessRight()
            : base()
        {
        }

        public bool CanAddPoint { get; set; }
        public bool CanEditPoint { get; set; }
        public bool CanDeletePoint { get; set; }
        public bool CanEditPointStatus { get; set; }
        public bool CanEditPointIssueType { get; set; }
        public bool CanEditPointInCharge { get; set; }
        public bool CanAddComment { get; set; }
        public bool CanDeleteComment { get; set; }
        public bool CanArchiveComment { get; set; }
        public bool CanAddDoc { get; set; }
        public bool CanGenerateReport { get; set; }
        public bool CanCreateNextMeeting { get; set; }
        public bool CanEditAllPoint { get; set; }
        public bool CanViewOnlyPointInCharge { get; set; }
        public bool CanAddPointDocument { get; set; }
        public bool CanDeletePointDocument { get; set; }
        public bool CanViewDashboard { get; set; }
        public bool CanViewParticipant { get; set; }
    }
}
