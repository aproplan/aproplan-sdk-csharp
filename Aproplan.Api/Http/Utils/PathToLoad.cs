using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public class PathToLoad
    {
        #region Properties
        public IEnumerable<string> PropertiesPath { get { return _propertiesPath; } }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Join(",", _propertiesPath);
        }

        #endregion


        #region Constructors
        public PathToLoad(string listPath)
        {
            string[] paths = null;
            if (!String.IsNullOrEmpty(listPath)) {
                paths = listPath.Split(',');
            }
            if (paths != null)
                _propertiesPath.AddRange(paths);

        }

        public PathToLoad(IEnumerable<string> listPath)
        {
            if(listPath != null)
                _propertiesPath.AddRange(listPath);
        }
        #endregion

        #region Private members

        private List<string> _propertiesPath = new List<string>();

        #endregion
    }
}
