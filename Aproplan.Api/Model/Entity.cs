using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model
{
    public abstract class Entity
    {
        public Guid Id;
        public string EntityVersion;
        public string[] ModifiedProperties;

        public Entity()
        {
            Id = new Guid();
        }
    }
}
