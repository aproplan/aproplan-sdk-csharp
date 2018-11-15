using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model
{
    public abstract partial class Entity
    {
        public Guid Id { get; set; }
        public int EntityVersion { get; set; }
        public string[] ModifiedProperties { get; set; }
        public Boolean Deleted { get; set; }
        public DateTime EntityCreationDate { get; set; }
        public DateTime EntityModificationDate { get; set; }
        public Guid EntityCreationUser { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            EntityVersion = 0;
        }
    }
}
