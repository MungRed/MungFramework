using System;
using UnityEngine;

namespace MungFramework.Logic.MungBag.PairBag
{
    /// <summary>
    /// 键值对背包项
    /// </summary>
    [Serializable]
    public class MungPairBagItem : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private string key;

        public string Key
        {
            get => key;
            set => key = value;
        }
    }
}
