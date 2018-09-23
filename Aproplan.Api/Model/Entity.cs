using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model
{
    public abstract partial class Entity
    {
        public Guid Id;
        public int EntityVersion;
        public string[] ModifiedProperties;

        public Entity()
        {
            Id = Guid.NewGuid();
            EntityVersion = 0;
        }
    }
}
