using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MungFramework.ComponentExtend
{
    public static class ListExtend
    {
        public static bool Empty<T>(this List<T> list)=>list.Count == 0;
    }
}
