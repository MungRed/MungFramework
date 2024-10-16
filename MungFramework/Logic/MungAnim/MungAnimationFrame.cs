using System;
using UnityEngine;

namespace MungFramework.Logic.MungAnim
{
    [Serializable]
    public class MungAnimationFrame : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private Sprite sprite; //动画帧
        [SerializeField]
        private int duration; //持续帧数

        public Sprite Sprite => sprite;
        public int Duration => duration;

    }
}
