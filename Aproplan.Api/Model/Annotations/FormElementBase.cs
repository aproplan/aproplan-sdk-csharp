using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public abstract partial class FormElementBase : Entity
    {
        public int DisplayOrder
        {
            get;
            set;
        }


        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public FormFilterCondition VisibleCondition
        {
            get; set;
        }
    }
}
