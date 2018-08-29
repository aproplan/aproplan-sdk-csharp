using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FormSectionBase : FormElementBase
    {
        public static int FormSectionApiVersion = 11;
        public int ItemsCount
        {
            get; set;
        }
    }
}
