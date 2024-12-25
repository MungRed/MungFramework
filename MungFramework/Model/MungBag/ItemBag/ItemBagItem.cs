using System;
using UnityEngine;

namespace MungFramework.Model.MungBag.ItemBag
{
    /// <summary>
    /// 道具背包项,记录道具id和数量
    /// </summary>
    [Serializable]
    public class ItemBagItem : MungFramework.ModelData.ModelData
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
