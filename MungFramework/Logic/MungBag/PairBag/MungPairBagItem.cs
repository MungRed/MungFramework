using System;
using UnityEngine;

namespace MungFramework.Logic.MungBag.PairBag
{
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
