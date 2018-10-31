using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class Form : NoteBase
    {
        public FormStatus Status
        {
            get;
            set;
        }

        public bool IsConform
        {
            get;
            set;
        }

        public List<FormItem> Items
        {
            get;
            set;
        }

        public List<FormSection> Sections
        {
            get;
            set;
        }

        public FormType Type
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public DateTime? DoneDate
        {
            get;
            set;
        }

        public Guid TemplateId
        {
            get;
            set;
        }
        public Document Report
        {
            get;
            set;
        }

        public bool MustDisplayElementsCode
        {
            get; set;
        }

        public FormTemplate Template // THIS SHOULD BE REMOVED ; used temporary 
        {
            get; set; 
        }
    }
}
