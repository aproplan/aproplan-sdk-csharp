using System;

namespace Aproplan.Api.Model.IdentificationFiles
{
    public class Signature : Entity
    {
        public string DisplayName
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string Role
        {
            get; set;
        }

        public string Company
        {
            get; set;
        }

        // same format as other signature of the user
        public string SignatureDrawing
        {
            get; set;
        }

        // Date where the drawing has been made
        public DateTime? SignatureDate
        {
            get; set;
        }

        public Guid? UserId
        {
            get; set;
        }
    }
}
