using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http
{
    /// <summary>
    /// Class used to represent the output of a sync
    /// </summary>
    /// <typeparam name="T">Kind of entity to sync</typeparam>
    public class SyncResult<T>
    {
        /// <summary>
        /// Token used to get the next batch of sync
        /// </summary>
        public String ContinuationToken;

        /// <summary>
        /// Entities synced 
        /// </summary>
        public List<T> Data; 
    }
}
