using MungFramework.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.MungAnim
{
    [CreateAssetMenu(fileName = "新动画", menuName = "MungFramework/Anim/新动画")]
    public class MungAnimationAssets : DataSO
    {
        [SerializeField]
        private List<MungAnimationFrame> animFramList;


        public int GetSize()
        {
            return animFramList.Count;
        }
        public MungAnimationFrame GetFrame(int index)
        {
            return animFramList[index];
        }
    }
}
