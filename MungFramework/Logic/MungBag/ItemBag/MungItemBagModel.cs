using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace MungFramework.Logic.MungBag.ItemBag
{
    /// <summary>
    /// 道具背包
    /// 背包中储存拥有的道具以及数量
    /// </summary>
    [Serializable]
    public class MungItemBagModel<T_BagItem> : MungFramework.Model.Model where T_BagItem : MungItemBagItem,new()
    {
        [SerializeField]
        private List<T_BagItem> itemList = new();


        public ReadOnlyCollection<T_BagItem> GetItemList()
        {
            return itemList.AsReadOnly();
        }

        public bool HaveItem(string itemId)
        {
            return FindItem(itemId) != null;
        }

        /// <summary>
        /// 更新道具数量
        /// </summary>
        public T_BagItem UpdateItemCount(string itemId, int itemCount)
        {
            var find = FindItem(itemId);
            if (find != null)
            {
                find.ItemCount = itemCount;
                return find;
            }
            else
            {
                T_BagItem item = new()
                {
                    ItemId = itemId,
                    ItemCount = itemCount
                };
                InsertItem(item);
                return item;
            }
        }

        /// <summary>
        /// 向背包中添加一个道具
        /// </summary>
        [Button]
        public T_BagItem AddItem(string itemId, int itemCount)
        {
            var find = FindItem(itemId);
            if (find != null)
            {
                find.ItemCount += itemCount;
                return find;
            }
            else
            {
                T_BagItem item = new()
                {
                    ItemId = itemId,
                    ItemCount = itemCount
                };
                InsertItem(item);
                return item;
            }
        }

        /// <summary>
        /// 获得道具数量
        /// </summary>
        public int GetItemCount(string itemId)
        {
            var find = FindItem(itemId);
            if (find != null)
            {
                return find.ItemCount;
            }
            return 0;
        }

        /// <summary>
        /// 判断某个道具是否有指定数量个
        /// </summary>
        public bool CheckItemCount(string itemId, int itemCount)
        {
            var find = FindItem(itemId);
            if (find != null)
            {
                return find.ItemCount >= itemCount;
            }
            return false;
        }

        /// <summary>
        /// 减少道具数量，返回是否成功
        /// </summary>
        public bool RemoveItem(string itemId, int itemCount)
        {
            var find = FindItem(itemId);
            if (find != null)
            {
                if (find.ItemCount < itemCount)
                {
                    return false;
                }
                find.ItemCount -= itemCount;
                return true;
            }
            return false;
        }


        private void InsertItem(T_BagItem item)
        {
            itemList.Add(item);
            for (int i = itemList.Count - 1; i >= 1; i--)
            {
                if (itemList[i].ItemId.CompareTo(itemList[i - 1].ItemId) < 0)
                {
                    var temp = itemList[i];
                    itemList[i] = itemList[i - 1];
                    itemList[i - 1] = temp;
                }
            }
        }
        private T_BagItem FindItem(string itemId)
        {
            //二分查找
            int left = 0;
            int right = itemList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                int compare = string.Compare(itemId, itemList[mid].ItemId);
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
            itemList.Sort((a, b) => string.Compare(a.ItemId, b.ItemId));
        }

    }
}
