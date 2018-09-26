using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{

    public abstract partial class CellBase : Entity
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        //AP-11174: Add IsDisabled to CellBases
        public bool IsDisabled
        {
            get;
            set;
        }
    }
}
