using System;
using System.Collections.Generic;

namespace MungFramework.Extension.ComponentExtension
{
    public static class ListExtension
    {
        public enum TraversalOrder
        {
            先序遍历,
            后序遍历,
        }

        public static bool Empty<T>(this List<T> list) => list.Count == 0;

        public static IEnumerable<T> Traversal<T>(this List<T> list, TraversalOrder order)
        {
            if (order == TraversalOrder.先序遍历)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    yield return list[i];
                }
            }
            else
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    yield return list[i];
                }
            }
        }

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

        public static T GetRandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
