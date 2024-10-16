using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace MungFramework.Logic.MungBag.BagUi
{
    /// <summary>
    /// 背包Ui 模型/容器
    /// 记录背包Ui中的道具信息，具体如何显示道具由组合该模型的Ui控制器决定
    /// </summary>
    [Serializable]
    public class BagUiModel<T_ItemTypeEnum,T_ItemModel> : MungFramework.Model.Model 
        where T_ItemTypeEnum: Enum 
        where T_ItemModel : BagUiItemModel
    {
        [SerializeField]
        private SerializedDictionary<T_ItemTypeEnum, List<T_ItemModel>> itemDictionary = new();


        public void LoadData(Dictionary<T_ItemTypeEnum, List<T_ItemModel>> data)
        {
            itemDictionary.Clear();
            foreach (var item in data)
            {
                itemDictionary.Add(item.Key, new List<T_ItemModel>(item.Value));
            }
        }

        public void Clear()
        {
            itemDictionary.Clear();
        }


        public Dictionary<T_ItemTypeEnum, List<T_ItemModel>> GetAllItem()
        {
            return itemDictionary;
        }


        /// <summary>
        /// 获取所有道具Model
        /// </summary>
        public List<KeyValuePair<T_ItemTypeEnum, T_ItemModel>> GetItemModelList()
        {
            List<KeyValuePair<T_ItemTypeEnum, T_ItemModel>> res = new();

            var listByGroup = GetItemListByGroup();
            foreach (var item in listByGroup)
            {
                foreach (var itemModel in item.Value)
                {
                    res.Add(new KeyValuePair<T_ItemTypeEnum, T_ItemModel>(item.Key, itemModel));
                }
            }
            return res;
        }

        /// <summary>
        /// 获取所有道具Model
        /// 根据ItemType分类
        /// </summary>
        public List<KeyValuePair<T_ItemTypeEnum, List<T_ItemModel>>> GetItemListByGroup()
        {
            var res = itemDictionary.ToList();
            res.Sort((x, y) => x.Key.CompareTo(y.Key));
            return res;
        }

        /// <summary>
        /// 获取所有指定类型的道具Model
        /// </summary>
        public List<T_ItemModel> GetItemListByType(T_ItemTypeEnum type)
        {
            if (itemDictionary.ContainsKey(type))
            {
                return itemDictionary[type];
            }
            return new();
        }


        /// <summary>
        /// 获取道具Model
        /// </summary>
        public T_ItemModel GetItemModel(T_ItemTypeEnum itemType, Predicate<T_ItemModel> match)
        {
            if (itemDictionary.ContainsKey(itemType))
            {
                return itemDictionary[itemType].Find(match);
            }
            return null;
        }

        /// <summary>
        /// 添加道具Model
        /// </summary>
        public void AddItemModel(T_ItemTypeEnum itemType, T_ItemModel itemModel)
        {
            if (!itemDictionary.ContainsKey(itemType))
            {
                itemDictionary.Add(itemType, new ());
            }
            itemDictionary[itemType].Add(itemModel);
        }

        public void RemoveItemModel(T_ItemTypeEnum itemType, T_ItemModel itemModel)
        {
            if (itemDictionary.ContainsKey(itemType))
            {
                itemDictionary[itemType].Remove(itemModel);
            }
        }
    }
}
