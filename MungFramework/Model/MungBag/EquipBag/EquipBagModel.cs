using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MungFramework.Model.MungBag.EquipBag
{
    /// <summary>
    /// 装备背包模型
    /// 每个装备单独分配一个id
    /// 储存每个装备单的的id和装备id和被装备的角色id
    /// </summary>
    [Serializable]
    public class EquipBagModel<T_BagItem> : ListBagModel<T_BagItem> where T_BagItem : EquipBagItem, new()
    {
        /// <summary>
        /// 向背包中添加一个道具
        /// </summary>
        [Button]
        public T_BagItem AddEquip(string itemId)
        {
            if (itemId == null)
            {
                return null;
            }

            T_BagItem item = new()
            {
                EquipId = itemId,
                EquipGuid = Guid.NewGuid().ToString(),
                OwnerId = "",
                OwnerGuid = ""
            };

            InsertEquip(item);
            return item;
        }

        /// <summary>
        /// 获得道具
        /// </summary>
        public T_BagItem GetEquip(string equipId, string euipGuid)
        {
            if (equipId == null)
            {
                return null;
            }
            return ItemList.Find(x => x.EquipId == equipId && x.EquipGuid == euipGuid);
        }


        /// <summary>
        /// 根据拥有者获得道具背包数据
        /// </summary>
        public IEnumerable<T_BagItem> GetEquipByOwner(string ownerId)
        {
            return ItemList.Where(x => x.OwnerId == ownerId);
        }
        public IEnumerable<T_BagItem> GetEquipByOwner(string ownerId, string ownerGuid)
        {
            return ItemList.Where(x => x.OwnerId == ownerId && x.OwnerGuid == ownerGuid);
        }

        /// <summary>
        /// 删除道具，返回是否成功
        /// </summary>
        public bool RemoveEquip(string equipId, string equipGuid)
        {
            if (equipId == null)
            {
                return false;
            }
            return ItemList.RemoveAll(x => x.EquipId == equipId && x.EquipGuid == equipGuid) > 0;
        }

        /// <summary>
        /// 改变道具的拥有者
        /// </summary>
        public bool ChangeEquipOwner(string equipId, string equipGuid, string ownerId, string ownerGuid = "")
        {
            if (equipId == null)
            {
                return false;
            }

            var item = ItemList.Find(x => x.EquipId == equipId && x.EquipGuid == equipGuid);
            if (item != null)
            {
                item.OwnerId = ownerId;
                item.OwnerGuid = ownerGuid;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得道具的拥有者
        /// </summary>
        public (string ownerId, string ownerGuid) GetEquipOwner(string equipId, string guid)
        {
            if (equipId == null)
            {
                return ("", "");
            }
            var item = ItemList.Find(x => x.EquipId == equipId && x.EquipGuid == guid);
            if (item != null)
            {
                return (item.OwnerId, item.OwnerGuid);
            }
            return ("", "");
        }

        /// <summary>
        /// 获得道具数量
        /// </summary>
        public int GetEquipCount(string equipId)
        {
            if (equipId == null)
            {
                return 0;
            }
            return ItemList.Count(x => x.EquipId == equipId);
        }

        /// <summary>
        /// 判断某个道具是否有指定数量个
        /// </summary>
        public bool CheckEquipCount(string equipId, int itemCount)
        {
            if (equipId == null)
            {
                return false;
            }
            return ItemList.Count(x => x.EquipId == equipId) >= itemCount;
        }


        private void InsertEquip(T_BagItem item)
        {
            ItemList.Add(item);
            for (int i = ItemList.Count - 1; i >= 1; i--)
            {
                if (ItemList[i].EquipId.CompareTo(ItemList[i - 1].EquipId) < 0)
                {
                    Algorithm.Math.Swap(ItemList[i - 1], ItemList[i]);
                }
            }
        }

#if UNITY_EDITOR
        [Button]
        private void SortItemList()
        {
            ItemList.Sort((a, b) => string.Compare(a.EquipId, b.EquipId));
        }
#endif
    }
}
