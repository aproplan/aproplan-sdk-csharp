using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public enum FormFilterType
    {
        And,
        Or
    }
    public class FilterCombination : FormFilterCondition
    {
        public FormFilterType Type { get; set; }

        public FormFilterCondition RightFilter { get; set; }
        public FormFilterCondition LeftFilter { get; set; }
    }
}
