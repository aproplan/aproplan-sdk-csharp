using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public enum FormItemType
    {
        /// <summary>
        /// It means the answer to the question must be a free text
        /// </summary>
        FreeText,
        /// <summary>
        /// It means the answer to the question must be free number
        /// </summary>
        FreeNumber,
        /// <summary>
        /// It means the answer to the question must be a date
        /// </summary>
        DateTime,
        /// <summary>
        /// It means the answer must be one of those proposed and the kind of the value is a string
        /// </summary>
        PredefinedString,
        /// <summary>
        /// It means the answer must be one of those proposed and the kind of the value is a number
        /// </summary>
        PredefinedNumber,
        /// <summary>
        /// It means the answer must be one or several of those proposed.
        /// </summary>
        MultipleChoice
    }
}
