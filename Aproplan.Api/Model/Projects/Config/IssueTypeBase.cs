﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{
    public abstract class IssueTypeBase : Entity
    {
        public string Code { get; set; }

        public string Description { get; set; }

    }
}
