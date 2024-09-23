using MungFramework.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MungFramework.Logic.Anim
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
