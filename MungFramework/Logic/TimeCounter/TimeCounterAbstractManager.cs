using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Logic.TimeCounter
{
    public class TimeCounterAbstractManager : SingletonGameManagerAbstract<TimeCounterAbstractManager>
    {
        private static Dictionary<TimeCounter, Coroutine> timeCounterDic = new();


        public static TimeCounter StartTimeCounter(float totalTime, float stepTime, UnityAction stepAction, UnityAction completeAction)
        {
            TimeCounter TimeCounter = new(totalTime, stepTime, stepAction, completeAction);         
            timeCounterDic.Add(TimeCounter, Instance.StartCoroutine(TimeCount(TimeCounter)));
            return TimeCounter;
        }
        public static void StopTimeCounter(TimeCounter timeCounter)
        {
            if (timeCounterDic.ContainsKey(timeCounter))
            {
                Instance.StopCoroutine(timeCounterDic[timeCounter]);
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
