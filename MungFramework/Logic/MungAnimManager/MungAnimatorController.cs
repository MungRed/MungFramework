using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MungFramework.Logic.Anim
{
    /// <summary>
    /// 全局的动画控制器
    /// </summary>
    public class MungAnimatorController : SingletonGameControllerAbstract<MungAnimatorController>
    {
        [SerializeField]
        [ReadOnly]
        private List<MungAnimator> animatorList = new();


        public void Update()
        {
            foreach (var animator in animatorList)
            {
                if (!animator.UseMungLifeCycle)
                {
                    animator.AddTime(Time.deltaTime);
                }
            }
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);
            foreach (var animator in animatorList)
            {
                if (animator.UseMungLifeCycle)
                {
                    animator.AddTime(Time.deltaTime);
                }
            }
        }

        public void AddAnimator(MungAnimator animator)
        {
            if (!animatorList.Contains(animator))
            {
                animatorList.Add(animator);
            }
        }
        public void RemoveAnimator(MungAnimator animator)
        {
            animatorList.Remove(animator);
        }
    }
}

