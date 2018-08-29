using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FormItem : FormItemBase
    {
        public User User
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public bool NotApplicable
        {
            get;
            set;
        }

        public bool IsConform
        {
            get;
            set;
        }

        public bool NeedAnswer
        {
            get; set;
        }

        public Guid? FormId
        {
            get;
            set;
        }

        public Guid? SectionId
        {
            get;
            set;
        }

        public Guid QuestionId
        {
            get;
            set;
        }

        public List<NoteDocument> NoteDocuments
        {
            get;
            set;
        }
    }
}
