using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public partial class NoteProjectStatus : NoteProjectStatusBase
    {
        public Project Project { get; set; }
        public bool IsDisabled { get; set; }

    }
}
