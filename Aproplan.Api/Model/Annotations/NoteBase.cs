using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public abstract class NoteBase : Entity
    {
        public string CodeNum
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public DateTime ModificationDate
        {
            get;
            set;
        }

        public User From
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            set;
        }

        public bool IsArchived
        {
            get;
            set;
        }

        public bool IsUrgent
        {
            get;
            set;
        }

        public List<NoteComment> Comments
        {
            get;
            set;
        }

        public List<NoteInCharge> NoteInCharge
        {
            get;
            set;
        }

        public List<NoteDocument> NoteDocuments
        {
            get;
            set;
        }

        public DateTime? DueDate
        {
            get;
            set;
        }

        public IssueType IssueType
        {
            get;
            set;
        }

        public DateTime? ArchivedDate
        {
            get;
            set;
        }

        public Guid? ParentNoteId { get; set; }

        public Meeting Meeting
        {
            get;
            set;
        }

        public string NumberCreation
        {
            get;
            set;
        }

        public string NumberModification
        {
            get;
            set;
        }

        public int? OccurrenceCreation
        {
            get;
            set;
        }

        public int? OccurrenceModification
        {
            get;
            set;
        }

        // Calculated
        //public MeetingAccessRight MeetingAccessRight
        //{
        //    get;
        //    set;
        //}

        public List<NoteProcessStatusHistory> ProcessStatusHistories
        {
            get;
            set;
        }

        public int MeetingNumSeq
        {
            get;
            set;
        }

        public int ProjectNumSeq
        {
            get;
            set;
        }

        // Calculated
        public string Code
        {
            get;
            set;
        }

        public bool IsReadOnly
        {
            get;
            set;
        }


        public bool HasAttachment { get; set; }

    }
}
