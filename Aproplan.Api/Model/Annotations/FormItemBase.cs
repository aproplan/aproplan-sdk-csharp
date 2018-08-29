using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FormItemBase : FormElementBase
    {
        public string Template
        {
            get;
            set;
        }
        public string ConformityRules
        {
            get;
            set;
        }

        public FormItemType ItemType
        {
            get;
            set;
        }

        public bool AllowAttachPicture
        {
            get; set;
        }

        public bool AllowAttachComment
        {
            get; set;
        }
    }
}
