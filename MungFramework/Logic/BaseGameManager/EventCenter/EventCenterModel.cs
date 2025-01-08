using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    /// <summary>
    /// 事件中心模型，可以组合到任何需要的类中，可以代替UnityEvent，实现事件的监听和调用
    /// </summary>
    public class EventCenterModel : MungFramework.Model.Model
    {
        private UnityEvent<string> onActionCall = new();
        private UnityEvent<string> onFuncCall = new();

        private Dictionary<string, UnityEvent> actionDic_NoParameter = new();
        private Dictionary<string, Dictionary<Type, HashSet<object>>> funcDic_NoParameter = new();
        private Dictionary<string, Dictionary<Type, HashSet<object>>> actionDic_HaveParameter = new();
        private Dictionary<string, Dictionary<(Type, Type), HashSet<object>>> funcDic_HaveParameter = new();


        #region OnActionOrFuncCall
        public void AddListener_OnActionCall(UnityAction<string> listener)
        {
            onActionCall.AddListener(listener);
        }
        public void RemoveListener_OnActionCall(UnityAction<string> listener)
        {
            onActionCall.RemoveListener(listener);
        }
        public void AddListener_OnFuncCall(UnityAction<string> listener)
        {
            onFuncCall.AddListener(listener);
        }
        public void RemoveListener_OnFuncCall(UnityAction<string> listener)
        {
            onFuncCall.RemoveListener(listener);
        }
        #endregion

        #region Action
        public void AddListener_Action(string eventType, UnityAction listener)
        {
            if (!actionDic_NoParameter.ContainsKey(eventType))
            {
                actionDic_NoParameter.Add(eventType, new());
            }
            actionDic_NoParameter[eventType].AddListener(listener);
        }
        public void RemoveListener_Action(string eventType, UnityAction listener)
        {
            if (actionDic_NoParameter.ContainsKey(eventType))
            {
                actionDic_NoParameter[eventType].RemoveListener(listener);
            }
        }
        public void CallAction(string eventType)
        {
            if (actionDic_NoParameter.ContainsKey(eventType))
            {
                actionDic_NoParameter[eventType].Invoke();
            }
            onActionCall.Invoke(eventType);
        }


        public void AddListener_Action<T>(string eventType, UnityAction<T> listener)
        {
            Type parameterType = typeof(T);
            if (!actionDic_HaveParameter.ContainsKey(eventType))
            {
                actionDic_HaveParameter.Add(eventType, new());
            }
            if (!actionDic_HaveParameter[eventType].ContainsKey(parameterType))
            {
                actionDic_HaveParameter[eventType].Add(parameterType, new());
            }
            actionDic_HaveParameter[eventType][parameterType].Add(listener);
        }
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> listener)
        {
            Type parameterType = typeof(T);
            if (actionDic_HaveParameter.ContainsKey(eventType) && actionDic_HaveParameter[eventType].ContainsKey(parameterType))
            {
                actionDic_HaveParameter[eventType][parameterType].Remove(listener);
            }
        }
        public void CallAction<T>(string eventType, T parameter)
        {
            Type parameterType = typeof(T);
            if (actionDic_HaveParameter.ContainsKey(eventType) && actionDic_HaveParameter[eventType].ContainsKey(parameterType))
            {
                foreach (UnityAction<T> action in actionDic_HaveParameter[eventType][parameterType])
                {
                    action?.Invoke(parameter);
                }
            }
            onActionCall.Invoke(eventType);
        }
        #endregion

        #region Func
        public void AddListener_Func<R>(string eventType, Func<R> listener)
        {
            Type returnType = typeof(R);
            if (!funcDic_NoParameter.ContainsKey(eventType))
            {
                funcDic_NoParameter.Add(eventType, new());
            }
            if (!funcDic_NoParameter[eventType].ContainsKey(returnType))
            {
                funcDic_NoParameter[eventType].Add(returnType, new());
            }
            funcDic_NoParameter[eventType][returnType].Add(listener);
        }
        public void RemoveListener_Func<R>(string eventType, Func<R> listener)
        {
            Type returnType = typeof(R);
            if (funcDic_NoParameter.ContainsKey(eventType) && funcDic_NoParameter[eventType].ContainsKey(returnType))
            {
                funcDic_NoParameter[eventType][returnType].Remove(listener);
            }
        }
        public List<R> CallFunc<R>(string eventType)
        {
            Type returnType = typeof(R);
            List<R> result = new();
            if (funcDic_NoParameter.ContainsKey(eventType) && funcDic_NoParameter[eventType].ContainsKey(returnType))
            {
                foreach (Func<R> action in funcDic_NoParameter[eventType][returnType])
                {
                    if (action != null)
                    {
                        var res = action.Invoke();
                        result.Add(res);
                    }
                }
            }
            onFuncCall.Invoke(eventType);
            return result;
        }

        public void AddListener_Func<T, R>(string eventType, Func<T, R> listener)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);
            if (!funcDic_HaveParameter.ContainsKey(eventType))
            {
                funcDic_HaveParameter.Add(eventType, new());
            }
            if (!funcDic_HaveParameter[eventType].ContainsKey(type))
            {
                funcDic_HaveParameter[eventType].Add(type, new());
            }
            funcDic_HaveParameter[eventType][type].Add(listener);
        }
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> listener)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);
            if (funcDic_HaveParameter.ContainsKey(eventType) && funcDic_HaveParameter[eventType].ContainsKey(type))
            {
                funcDic_HaveParameter[eventType][type].Remove(listener);
            }
        }
        public List<R> CallFunc<T, R>(string eventType, T parameter)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);

            List<R> result = new();
            if (funcDic_HaveParameter.ContainsKey(eventType) && funcDic_HaveParameter[eventType].ContainsKey(type))
            {
                foreach (Func<T, R> action in funcDic_HaveParameter[eventType][type])
                {
                    if (action != null)
                    {
                        var res = action.Invoke(parameter);
                        result.Add(res);
                    }
                }
            }
            onFuncCall.Invoke(eventType);
            return result;
        }
        #endregion

        public void Clear()
        {
            onActionCall.RemoveAllListeners();
            onFuncCall.RemoveAllListeners();
            actionDic_NoParameter.Clear();
            actionDic_HaveParameter.Clear();
            funcDic_NoParameter.Clear();
            funcDic_HaveParameter.Clear();
        }
    }
}
