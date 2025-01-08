using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public class EventCenterManager : GameManagerAbstract,IEventCenter
    {
        public EventCenterModel EventCenterModel
        {
            get;
        } = new();

        public void AddListener_OnActionCall(UnityAction<string> listener) => EventCenterModel.AddListener_OnActionCall(listener);
        public void RemoveListener_OnActionCall(UnityAction<string> listener) => EventCenterModel.RemoveListener_OnActionCall(listener);
        public void AddListener_OnFuncCall(UnityAction<string> listener) => EventCenterModel.AddListener_OnFuncCall(listener);
        public void RemoveListener_OnFuncCall(UnityAction<string> listener) => EventCenterModel.RemoveListener_OnFuncCall(listener);

        public void AddListener_Action(string eventType, UnityAction listener) => EventCenterModel.AddListener_Action(eventType, listener);
        public void RemoveListener_Action(string eventType, UnityAction listener) => EventCenterModel.RemoveListener_Action(eventType, listener);
        public void CallAction(string eventType) => EventCenterModel.CallAction(eventType);


        public void AddListener_Action<T>(string eventType, UnityAction<T> listener) => EventCenterModel.AddListener_Action(eventType, listener);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> listener) => EventCenterModel.RemoveListener_Action(eventType, listener);
        public void CallAction<T>(string eventType, T parameter) => EventCenterModel.CallAction(eventType, parameter);

        public void AddListener_Func<R>(string eventType, Func<R> listener) => EventCenterModel.AddListener_Func(eventType, listener);
        public void RemoveListener_Func<R>(string eventType, Func<R> listener) => EventCenterModel.RemoveListener_Func(eventType, listener);
        public List<R> CallFunc<R>(string eventType) => EventCenterModel.CallFunc<R>(eventType);

        public void AddListener_Func<T, R>(string eventType, Func<T, R> listener) => EventCenterModel.AddListener_Func(eventType, listener);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> listener) => EventCenterModel.RemoveListener_Func(eventType, listener);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => EventCenterModel.CallFunc<T, R>(eventType, parameter);

        public void Clear() => EventCenterModel.Clear();
    }
}
