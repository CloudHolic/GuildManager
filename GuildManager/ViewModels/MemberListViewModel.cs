using System.Collections.ObjectModel;
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

    #endregion
    
    public MemberListViewModel()
    {
        GuildMembers = new ObservableCollection<GuildMember>();
    }

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task Sync()
    {
        var crawler = new GuildMemberCrawler("scania", "아이엠캔들");
        var members = await crawler.GetMembers();
        members.ForEach(GuildMembers.Add);
    }

    [RelayCommand]
    public void Save()
    {

    }

    #endregion
}