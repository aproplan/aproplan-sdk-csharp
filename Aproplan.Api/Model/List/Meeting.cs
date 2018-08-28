using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.List
{
    public class Meeting : Entity
    {        
        public List<MeetingConcern> MeetingConcerns { get; set; }
        public List<MeetingDocument> MeetingDocuments { get; set; }
        public List<MeetingReport> Reports { get; set; }
        public List<MeetingTransferredDocs> TransferredDocuments { get; set; }

        public string Title
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public string Building
        {
            get;
            set;
        }

        public string Floor
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            set;
        }

        public int Revision
        {
            get;
            set;
        }

        public int Occurrence
        {
            get;
            set;
        }

        public bool IsArchived
        {
            get;
            set;
        }

        public MeetingType Type
        {
            get;
            set;
        }

        public string Header
        {
            get;
            set;
        }

        public string Footer
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        //V3
        public bool IsPublic
        {
            get;
            set;
        }

        //V3
        public bool IsSystem
        {
            get;
            set;
        }

        //Calculated
        public int NumberUnSentPoints
        {
            get;
            set;
        }

        //Calculated
        public int NumberSentPoints
        {
            get;
            set;
        }

        //Calculated
        public User MeetingAuthor
        {
            get;
            set;
        }

        //Bao added this property to know the right of the current user on the meeting
        //Note that, if the meeting is public and the current user is not the participant of the meeting, the access right in this case is the project access right of the user on meeting's project
        //Calculated
        //public MeetingAccessRight UserAccessRight { get; set; }

        //AP-6240 : Thumbnails view of the lists
        //Add these calculated properties to display on the list 
        public int NotesNumber { get; set; }
        public int TotalNotesNumber { get; set; }
        public int DocumentsNumber { get; set; }
        public int ParticipantsNumber { get; set; }

        public MeetingNumberingType NumberingType
        {
            get;
            set;
        }
    }
}
