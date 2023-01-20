using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    private BackgroundWorker SyncWorker { get; }
    
    public MemberListViewModel()
    {
        GuildMembers = new ObservableCollection<GuildMember>();
        SyncWorker = new BackgroundWorker();
        SyncWorker.DoWork += SyncWorker_DoWork;
        SyncWorker.RunWorkerCompleted += SyncWorker_Completed;
        SyncWorker.ProgressChanged += SyncWorker_ProgressChanged;
    }

    #region Commands

    [RelayCommand(CanExecute = nameof(CanSync))]
    private void Sync()
    {
        SyncWorker.RunWorkerAsync();
    }

    private bool CanSync()
    {
        return !SyncWorker.IsBusy;
    }

    [RelayCommand]
    public void Save()
    {

    }

    #endregion

    #region SyncWorker Methods

    private void SyncWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        var crawler = new GuildMemberCrawler("scania", "아이엠캔들");
        e.Result = crawler.GetMembers().Result;
    }

    private void SyncWorker_Completed(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is List<GuildMember> { Count: > 0 } members)
            members.ForEach(GuildMembers.Add);
    }

    private void SyncWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {

    }

    #endregion
}