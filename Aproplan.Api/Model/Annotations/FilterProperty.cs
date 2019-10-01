using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public partial class FilterProperty : FormFilterCondition
    {
        public FilterProperty()
        {
            EntityDiscriminator = nameof(FilterProperty);
        }

        public bool? IsConform { get; set; }

        public string Value { get; set; }

        public bool NotApplicable { get; set; }

        public Guid ItemId { get; set; }

        public override FormFilterCondition Copy()
        {
            var copy = new FilterProperty
            {
                IsConform = IsConform,
                Value = Value,
                NotApplicable = NotApplicable,
                ItemId = ItemId
            };
            return copy;
        }

    }
}
