using System.Collections.Generic;
using System.Linq;

namespace ControlsLibrary.Infrastructure
{
    public static class CollectionUtils
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> self) =>
            self.SelectMany(subCollection => subCollection);
    }
}