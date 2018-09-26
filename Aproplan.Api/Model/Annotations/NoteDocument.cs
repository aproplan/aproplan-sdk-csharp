using Aproplan.Api.Model.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class NoteDocument : Entity
    {
        public Note Note
        {
            get;
            set;
        }

        public Form Form
        {
            get;
            set;
        }

        public Guid NoteBaseId
        {
            get;
            set;
        }

        public Document Document
        {
            get;
            set;
        }

        public int DisplayOrder
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
