using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.AccessRights
{
    public partial class ProjectAccessRight : AccessRightBase
    { 
        public bool CanConfig { get; set; }
        public bool CanAddFolder { get; set; }
        public bool CanUploadDoc { get; set; }
        public bool CanDownloadDoc { get; set; }
        public bool CanArchiveDoc { get; set; }
        public bool CanDeleteFolder { get; set; }
        public bool CanEditFolder { get; set; }
        public bool CanEditAllFolder { get; set; }
        public bool CanEditDoc { get; set; }
        public bool CanDeleteDoc { get; set; }
        public bool CanEditAllDoc { get; set; }
        public bool CanAddMeeting { get; set; }
        public bool CanEditAllList { get; set; }
        public bool CanSkipFolderVisibility { get; set; }
        public bool CanAddVersion { get; set; }
        public bool CanDeleteVersion { get; set; }
        public bool CanMoveDoc { get; set; }
        public bool CanEditContact { get; set; }
        public bool CanRemoveContact { get; set; }
        public bool CanEditAllContact { get; set; }
        public bool CanViewContactStats { get; set; }
    }
}
