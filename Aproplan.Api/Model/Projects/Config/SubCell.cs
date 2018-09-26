using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public partial class SubCell : CellBase
    {
        public ParentCell ParentCell { get; set; }
    }
}
