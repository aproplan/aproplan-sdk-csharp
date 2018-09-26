using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class NoteComment : Entity
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

        public User From
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public DateTime LastModificationDate
        {
            get;
            set;
        }

        public bool IsFirst
        {
            get;
            set;
        }

        public bool IsRead
        {
            get;
            set;
        }

        public List<Drawing> Drawings
        {
            get;
            set;
        }

        public int MeetingOccurrence
        {
            get;
            set;
        }

        public int MeetingRevision
        {
            get;
            set;
        }

        public string MeetingPointNumber
        {
            get;
            set;
        }

        public bool IsArchived
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public Guid? FormItemId
        {
            get;
            set;
        }
    }
}
