using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class FormTemplate : Entity
    {
        public string Subject
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public bool IsArchived
        {
            get;
            set;
        }

        public Language Language
        { get; set; }

        public User Creator
        {
            get;
            set;
        }

        public int CreatorId
        { get; set; }

        public DateTime? ArchivedDate
        {
            get;
            set;
        }

        public FormType Type
        {
            get;
            set;
        }

        public Guid CompanyId { get; set; }


        public List<FormQuestion> Questions
        {
            get;
            set;
        }

        public List<FormSectionRule> SectionRules
        {
            get; set;
        }

        public bool MustDisplayElementsCode
        {
            get; set;
        }

        public bool IsSignatureAllowed
        {
            get; set;
        }

        public Form ToForm()
        {
            var formId = Guid.NewGuid();
            var sections = SectionRules?.Select(sr => sr.ToSection(formId)).ToList();

            Dictionary<Guid, Guid> mapQuestions = new Dictionary<Guid, Guid>();
            List<FormItem> items = new List<FormItem>();
            Questions?.ForEach((question) =>
            {
                FormItem item = question.ToFormItem(formId, sections);
                mapQuestions.Add(question.Id, item.Id);
                items.Add(item);                
            });

            sections.ForEach((section) =>
            {
                if(section.VisibleCondition != null)
                    FormFilterCondition.MapVisibleConditionQuestionToItemId(section.VisibleCondition, mapQuestions);
            });

            items.ForEach((item) =>
            {
                if (item.VisibleCondition != null)
                {
                    FormFilterCondition.MapVisibleConditionQuestionToItemId(item.VisibleCondition, mapQuestions);
                }
            });

            return new Form
            {
                Id = formId,
                TemplateId = Id,
                Subject = Subject,
                MustDisplayElementsCode = MustDisplayElementsCode,
                Type = Type,
                Language = Language,
                Sections = sections,
                Items = items,
                IsSignatureAllowed = IsSignatureAllowed,
            };
        }
    }
}
