using UnityEngine.Events;

namespace MungFramework.Logic.TimeCounter
{
    public class TimeCounter : Model.Model
    {
        private float totalTime;        //总时间
        private float stepTime;        //每步时间
        private float stepCount;
        private float nowTime;        //当前时间
        private bool complete;        //完成

        private UnityAction stepAction;
        private UnityAction completeAction;

        public float TotalTime => totalTime;
        public float NowTime => nowTime;
        public float Progress => nowTime / totalTime;
        public bool Complete => complete;

        public TimeCounter(float totalTime, float stepTime,UnityAction stepAction,UnityAction completeAction)
        {
            this.totalTime = totalTime;
            this.stepTime = stepTime;
            this.stepAction = stepAction;
            this.completeAction = completeAction;

            stepCount = 0;
            nowTime = 0;
            complete = false;
        }

        public void AddNowTime(float deltaTime)
        {
            nowTime += deltaTime;
            stepCount += deltaTime;

            if (stepCount >= stepTime)
            {
                stepCount = 0;
                if (stepAction != null)
                {
                    stepAction.Invoke();
                }
            }

            if (nowTime >= totalTime)
            {
                nowTime = deltaTime;
                complete = true;
                if (completeAction != null)
                {
                    completeAction.Invoke();
                }
            }
        }
    }
}
