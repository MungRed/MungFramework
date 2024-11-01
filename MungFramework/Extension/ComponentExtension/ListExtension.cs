using System.Collections.Generic;

namespace MungFramework.Extension.ComponentExtension
{
    public static class ListExtension
    {
        public static bool Empty<T>(this List<T> list) => list.Count == 0;
    }
}
