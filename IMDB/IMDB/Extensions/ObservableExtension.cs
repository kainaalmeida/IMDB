using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IMDB.Extensions
{
    public static class ObservableExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
    }
}
