using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public class UnaryFilter : PropertyFilter
    {
        public UnaryFilter(FilterUnaryType type, string propertyPath = null): base(propertyPath)
        {
            Type = type;
        }

        public FilterUnaryType Type { get; set; }

        public override String ToString()
        {
            return $"Filter.{Type.ToString()}({propertyPath.ToString()})";
        }
    }
}
