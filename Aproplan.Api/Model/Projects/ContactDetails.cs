using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.IdentificationFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects
{
    public class ContactDetails
    {
        public string City
        {
            get;
            set;
        }

        public string Company
        {
            get;
            set;
        }

        public Country Country
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string Role
        {
            get;
            set;
        }

        public string Street
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public Guid ProjectId { get; set; }

        public Project Project
        {
            get { return _project; }
            set
            {
                if (_project != value)
                {
                    _project = value;
                    ProjectId = _project.Id;
                }
            }
        }

        public string VAT
        {
            get;
            set;
        }

        public bool IsDisabled
        {
            get;
            set;
        }

        public bool IsInvited
        {
            get;
            set;
        }

        //Calculated
        public Guid PayerMasterId
        {
            get;
            set;
        }

        //Calculated
        public Guid InviterId
        {
            get;
            set;
        }


        private Project _project;
    }
}
