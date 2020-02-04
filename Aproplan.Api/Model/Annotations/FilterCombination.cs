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
    public partial class FilterCombination : FormFilterCondition
    {
        public FilterCombination()
        {
            
        }

        public FormFilterType Type { get; set; }

        public FormFilterCondition RightFilter { get; set; }
        public FormFilterCondition LeftFilter { get; set; }

        public override FormFilterCondition Copy()
        {
            var copy = new FilterCombination
            {
                Type = Type,
                RightFilter = RightFilter.Copy(),
                LeftFilter = LeftFilter.Copy()
            };
            return copy;
        }
    }
}
