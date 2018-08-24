using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public class ManagedCompany : Party
    {

        public User CompanyOwner
        {
            get;
            set;
        }

        public List<CompanyUser> CompanyUsers
        {
            get;
            set;
        }
    }
}
