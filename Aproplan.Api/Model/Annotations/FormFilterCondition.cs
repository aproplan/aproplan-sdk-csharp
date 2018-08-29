using Aproplan.Api.Http.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    [JsonConverter(typeof(FormFilterConditionJsonConverter))]
    public abstract class FormFilterCondition : Entity
    {
        public Guid? FormElementId { get; set; }

        public string EntityDiscriminator { get; set; }
    }
}
