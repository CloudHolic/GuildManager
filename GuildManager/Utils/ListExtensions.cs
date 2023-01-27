using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GuildManager.Utils;

public static class ListExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list) => new(list);
}