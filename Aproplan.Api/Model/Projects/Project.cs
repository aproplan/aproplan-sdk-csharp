using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects
{
    public class Project: Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public string Address { get; set; }

        public decimal MaxStorageSize { get; set; }

        public decimal StorageSize { get; set; }

        public Country Country { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }


        public string IntegratorClient { get; set; }

        public bool IsActive { get; set; }

        public string LogoUrl { get; set; }
        public User Creator { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public Guid PhotoFolderId { get; set; }
    }
}
