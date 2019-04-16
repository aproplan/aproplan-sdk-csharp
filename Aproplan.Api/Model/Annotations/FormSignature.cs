using System;

namespace Aproplan.Api.Model.Annotations
{
    public class FormSignature : Entity
    {
        public Guid? FormId
        {
            get;
            set;
        }

        public Guid SignatureId
        {
            get;
            set;
        }
    }
}
