using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.IdentificationFiles
{
    public class Phone : Entity
    {

        public string Number
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public InfoType InfoType
        {
            get;
            set;
        }
    }
}
