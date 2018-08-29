using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public class ApiException : Exception
    {
        #region Properties
        public string Method { get; private set; }

        public string Url { get; private set; }

        public string Code { get; private set; }

        public int ReturnCode { get; private set; }

        #endregion
        #region Constructors

        public ApiException(string message = null, string code = null, string id = null, string url = null, int returnCode = -1, string method = null, Exception innerException = null): base(message, innerException)
        {
            Code = code;
            ReturnCode = returnCode;
            Url = url;
            Method = method;
        }

        public ApiException(string message, Exception innerException): base(message, innerException)
        {

        }

        #endregion
    }
}
