using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public class FormTemplateCountry : Entity
    {
        public Country Country
        {
            get;
            set;
        }

        public FormTemplate FormTemplate
        {
            get;
            set;
        }
    }
}
