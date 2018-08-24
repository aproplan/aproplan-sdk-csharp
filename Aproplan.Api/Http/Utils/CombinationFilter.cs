using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{

    public enum FilterCombinationType
    {
        And, Or
    }

    public class CombinationFilter: Filter
    {
        public FilterCombinationType Type;
        public Filter LeftFilter;
        public Filter RightFilter;

        public CombinationFilter(Filter leftFilter, Filter rightFilter, FilterCombinationType type)
        {
            Type = type;
            LeftFilter = leftFilter;
            RightFilter = rightFilter;
        }

        public override String ToString()
        {
            return $"Filter.{Type.ToString()}({LeftFilter.ToString()},{RightFilter.ToString()})";
        }
    }
}
