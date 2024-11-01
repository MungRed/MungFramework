using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public interface IEventCenter
    {
        public void AddListener_OnActionCall(UnityAction<string> listener);
        public void RemoveListener_OnActionCall(UnityAction<string> listener);
        public void AddListener_OnFuncCall(UnityAction<string> listener);
        public void RemoveListener_OnFuncCall(UnityAction<string> listener);

        public void AddListener_Action(string eventType, UnityAction listener);
        public void RemoveListener_Action(string eventType, UnityAction listener);
        public void CallAction(string eventType);

        public void AddListener_Action<T>(string eventType, UnityAction<T> listener);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> listener);
        public void CallAction<T>(string eventType, T parameter);

        public void AddListener_Func<R>(string eventType, Func<R> listener);
        public void RemoveListener_Func<R>(string eventType, Func<R> listener);
        public List<R> CallFunc<R>(string eventType);

        public void AddListener_Func<T, R>(string eventType, Func<T, R> listener);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> listener);
        public List<R> CallFunc<T, R>(string eventType, T parameter);
    }
}
