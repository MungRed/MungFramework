﻿using MungFramework.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Extension.LifeCycleExtension
{
    public static class LifeCycleExtension
    {
        public static void LateUpdateHelp(UnityAction action)
        {
            IEnumerator lateUpdate(UnityAction action)
            {
                yield return new WaitForEndOfFrame();
                if (action != null)
                {
                    action.Invoke();
                }
            }
            GameApplicationAbstract.Instance.StartCoroutine(lateUpdate(action));
        }
    }
}
