using Sirenix.OdinInspector;
using System;

namespace MungFramework.Model.MungBag.ItemBag
{
    /// <summary>
    /// 道具背包
    /// 背包中储存拥有的道具以及数量
    /// </summary>
    [Serializable]
    public class ItemBagModel<T_BagItem> : ListBagModel<T_BagItem> where T_BagItem : ItemBagItem, new()
    {
        public bool HaveItem(string itemId)
        {
            return FindItem(itemId) != null;
        }

        /// <summary>
        /// 更新道具数量
        /// </summary>
        public T_BagItem UpdateItemCount(string itemId, int itemCount)
        {
            if (itemId == null)
            {
                return null;
            }

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
            if (itemId == null)
            {
                return null;
            }

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
            if (itemId == null)
            {
                return 0;
            }

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
            if (itemId == null)
            {
                return false;
            }

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
            if (itemId == null)
            {
                return false;
            }

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
            ItemList.Add(item);
            for (int i = ItemList.Count - 1; i >= 1; i--)
            {
                if (ItemList[i].ItemId.CompareTo(ItemList[i - 1].ItemId) < 0)
                {
                    var temp = ItemList[i];
                    ItemList[i] = ItemList[i - 1];
                    ItemList[i - 1] = temp;
                }
            }
        }
        private T_BagItem FindItem(string itemId)
        {
            //二分查找
            int left = 0;
            int right = ItemList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                int compare = string.Compare(itemId, ItemList[mid].ItemId);
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
            ItemList.Sort((a, b) => string.Compare(a.ItemId, b.ItemId));
        }
#endif
    }
}
