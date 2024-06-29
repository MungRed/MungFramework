using System.Collections.Generic;

namespace MungFramework.ComponentExtend
{
    public static class ListExtend
    {
        public static bool Empty<T>(this List<T> list)=>list.Count == 0;
    }
}
