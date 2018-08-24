using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.IdentificationFiles
{
    public class Country : Entity
    {

        public string Name
        {
            get;
            set;
        }

        public string Iso
        {
            get;
            set;
        }

        public string Iso2
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }
    }
}
