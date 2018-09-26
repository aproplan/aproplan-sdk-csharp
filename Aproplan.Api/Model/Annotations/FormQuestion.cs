using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class FormQuestion : FormItemBase
    {
        public Guid? FormTemplateID
        {
            get;
            set;
        }

        public Guid? SectionRuleId
        {
            get;
            set;
        }

        public bool IsArchived
        {
            get; set;
        }

        public DateTime? ArchivedDate
        {
            get; set;
        }
    }
}
