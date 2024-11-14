using System;
using System.Collections.Generic;

namespace MungFramework.Extension.ComponentExtension
{
    public static class ListExtension
    {
        public static bool Empty<T>(this List<T> list) => list.Count == 0;


        public static List<T> Init<T>(this List<T> list, int n, T val)
        {
            list.Clear();
            for (int i = 0; i < n; i++)
            {
                T tmp = val;
                list.Add(tmp);
            }
            return list;
        }
        public static List<T> Init<T>(this List<T> list, int n, Func<int, T> init)
        {
            list.Clear();
            for (int i = 0; i < n; i++)
            {
                list.Add(init(i));
            }
            return list;
        }
        public static T FirstOrTarget<T>(this List<T> list, T targetT)
        {
            if (list.Count == 0)
            {
                return targetT;
            }
            return list[0];
        }
    }
}
