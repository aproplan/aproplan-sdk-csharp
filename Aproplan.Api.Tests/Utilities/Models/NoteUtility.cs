using Aproplan.Api.Model.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities.Models
{
    public class NoteUtility
    {
        public static List<Note> GetFakeSimpleNotes()
        {
            List<Note> fakePoints = new List<Note>
            {
                new Note{ Id = Guid.NewGuid(), Code = "PR1", Subject = "Point 1" },
                new Note{ Id = Guid.NewGuid(), Code = "PR2", Subject = "Point 2" },
                new Note{ Id = Guid.NewGuid(), Code = "PR3", Subject = "Point 3" },
                new Note{ Id = Guid.NewGuid(), Code = "PR4", Subject = "Point 4" },
                new Note{ Id = Guid.NewGuid(), Code = "PR5", Subject = "Point 5" }
            };
            return fakePoints;
        }
    }
}
