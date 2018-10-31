using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aproplan.Api.Http
{
    /// <summary>
    /// Entity used to model the output of an HTTP call
    /// </summary>
    public class HttpResponse
    {
        public String Data;
        public WebHeaderCollection Headers; 
    }
}
