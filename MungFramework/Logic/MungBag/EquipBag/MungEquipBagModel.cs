using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.MungBag.EquipBag
{
    /// <summary>
    /// 装备背包模型
    /// 每个装备单独分配一个id
    /// 储存每个装备单的的id和装备id和被装备的角色id
    /// </summary>
    [Serializable]
    public class MungEquipBagModel<T_BagItem> : MungFramework.Model.Model where T_BagItem:MungEquipBagItem,new()
    {
        [SerializeField]
        private List<T_BagItem> itemList = new();


        public ReadOnlyCollection<T_BagItem> GetItemList()
        {
            return itemList.AsReadOnly();
        }


        /// <summary>
        /// 向背包中添加一个道具
        /// </summary>
        [Button]
        public T_BagItem AddItem(string itemId)
        {
            T_BagItem item = new()
            {
                EquipId = itemId,
                EquipGuid = Guid.NewGuid().ToString(),
                OwnerId = "",
                OwnerGuid = ""
            };

            InsertItem(item);
            return item;
        }

        /// <summary>
        /// 获得道具
        /// </summary>
        public T_BagItem GetItem(string equipId, string euipGuid)
        {
            return itemList.Find(x => x.EquipId == equipId && x.EquipGuid == euipGuid);
        }


        /// <summary>
        /// 根据拥有者获得道具背包数据
        /// </summary>
        public IEnumerable<T_BagItem> GetItemByOwner(string ownerId)
        {
            return itemList.Where(x => x.OwnerId == ownerId);
        }
        public IEnumerable<T_BagItem> GetItemByOwner(string ownerId,string ownerGuid)
        {
            return itemList.Where(x => x.OwnerId == ownerId&&x.OwnerGuid==ownerGuid);
        }

        /// <summary>
        /// 删除道具，返回是否成功
        /// </summary>
        public bool RemoveItem(string equipId, string equipGuid)
        {
            return itemList.RemoveAll(x => x.EquipId == equipId && x.EquipGuid == equipGuid) > 0;
        }

        /// <summary>
        /// 改变道具的拥有者
        /// </summary>
        public bool ChangeOwner(string equipId, string equipGuid, string ownerId,string ownerGuid = "")
        {
            var item = itemList.Find(x => x.EquipId == equipId && x.EquipGuid == equipGuid);
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
        public (string ownerId,string ownerGuid) GetOwner(string equipId, string guid)
        {
            var item = itemList.Find(x => x.EquipId == equipId && x.EquipGuid == guid);
            if (item != null)
            {
                return (item.OwnerId,item.OwnerGuid);
            }
            return ("","");
        }

        /// <summary>
        /// 获得道具数量
        /// </summary>
        public int GetItemCount(string itemId)
        {
            return itemList.Count(x => x.EquipId == itemId);
        }

        /// <summary>
        /// 判断某个道具是否有指定数量个
        /// </summary>
        public bool CheckItemCount(string itemId, int itemCount)
        {
            return itemList.Count(x => x.EquipId == itemId) >= itemCount;
        }


        private void InsertItem(T_BagItem item)
        {
            itemList.Add(item);
        }

        [Button]
        private void SortItemList()
        {
            itemList.Sort((a, b) => string.Compare(a.EquipId, b.EquipId));
        }
    }
}
