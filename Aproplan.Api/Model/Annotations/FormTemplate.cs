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

        public Form ToForm()
        {
            var formId = Guid.NewGuid();
            var sections = this.SectionRules?.Select(sr => sr.ToSection(formId)).ToList();

            return new Form
            {
                Id = formId,
                TemplateId = this.Id,
                Subject = this.Subject,
                Type = this.Type,
                Language = this.Language,
                Sections = sections,
                Items = this.Questions?.Select(q => q.ToFormItem(formId, sections)).ToList(),
            };
        }
    }
}
