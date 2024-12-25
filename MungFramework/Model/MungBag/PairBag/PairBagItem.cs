using System;
using UnityEngine;

namespace MungFramework.Model.MungBag.PairBag
{
    /// <summary>
    /// 键值对背包项
    /// </summary>
    [Serializable]
    public class PairBagItem : MungFramework.ModelData.ModelData
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
