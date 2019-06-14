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
            FormSection section = sections.Find(x => x.SectionRuleId == SectionRuleId);

            var formItem = new FormItem
            {
                FormId = formId,
                Template = Template,
                SectionId = section.Id,
                QuestionId = Id,
                DisplayOrder = DisplayOrder,
                Title = Title,
                ItemType = ItemType,
                Description = Description,
                Code = Code,
                VisibleCondition = VisibleCondition == null ? null : VisibleCondition.Copy(),
                ConformityRules = ConformityRules,
                AllowAttachComment = AllowAttachComment,
                AllowAttachPicture = AllowAttachPicture,
            };
            return formItem;
        }
    }
}
