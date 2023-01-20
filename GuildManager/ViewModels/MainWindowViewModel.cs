using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GuildManager.Events;

namespace GuildManager.ViewModels;

public class MainWindowViewModel : ObservableRecipient, IRecipient<TabChangedEvent>
{
    #region Properties

    #region public int CurrentTab

    private int _currentTab;

    public int CurrentTab
    {
        get => _currentTab;
        set => SetProperty(ref _currentTab, value);
    }

    #endregion

    #endregion

    public MainWindowViewModel()
    {
        CurrentTab = 0;
        WeakReferenceMessenger.Default.Register(this);
    }

    #region Event handler

    public void Receive(TabChangedEvent message)
    {
        CurrentTab = message.TabIndex;
    }

    #endregion
}