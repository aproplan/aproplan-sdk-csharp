using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects
{
    public class FolderIdLevel
    {
        public Guid Id;

        [JsonProperty(PropertyName = "Object")]
        public int Level;
    }
}
