using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.IdentificationFiles
{
    public class Language : Entity
    {
        public string Code
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string TranslationCode
        {
            get;
            set;
        }

        public string TranslatedName
        {
            get;
            set;
        }
    }
}
