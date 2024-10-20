using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Logic.TimeCounter
{
    public static class  TimeCounterManager
    {
        private static Dictionary<TimeCounter, Coroutine> timeCounterDic = new();

        public static TimeCounter StartTimeCounter(float totalTime, float stepTime, UnityAction<float, float> stepAction, UnityAction completeAction)
        {
            TimeCounter TimeCounter = new(totalTime, stepTime, stepAction, completeAction);         
            timeCounterDic.Add(TimeCounter, GameApplicationAbstract.Instance.StartCoroutine(TimeCount(TimeCounter)));
            return TimeCounter;
        }

        public static void StopTimeCounter(TimeCounter timeCounter)
        {
            if (timeCounterDic.ContainsKey(timeCounter))
            {
                GameApplicationAbstract.Instance.StopCoroutine(timeCounterDic[timeCounter]);
                timeCounterDic.Remove(timeCounter);
            }
        }
        private static IEnumerator TimeCount(TimeCounter timeCounter)
        {
            var wait = new WaitForFixedUpdate();
            while (!timeCounter.Complete)
            {
                timeCounter.AddNowTime(Time.fixedDeltaTime);
                yield return wait;
            }
            timeCounterDic.Remove(timeCounter);
        }
    }
}
