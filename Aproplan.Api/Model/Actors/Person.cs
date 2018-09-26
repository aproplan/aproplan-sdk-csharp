using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public partial class Person : Party
    {
        public Language SpellCheckLanguage
        {
            get;
            set;
        }
    }
}
