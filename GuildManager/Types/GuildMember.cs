﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace GuildManager.Types;

public class GuildMember : ObservableObject
{
    public string Nickname { get; set; }

    public string Job { get; set; }

    public int Level { get; set; }

    public int? Murung { get; set; }

    public int LastActivityDay { get; set; }

    public Position Position { get; set; }

    public GuildMember()
    {
        Nickname = Job = string.Empty;
        Level = 0;
        Murung = null;
        LastActivityDay = 1;
        Position = Position.Rest;
    }

    public GuildMember(string nickname, string job, int level, int? murung, int lastActivity, Position position)
    {
        Nickname = nickname;
        Job = job;
        Level = level;
        Murung = murung;
        LastActivityDay = lastActivity;
        Position = position;
    }

    public GuildMember(GuildMember prevGuildMember)
    {
        Nickname = prevGuildMember.Nickname;
        Job = prevGuildMember.Job;
        Level = prevGuildMember.Level;
        Murung = prevGuildMember.Murung;
        LastActivityDay = prevGuildMember.LastActivityDay;
        Position = prevGuildMember.Position;
    }
}