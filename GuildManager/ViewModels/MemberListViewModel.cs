using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuildManager.Models;
using GuildManager.Types;

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
        GuildMembers = new ObservableCollection<GuildMember>();
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

        members.ForEach(GuildMembers.Add);
    }

    [RelayCommand]
    public void Save()
    {

    }

    #endregion
}