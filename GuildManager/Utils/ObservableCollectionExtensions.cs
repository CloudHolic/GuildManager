using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GuildManager.Utils;

public static class ObservableCollectionExtensions
{
    public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
    {
        var items = collection.Where(condition).ToList();
        items.ForEach(x => collection.Remove(x));

        return items.Count;
    }
}