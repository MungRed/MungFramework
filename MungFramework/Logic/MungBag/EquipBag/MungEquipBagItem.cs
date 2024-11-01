using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.MungBag.EquipBag
{
    /// <summary>
    /// 装备背包项
    /// 记录装备id，guid
    /// 装备者id，guid
    /// </summary>
    [Serializable]
    public class MungEquipBagItem : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        [ReadOnly]
        private string equipId;
        [SerializeField]
        [ReadOnly]
        private string euipGuid;

        [SerializeField]
        private string ownerId;
        [SerializeField]
        private string ownerGuid;


        public string EquipId
        {
            get => equipId;
            set => equipId = value;
        }
        public string EquipGuid
        {
            get => euipGuid;
            set => euipGuid = value;
        }

        public string OwnerId
        {
            get => ownerId;
            set => ownerId = value;
        }
        public string OwnerGuid
        {
            get => ownerGuid;
            set => ownerGuid = value;
        }
    }
}
