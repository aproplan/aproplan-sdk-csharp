using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Actors
{
    public partial class ManagedCompany : Party
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
