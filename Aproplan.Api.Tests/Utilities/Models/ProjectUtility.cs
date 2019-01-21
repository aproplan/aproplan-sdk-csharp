using Aproplan.Api.Model.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities.Models
{
    public class ProjectUtility
    {
        public static List<Project> GetFakeSimpleProjects()
        {
            List<Project> fakeProjects = new List<Project>
            {
                new Project{ Id = Guid.NewGuid(), Code = "PR1", Name = "Project 1" },
                new Project{ Id = Guid.NewGuid(), Code = "PR2", Name = "Project 2" },
                new Project{ Id = Guid.NewGuid(), Code = "PR3", Name = "Project 3" },
                new Project{ Id = Guid.NewGuid(), Code = "PR4", Name = "Project 4" },
                new Project{ Id = Guid.NewGuid(), Code = "PR5", Name = "Project 5" }
            };
            return fakeProjects;
        }
    }
}
