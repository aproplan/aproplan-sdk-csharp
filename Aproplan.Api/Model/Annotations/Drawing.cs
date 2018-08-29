using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class Drawing : Entity
    {
        public Guid NoteCommentId
        {
            get;
            set;
        }

        public Guid NoteDocumentId
        {
            get;
            set;
        }


        public int Left
        {
            get;
            set;
        }

        public int Top
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public double Scale
        {
            get;
            set;
        }

        public double RenderCenterX
        {
            get;
            set;
        }

        public double RenderCenterY
        {
            get;
            set;
        }

        public double? PrintScale
        {
            get
            {
                if (_printScale == 0)
                {
                    return null;
                }
                return _printScale;
            }
            set
            {
                _printScale = value;
            }
        }
        private double? _printScale;

        public double? PrintRenderCenterX
        {
            get;
            set;
        }

        public double? PrintRenderCenterY
        {
            get;
            set;
        }

        public bool HasRectangle
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public string DrawingShapesXml
        {
            get;
            set;
        }
    }
}
