using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


namespace MungFramework.Logic.MungAnim
{
    /// <summary>
    /// 全局的动画控制器
    /// </summary>
    public class MungAnimatorController : SingletonGameControllerAbstract<MungAnimatorController>
    {
        [SerializeField]
        [ReadOnly]
        private List<MungAnimator> animatorList_UseUnityLifeCycle = new();
        [SerializeField]
        [ReadOnly]
        private List<MungAnimator> animatorList_UseMungLifeCycle = new();

        public void Update()
        {
            foreach (var animator in animatorList_UseUnityLifeCycle)
            {
                animator.AddTime(Time.deltaTime);
            }
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);
            foreach (var animator in animatorList_UseMungLifeCycle)
            {
                animator.AddTime(Time.deltaTime);
            }
        }

        public void AddAnimator(MungAnimator animator)
        {
            if (animator.UseMungLifeCycle)
            {
                if (!animatorList_UseMungLifeCycle.Contains(animator))
                {
                    animatorList_UseMungLifeCycle.Add(animator);
                }
            }
            else
            {
                if (!animatorList_UseUnityLifeCycle.Contains(animator))
                {
                    animatorList_UseUnityLifeCycle.Add(animator);
                }
            }
        }
        public void RemoveAnimator(MungAnimator animator)
        {
            if (animator.UseMungLifeCycle)
            {
                animatorList_UseMungLifeCycle.Remove(animator);
            }
            else
            {
                animatorList_UseUnityLifeCycle.Remove(animator);
            }
        }
    }
}

