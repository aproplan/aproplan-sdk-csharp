using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.AccessRights
{
    public abstract class AccessRightBase : Entity
    {
        public bool CanEdit { get; set; }

        public AccessRightLevel Level { get; set; }
        public string ModuleName { get; set; }

    }
}
