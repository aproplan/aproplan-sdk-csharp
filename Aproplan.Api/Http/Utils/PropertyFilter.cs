using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public abstract class PropertyFilter : Filter
    {
        public string PropertyPath { get; set; }

        public PropertyFilter(string propertyPath)
        {
            PropertyPath = propertyPath;
        }
    }
}
