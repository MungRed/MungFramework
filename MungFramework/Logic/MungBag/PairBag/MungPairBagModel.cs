using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace MungFramework.Logic.MungBag.PairBag
{
    [Serializable]
    public class MungPairBagModel<T_BagItem> where T_BagItem : MungPairBagItem, new()
    {
        [SerializeField]
        private List<T_BagItem> itemList = new();

        public ReadOnlyCollection<T_BagItem> GetItemList()
        {
            return itemList.AsReadOnly();
        }

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
            itemList.Add(item);
            for (int i = itemList.Count - 1; i >= 1; i--)
            {
                if (itemList[i].Key.CompareTo(itemList[i - 1].Key) < 0)
                {
                    var temp = itemList[i];
                    itemList[i] = itemList[i - 1];
                    itemList[i - 1] = temp;
                }
            }
        }

        private T_BagItem FindItem(string key)
        {
            //二分查找
            int left = 0;
            int right = itemList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                int compare = string.Compare(key, itemList[mid].Key);
                if (compare == 0)
                {
                    return itemList[mid];
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

        [Button]
        private void SortItemList()
        {
            itemList.Sort((a, b) => string.Compare(a.Key, b.Key));
        }
    }
}
