using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.List
{
    public class MeetingConcern : Entity
    {
        public User User { get; set; }
        public MeetingPresenceStatus PresenceStatus { get; set; }
        public Meeting Meeting { get; set; }
        public bool IsReport { get; set; }
        public bool IsOwner { get; set; }
        public bool IsInvited { get; set; }
        public string Signature { get; set; }
        public DateTime? SignatureDate { get; set; }
        public int DisplayOrder { get; set; }
        public string Miscellaneous { get; set; }
        //AP-10436: Add IsDisabled property to be able to disable a participant and re-enable it thereafter.
        public bool IsDisabled { get; set; }
        //Calculated
        public ContactDetails ContactDetails { get; set; }
        //Calculated
        public bool IsInvitedOnProject { get; set; }
        // public bool IsInHistory { get; set; }
        //public AccessRightLevel AccessRightLevel { get; set; }
        //Calculated
        public bool HasSuperAdminModule { get; set; }
    }
}
