using System.ComponentModel;

namespace GuildManager.Types;

public enum Position
{
    [Description("마스터")]
    Master = 1,

    [Description("부마스터")]
    SubMaster = 2,

    [Description("길드원")]
    Member = 3,

    [Description("플래그 부캐")]
    Flag = 4,

    [Description("개인사정")]
    Rest = 5,

    [Description("군인")]
    Soldier = 6,

    [Description("미정")]
    Unknown = 7
}