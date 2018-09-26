using Aproplan.Api.Model.Projects.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class Note : NoteBase
    {
        public SubCell Cell
        {
            get;
            set;
        }
        
        public NoteProjectStatus Status
        {
            get;
            set;
        }

        public Guid? FormItemId
        {
            get;
            set;
        }

        public Guid? OriginalNoteId { get; set; }
    }
}
