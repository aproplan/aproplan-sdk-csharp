using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.IdentificationFiles
{
    public class EmailInfo: Entity
    {

        public string Email
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
