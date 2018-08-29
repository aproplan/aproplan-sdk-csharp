using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FilterProperty : FormFilterCondition
    {
        public bool? IsConform { get; set; }

        public string Value { get; set; }

        public bool NotApplicable { get; set; }

        public Guid ItemId { get; set; }

    }
}
