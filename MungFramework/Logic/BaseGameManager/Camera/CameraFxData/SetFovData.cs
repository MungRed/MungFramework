using System;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    [Serializable]
    public class SetFovData : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private float value;
        [SerializeField]
        private float duration;


        public float Value => value;
        public float Duration => duration;
    }
}
