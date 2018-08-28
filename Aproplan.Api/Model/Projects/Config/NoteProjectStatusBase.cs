using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Projects.Config
{

    public abstract class NoteProjectStatusBase : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsOnlyUsedByMeetingManager { get; set; }

        public string Color { get; set; }

        public bool IsTodo { get; set; }
        public bool IsDone { get; set; }
        public bool DoneAction { get; set; }

        /// <summary>
        /// Action is blocked
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// This action is IsBlocked action
        /// </summary>
        public bool? IsBlockedAction { get; set; }

        public const string OrangeColor = "FF6A00";
        public const string GreenColor = "007F46";
        public const string LightGreyColor = "808080";
        public const string RedColor = "FF0000";
        public const string BlueColor = "0026FF";
    }
}