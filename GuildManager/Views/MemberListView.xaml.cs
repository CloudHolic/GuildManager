using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GuildManager.Views;

/// <summary>
/// Interaction logic for MemberListView.xaml
/// </summary>
public partial class MemberListView : UserControl
{
    private GridViewColumnHeader? LastHeaderClicked { get; set; }

    private ListSortDirection LastDirection { get; set; } = ListSortDirection.Ascending;

    public MemberListView()
    {
        InitializeComponent();
    }

    private void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not GridViewColumnHeader headerClicked)
            return;

        if (headerClicked.Role == GridViewColumnHeaderRole.Padding)
            return;

        var direction = (LastHeaderClicked, LastDirection) switch
        {
            var (x, _) when x != headerClicked => ListSortDirection.Ascending,
            (_, ListSortDirection.Ascending) => ListSortDirection.Descending,
            _ => ListSortDirection.Ascending
        };
            
        var sortBy = (headerClicked.Tag as string) switch
        {
            "닉네임" => "Nickname",
            "직업" => "Job",
            "레벨" => "Level",
            "무릉" => "Murung",
            "마지막 활동일" => "LastActivityDay",
            "직위" => "Position",
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(sortBy))
            return;

        InternalSort(sortBy, direction);

        LastDirection = direction;
        LastHeaderClicked = headerClicked;
    }

    private void InternalSort(string sortBy, ListSortDirection direction)
    {
        var dataView = CollectionViewSource.GetDefaultView(MemberList.ItemsSource);

        dataView.SortDescriptions.Clear();
        dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
        dataView.Refresh();
    }
}