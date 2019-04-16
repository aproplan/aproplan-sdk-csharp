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

        public FormItem ToFormItem(Guid formId, List<FormSection> sections)
        {
            Guid sectionId = sections.Find(x => x.SectionRuleId == SectionRuleId).Id;

            return new FormItem
            {
                FormId = formId,
                Template = Template,
                SectionId = sectionId,
                QuestionId = Id,
                DisplayOrder = DisplayOrder,
                Title = Title,
                ItemType = ItemType,
                Description = Description,
                Code = Code,
                VisibleCondition = VisibleCondition,
            };
        }
    }
}
