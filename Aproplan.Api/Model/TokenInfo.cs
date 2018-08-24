using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model
{
    public class TokenInfo
    {
        public Guid Token;
        public DateTime ValidityLimit;
        public DateTime ValidityStart;
    }
}
