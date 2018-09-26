using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
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
    }
}
