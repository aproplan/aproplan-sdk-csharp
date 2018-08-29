using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FormSection : FormSectionBase
    {
        public Guid? FormId { get; set; }

        public Guid SectionRuleId
        {
            get;
            set;
        }

        public bool IsVisible { get; set; }
    }
}
