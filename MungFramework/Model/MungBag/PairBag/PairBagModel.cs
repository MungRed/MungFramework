using Sirenix.OdinInspector;
using System;
namespace MungFramework.Model.MungBag.PairBag
{
    [Serializable]
    public class PairBagModel<T_BagItem> : ListBagModel<T_BagItem> where T_BagItem : PairBagItem, new()
    {
        [Button]
        public T_BagItem AddItem(string key)
        {
            if (key == null)
            {
                return null;
            }

            var find = FindItem(key);
            if (find != null)
            {
                return find;
            }
            else
            {
                T_BagItem item = new()
                {
                    Key = key
                };
                InsertItem(item);
                return item;
            }
        }

        public T_BagItem GetItem(string key)
        {
            if (key == null)
            {
                return null;
            }
            return FindItem(key);
        }

        public bool HaveItem(string key)
        {
            if (key == null)
            {
                return false;
            }
            return FindItem(key) != null;
        }

        private void InsertItem(T_BagItem item)
        {
            ItemList.Add(item);
            for (int i = ItemList.Count - 1; i >= 1; i--)
            {
                if (ItemList[i].Key.CompareTo(ItemList[i - 1].Key) < 0)
                {
                    var temp = ItemList[i];
                    ItemList[i] = ItemList[i - 1];
                    ItemList[i - 1] = temp;
                }
            }
        }

        private T_BagItem FindItem(string key)
        {
            //二分查找
            int left = 0;
            int right = ItemList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                int compare = string.Compare(key, ItemList[mid].Key);
                if (compare == 0)
                {
                    return ItemList[mid];
                }
                else if (compare < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        [Button]
        private void SortItemList()
        {
            ItemList.Sort((a, b) => string.Compare(a.Key, b.Key));
        }
#endif
    }
}
