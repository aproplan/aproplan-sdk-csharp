using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public abstract class PropertyFilter : Filter
    {
        public string propertyPath { get; set; }

        public PropertyFilter(string propertyPath)
        {
            propertyPath = propertyPath;
        }
    }
}
