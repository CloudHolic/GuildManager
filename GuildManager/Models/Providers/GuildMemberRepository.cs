using System.Collections.Generic;
using System.Linq;
using Dapper;
using GuildManager.Models.Interfaces;

namespace GuildManager.Models.Providers;

public class GuildMemberRepository : SqliteBaseRepository, IGuildMemberRepository
{
    private string TableName => GetType().Name.Replace("Repository", "");

    public GuildMemberRepository()
    {
        CreateDatabase(
            "CREATE TABLE IF NOT EXISTS GuildMember " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "Nickname TEXT UNIQUE, Job TEXT, Level INT, " +
            "Murung INT, LastActivity INT, Position INT);");
    }

    public List<GuildMember> GetGuildMembers(List<(string, string)> queryList)
    {
        var query = $"SELECT * FROM {TableName}";
        var dp = new DynamicParameters();
        
        var param = string.Join(" AND ", queryList.Select(x => $"{x.Item1} = @{x.Item1}"));
        if (!string.IsNullOrEmpty(param))
            query += $" WHERE {param}";
        query += ";";

        queryList.ForEach(x => dp.Add($"@{x.Item1}", x.Item2));

        return GetMultipleQuery<GuildMember>(query, dp)?.ToList() ?? new List<GuildMember>();
    }
    public List<GuildMember> UpsertGuildMembers(List<GuildMember> guildMembers)
    {
        return guildMembers.Select(x =>
        {
            var id = ExecuteQuery(
                $"INSERT INTO {TableName} (Nickname, Job, Level, Murung, LastActivity, Position) " +
                "VALUES (@Nickname, @Job, @Level, @Murung, @LastActivityDay, @Position) " +
                "ON CONFLICT (Nickname) DO UPDATE SET Job=excluded.Job, Level=excluded.Level, " +
                "Murung=excluded.Murung, LastActivity=excluded.LastActivity, Position=excluded.Position;",
                new { x.Nickname, x.Job, x.Level, x.Murung, x.LastActivityDay, x.Position });

            return new GuildMember(x) { Id = x.Id == 0 ? id : x.Id };
        }).ToList();
    }

    public List<GuildMember> InsertGuildMembers(List<GuildMember> guildMembers)
    {
        return guildMembers.Select(x =>
        {
            var id = ExecuteQuery(
                $"INSERT INTO {TableName} (Nickname, Job, Level, Murung, LastActivity, Position) " +
                "VALUES (@Nickname, @Job, @Level, @Murung, @LastActivityDay, @Position);",
                new { x.Nickname, x.Job, x.Level, x.Murung, x.LastActivityDay, x.Position });

            return new GuildMember(x) { Id = id };
        }).ToList();
    }

    public List<int> UpdateGuildMembers(List<GuildMember> guildMembers)
    {
        return guildMembers.Select(x => ExecuteQuery(
            $"UPDATE {TableName} SET Nickname=@Nickname, Job=@Job, Level=@Level, " +
            "Murung=@Murung, LastActivity=@LastActivityDay, Position=@Position " +
            "WHERE Id=@Id;",
            new { x.Nickname, x.Job, x.Level, x.Murung, x.LastActivityDay, x.Position, x.Id }))
            .ToList();
    }

    public List<int> DeleteGuildMembers(List<GuildMember> guildMembers)
    {
        return guildMembers.Select(x => ExecuteQuery(
            $"DELETE FROM {TableName} WHERE Id = @Id",
            new { x.Id }))
            .ToList();
    }
}