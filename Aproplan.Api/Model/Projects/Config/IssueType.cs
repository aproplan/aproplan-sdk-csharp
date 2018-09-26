using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public partial class IssueType : IssueTypeBase
    {
        public int DisplayOrder { get; set; }
        public Chapter ParentChapter { get; set; }
        public List<IssueTypeNoteSubject> NoteSubjects { get; set; }
        public bool HasDefaultNoteSubjects { get; set; }

        //This is not a property bu a "flag" to know if the NoteSubjects have been loaded
        //It is used when saving issue types in order to know if we need to update the NoteSubjects (because they may have not been loaded
        //Defaut value is false which means the the list of subjects has been loaded
        //Used only by Aproplan clients -> do not add in API documentation
        public bool IdleSubjectList { get; set; }

        public List<ContactIssueType> ContactIssueTypeLinks { get; set; }

        //AP-8262: We need to know the category have how many punchlist are been linked 
        public long CategoryNumber { get; set; }

        //AP-11173: Add IsDisabled to IssueTypes
        public bool IsDisabled { get; set; }
    }
}
