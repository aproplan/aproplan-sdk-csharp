using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public class ComparisonFilter: PropertyFilter
    {
        public FilterComparisonType Type { get; set; }
        public object Value { get; set; }



        public override String ToString()
        {
            return $"Filter.{Type.ToString()}({PropertyPath.ToString()},{Value.ToString()})";
        }

        public ComparisonFilter(FilterComparisonType type, object value, string propertyPath) : base(propertyPath)
        {
            Type = type;
            Value = value;
        }

    }
}
