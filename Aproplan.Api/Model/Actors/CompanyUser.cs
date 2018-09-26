using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public partial class CompanyUser : Entity
    {
        public bool IsOwner
        {
            get;
            set;
        }

        public bool IsManager
        {
            get;
            set;
        }

        public ManagedCompany Company
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get; set;
        }
    }
}
