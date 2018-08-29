using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class NoteInCharge : Entity
    {
        public Note Note
        {
            get;
            set;
        }
        public Guid NoteBaseId
        {
            get;
            set;
        }

        public Guid? UserId
        {
            get;
            set;
        }

        public string Tag
        {
            get;
            set;
        }

        // Calculated
        public bool IsContactInvitedOnProject
        {
            get;
            set;
        }
    }
}
