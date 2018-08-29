using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Projects.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class NoteProcessStatusHistory : Entity
    {
        public User User { get; set; }

        public DateTime Date { get; set; }

        public Note Note { get; set; }

        public NoteProjectStatus ProjectStatus { get; set; }
    }
}
