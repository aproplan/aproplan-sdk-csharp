using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public abstract partial class Party : Entity
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

        public Language Language
        {
            get;
            set;
        }

        public string LanguageCode
        {
            get;
            set;
        }

        public Country Country
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public List<Phone> Phones
        {
            get;
            set;
        }

        public List<EmailInfo> Emails
        {
            get;
            set;
        }
    }
}
