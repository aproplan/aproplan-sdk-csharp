using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects
{
    public enum FolderType
    {
        Custom = 0, //means normal folder created by the users
        Photo = 1,  // system folder created to storage the photo
        Report = 2  // system folder created to storage the report
    }
    public class Folder: Entity
    {
        public string Name { get; set; }
        public Guid? ParentFolderId
        {
            get;
            set;
        }
        public Project Project { get; set; }

        public FolderType FolderType { get; set; }

        public string ImageUrl { get; set; }

        public int? DisplayOrder { get; set; }

        public int PlanNumber { get; set; }
        public List<Folder> Children { get; set; }
        public bool IsPublic
        {
            get;
            set;
        }
        public User Creator { get; set; }
    }
}
