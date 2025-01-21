using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Model.MungBag.FlagBag
{
    /// <summary>
    /// flag背包项
    /// 记录flag名字和值以及上次修改的时间
    /// </summary>
    [Serializable]
    public class FlagBagItem : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private string flagName;
        [SerializeField]
        private int flagValue;
        [SerializeField]
        [ReadOnly]
        private string lastChangeTime;

        public string FlagName
        {
            get => flagName;
            set => flagName = value;
        }
        public int FlagValue
        {
            get => flagValue;
            set => flagValue = value;
        }
        public string LastChangeTime
        {
            get => lastChangeTime;
            set => lastChangeTime = value;
        }
    }
}
