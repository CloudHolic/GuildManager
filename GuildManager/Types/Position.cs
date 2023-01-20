using System.ComponentModel;

namespace GuildManager.Types;

public enum Position
{
    [Description("마스터")]
    Master,

    [Description("부마스터")]
    SubMaster,

    [Description("길드원")]
    Member,

    [Description("플래그 부캐")]
    Flag,

    [Description("개인사정")]
    Rest,

    [Description("군인")]
    Soldier
}