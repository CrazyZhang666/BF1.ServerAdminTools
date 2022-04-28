namespace BF1.ServerAdminTools.Common.Extension;

public static class ObservableExtension
{
    /// <summary>
    /// 整理数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
    {
        var sortedList = collection.OrderBy(x => x).ToList();
        for (int newIndex = 0; newIndex < sortedList.Count(); newIndex++)
        {
            var oldIndex = collection.IndexOf(sortedList[newIndex]);
            if (oldIndex != newIndex)
            {
                collection.Move(oldIndex, newIndex);
            }
        }
    }
}
