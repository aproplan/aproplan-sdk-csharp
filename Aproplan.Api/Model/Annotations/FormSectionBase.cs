using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class FormSectionBase : FormElementBase
    {
        public static int FormSectionApiVersion = 11;
        public int ItemsCount
        {
            get; set;
        }
    }
}
