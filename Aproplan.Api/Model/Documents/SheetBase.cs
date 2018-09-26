using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Documents
{
    public abstract partial class SheetBase : Entity
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int ZoomLevelNumber { get; set; }

        public int TileSize { get; set; }

        public int BigThumbWidth { get; set; }

        public int BigThumbHeight { get; set; }

        public int SmallThumbWidth { get; set; }

        public int SmallThumbHeight { get; set; }

        public string TilesPath { get; set; }
    }
}
