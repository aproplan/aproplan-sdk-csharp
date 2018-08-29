using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.AccessRights
{
    public enum AccessRightDefinition : long
    {
        None = 0,
        Edit = 1,
        Config = 2,
        AddFolder = 4,
        UploadDoc = 8,
        DownloadDoc = 16,
        AddPoint = 32,
        EditPoint = 64,
        DeletePoint = 128,
        EditPointStatus = 256,
        AddComment = 512,
        AddDoc = 1024,
        GenerateReport = 2048,
        CreateNextMeeting = 4096,
        DeleteComment = 8192,
        ArchiveComment = 16384,
        ArchiveDoc = 32768,
        DeleteFolder = 65536,
        EditFolder = 131072,
        EditAllFolder = 262144,
        EditDoc = 524288,
        DeleteDoc = 1048576,
        EditAllDoc = 2097152,
        AddMeeting = 4194304,
        EditAllList = 8388608,
        EditAllPoint = 16777216,
        SkipFolderVisibility = 33554432,
        EditPointIssueType = 67108864,
        EditPointInCharge = 134217728,
        AddVersion = 268435456,
        DeleteVersion = 536870912,
        MoveDoc = 1073741824,
        EditContact = 2147483648,
        RemoveContact = 4294967296,
        EditAllContact = 8589934592,
        ViewOnlyPointInCharge = 17179869184,
        AddPointDocument = 34359738368,
        DeletePointDocument = 68719476736,
        ViewDashboard = 137438953472,
        ViewParticipant = 274877906944,
        ViewContactStats = 549755813888,
    }
}
