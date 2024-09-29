using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace MungFramework.Logic.Bag.FlagBag
{

    [Serializable]
    public class MungFlagBagItem : MungFramework.ModelData.ModelData
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
