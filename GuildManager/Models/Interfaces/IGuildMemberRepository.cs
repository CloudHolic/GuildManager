using System.Collections.Generic;

namespace GuildManager.Models.Interfaces;

public interface IGuildMemberRepository
{
    List<GuildMember> GetGuildMembers(List<(string, string)> queryList);

    List<GuildMember> InsertGuildMembers(List<GuildMember> guildMembers);

    List<int> UpdateGuildMembers(List<GuildMember> guildMembers);

    List<int> DeleteGuildMembers(List<string> nicknames);
}