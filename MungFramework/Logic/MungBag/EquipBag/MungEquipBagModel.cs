using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.Bag.EquipBag
{
    /// <summary>
    /// 装备背包模型
    /// 每个装备单独分配一个id
    /// 储存每个装备单的的id和装备id和被装备的角色id
    /// </summary>
    [Serializable]
    public class MungEquipBagModel<BagItem> : MungFramework.Model.Model where BagItem:MungEquipBagItem,new()
    {
        [SerializeField]
        private List<BagItem> itemList = new();


        public ReadOnlyCollection<BagItem> GetItemList()
        {
            return itemList.AsReadOnly();
        }
        public IEnumerable<BagItem> GetItemByOwner(string ownerId)
        {
            return GetItemList().Where(x => x.OwnerId == ownerId);
        }


        /// <summary>
        /// 向背包中添加一个道具
        /// </summary>
        [Button("AddItem")]
        public BagItem AddItem(string itemId)
        {
            BagItem item = new()
            {
                UUID = Guid.NewGuid().ToString(),
                EquipId = itemId,
                OwnerId = ""
            };
            InsertItem(item);
            return item;
        }

        /// <summary>
        /// 获得道具
        /// </summary>
        public BagItem GetItem(string equipId, string uuid)
        {
            return itemList.Find(x => x.EquipId == equipId && x.UUID == uuid);
        }

        /// <summary>
        /// 删除道具，返回是否成功
        /// </summary>
        public bool RemoveItem(string equipId, string uuid)
        {
            return itemList.RemoveAll(x => x.EquipId == equipId && x.UUID == uuid) > 0;
        }


        /// <summary>
        /// 改变道具的拥有者
        /// </summary>
        public bool ChangeOwner(string equipId, string uuid, string ownerId)
        {
            var item = itemList.Find(x => x.EquipId == equipId && x.UUID == uuid);
            if (item != null)
            {
                item.OwnerId = ownerId;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得道具的拥有者
        /// </summary>
        public string GetOwner(string equipId, string uuid)
        {
            var item = itemList.Find(x => x.EquipId == equipId && x.UUID == uuid);
            if (item != null)
            {
                return item.OwnerId;
            }
            return "";
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


        private void InsertItem(BagItem item)
        {
            itemList.Insert(0, item);
            for (int i = 1; i < itemList.Count; i++)
            {
                if (itemList[i-1].EquipId.CompareTo(itemList[i].EquipId) > 0)
                {
                    var temp = itemList[i];
                    itemList[i] = itemList[i - 1];
                    itemList[i - 1] = temp;
                }
            }
        }

        [Button("SortItemList")]
        private void SortItemList()
        {
            itemList.Sort((a, b) => string.Compare(a.EquipId, b.EquipId));
        }
    }
}
