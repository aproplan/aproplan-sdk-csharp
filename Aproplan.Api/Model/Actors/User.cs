using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public partial class User : Entity
    {
        public string Alias { get; set; }
        public string DisplayName { get; set; }
        public string TimeZoneName { get; set; }
        public string DefaultEmail { get; set; }
        public string LanguageCode { get; set; }
        public bool IsDailyMailReceiver { get; set; }
        public bool IsDailyMyTasksReceiver { get; set; }

        public Person Person { get; set; }

        public ManagedCompany Company { get; set; }


        public Profession Profession { get; set; }

        public string CompanyName { get; set; }

        public string Role { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public DateTime? ActivationDate { get;  set; }

        public DateTime? FirstConnectionDate { get; set; }
    }
}
