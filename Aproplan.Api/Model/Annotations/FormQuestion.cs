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
            Guid sectionId = sections.Find(x => x.SectionRuleId == this.SectionRuleId).Id; 

            return new FormItem
            {
                FormId = formId,
                Template = this.Template,
                SectionId = sectionId,
                QuestionId = this.Id, 
                DisplayOrder= this.DisplayOrder, 
                Title = this.Title, 
                ItemType = this.ItemType
            };
        }
    }
}
