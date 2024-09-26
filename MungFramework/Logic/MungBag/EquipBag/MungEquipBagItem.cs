using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Bag.EquipBag
{
    /// <summary>
    /// 装备背包项
    /// </summary>
    [Serializable]
    public class MungEquipBagItem : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        [ReadOnly]
        private string uuid;
        [SerializeField]
        [ReadOnly]
        private string equipId;
        [SerializeField]
        private string ownerId;

        public string UUID
        {
            get => uuid;
            set => uuid = value;
        }
        public string EquipId
        {
            get => equipId;
            set => equipId = value;
        }
        public string OwnerId
        {
            get => ownerId;
            set => ownerId = value;
        }
    }
}
