using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuildManager.Models;
using GuildManager.Models.Controllers;
using GuildManager.Models.Providers;
using GuildManager.Utils;

namespace GuildManager.ViewModels;

public partial class MemberListViewModel : ObservableRecipient
{
    #region Properties

    #region public ObservableCollection<GuildMember> GuildMembers

    private ObservableCollection<GuildMember> _guildMembers = null!;

    public ObservableCollection<GuildMember> GuildMembers
    {
        get => _guildMembers;
        set => SetProperty(ref _guildMembers, value);
    }

    #endregion

    #region public int Progress

    private int _progress;

    public int Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    #endregion

    #endregion

    public MemberListViewModel()
    {
        var repo = new GuildMemberRepository();
        GuildMembers = repo.GetGuildMembers(new List<(string, string)>()).ToObservableCollection();
    }

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Sync()
    {
        var progressHandler = new Progress<int>(value =>
        {
            Progress = value;
        });

        var progress = progressHandler as IProgress<int>;

        var members = await Task.Run(async () =>
        {
            var crawler = new GuildMemberCrawler("scania", "아이엠캔들");
            var members = await crawler.GetMembers();

            var memberCount = members.Count;
            var murungList = members.Select(async (x, i) =>
                {
                    var murung = await GuildMemberCrawler.CrawlMurung(x.Nickname);
                    progress.Report(100 * (i + 1) / memberCount);

                    return murung;
                }).Select(x => x.Result).ToList();

            return members.Select((x, i) =>
            {
                x.Murung = murungList[i];
                return x;
            }).ToList();
        });

        members.ForEach(x =>
        {
            var member = GuildMembers.FirstOrDefault(g => g.Nickname == x.Nickname);
            if (member != null)
            {
                member.Level = x.Level;
                member.Job = x.Job;
                member.Murung = x.Murung;
                member.LastActivityDay = x.LastActivityDay;
            }
            else
                GuildMembers.Add(x);
        });

        MessageBox.Show("길드원 목록 갱신 완료", "GuildManager", MessageBoxButton.OK);
    }

    [RelayCommand]
    public void Save()
    {
        var repo = new GuildMemberRepository();
        repo.UpsertGuildMembers(GuildMembers.ToList());

        MessageBox.Show("길드원 목록 저장 완료", "GuildManager", MessageBoxButton.OK);
    }

    #endregion
}