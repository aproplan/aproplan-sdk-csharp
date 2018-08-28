using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public class ParentCell : CellBase
    {
        public Guid ProjectId { get; set; }

        public List<SubCell> SubCells { get; set; }
    }
}
