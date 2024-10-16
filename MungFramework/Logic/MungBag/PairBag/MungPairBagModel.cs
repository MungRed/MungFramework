using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace MungFramework.Logic.MungBag.PairBag
{
    [Serializable]
    public class MungPairBagModel<BagItem> where BagItem : MungPairBagItem, new()
    {
        [SerializeField]
        private List<BagItem> itemList = new();

        public ReadOnlyCollection<BagItem> GetItemList()
        {
            return itemList.AsReadOnly();
        }

        [Button("AddItem")]
        public BagItem AddItem(string key)
        {
            var find = FindItem(key);
            if (find != null)
            {
                return find;
            }
            else
            {
                BagItem item = new()
                {
                    Key = key
                };
                InsertItem(item);
                return item;
            }
        }

        public BagItem GetItem(string key)
        {
            return FindItem(key);
        }

        public bool HaveItem(string key)
        {
            return FindItem(key) != null;
        }

        private void InsertItem(BagItem item)
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

        private BagItem FindItem(string key)
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

        [Button("SortItemList")]
        private void SortItemList()
        {
            itemList.Sort((a, b) => string.Compare(a.Key, b.Key));
        }
    }
}
