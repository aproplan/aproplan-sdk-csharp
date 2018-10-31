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

        public FormSection ToSection(Guid formId)
        {
            return new FormSection
            {
                Id = Guid.NewGuid(),
                FormId = formId,
                SectionRuleId = this.Id,
                DisplayOrder = this.DisplayOrder
            };
        }
    }
}
