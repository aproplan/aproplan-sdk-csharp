using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class FormSectionRule : FormSectionBase
    {
        public Guid? FormTemplateId
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
