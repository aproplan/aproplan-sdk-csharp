using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Services
{
    public abstract class BaseService
    {
        #region Properties

        protected IApiRequest Requester { get; private set; }

        #endregion

        #region Constructors

        protected BaseService(IApiRequest request)
        {
            Requester = request;
        }

        #endregion

        #region Private members
        
        #endregion
    }
}
