using System;
using UnityEngine;

namespace MungFramework.Logic.Bag.ItemBag
{
    /// <summary>
    /// 道具背包项
    /// </summary>
    [Serializable]
    public class MungItemBagItem : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private string itemId;
        [SerializeField]
        private int itemCount;

        public string ItemId
        {
            get => itemId;
            set => itemId = value;
        }
        public int ItemCount
        {
            get => itemCount;
            set => itemCount = value;
        }
    }
}
