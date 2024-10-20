using System;
using System.Collections.Generic;

namespace MungFramework.Algorithm
{
    public static class GetKthItemStatic
    {
        public static T GetKthItem<T>(List<T> list, int k, Func<T, T, bool> cmp)
        {
            if (list == null || list.Count == 0 || k < 0 || k >= list.Count)
            {
                throw new ArgumentException("Invalid input");
            }
            return QuickSelect(list, 0, list.Count - 1, k, cmp);
        }
        private static T QuickSelect<T>(List<T> list, int left, int right, int k, Func<T, T, bool> cmp)
        {
            if (left == right)
            {
                return list[left];
            }

            int pivotIndex = Partition(list, left, right, cmp);

            if (k == pivotIndex)
            {
                return list[k];
            }
            else if (k < pivotIndex)
            {
                return QuickSelect(list, left, pivotIndex - 1, k, cmp);
            }
            else
            {
                return QuickSelect(list, pivotIndex + 1, right, k, cmp);
            }
        }
        private static int Partition<T>(List<T> list, int left, int right, Func<T, T, bool> cmp)
        {
            T pivot = list[right];
            int i = left;

            for (int j = left; j < right; j++)
            {
                if (cmp(list[j], pivot))
                {
                    Swap(list, i, j);
                    i++;
                }
            }

            Swap(list, i, right);
            return i;
        }
        private static void Swap<T>(List<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
