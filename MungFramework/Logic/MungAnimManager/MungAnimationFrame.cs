using System;
using UnityEngine;

namespace MungFramework.Logic.Anim
{
    [Serializable]
    public class MungAnimationFrame : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private Sprite sprite; //动画帧
        [SerializeField]
        private int durationFrame; //持续帧数


        public Sprite Sprite => sprite;
        public int DurationFrame => durationFrame;

    }
}
