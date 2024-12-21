using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public class EventCenterAbstract : MungFramework.Logic.SingletonGameManagerAbstract<EventCenterAbstract>, IEventCenter
    {
        private EventCenterModel eventCenterModel = new();

        public void AddListener_OnActionCall(UnityAction<string> listener) => eventCenterModel.AddListener_OnActionCall(listener);
        public void RemoveListener_OnActionCall(UnityAction<string> listener) => eventCenterModel.RemoveListener_OnActionCall(listener);
        public void AddListener_OnFuncCall(UnityAction<string> listener) => eventCenterModel.AddListener_OnFuncCall(listener);
        public void RemoveListener_OnFuncCall(UnityAction<string> listener) => eventCenterModel.RemoveListener_OnFuncCall(listener);

        public void AddListener_Action(string eventType, UnityAction listener) => eventCenterModel.AddListener_Action(eventType, listener);
        public void RemoveListener_Action(string eventType, UnityAction listener) => eventCenterModel.RemoveListener_Action(eventType, listener);
        public void CallAction(string eventType) => eventCenterModel.CallAction(eventType);


        public void AddListener_Action<T>(string eventType, UnityAction<T> listener) => eventCenterModel.AddListener_Action(eventType, listener);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> listener) => eventCenterModel.RemoveListener_Action(eventType, listener);
        public void CallAction<T>(string eventType, T parameter) => eventCenterModel.CallAction(eventType, parameter);

        public void AddListener_Func<R>(string eventType, Func<R> listener) => eventCenterModel.AddListener_Func(eventType, listener);
        public void RemoveListener_Func<R>(string eventType, Func<R> listener) => eventCenterModel.RemoveListener_Func(eventType, listener);
        public List<R> CallFunc<R>(string eventType) => eventCenterModel.CallFunc<R>(eventType);

        public void AddListener_Func<T, R>(string eventType, Func<T, R> listener) => eventCenterModel.AddListener_Func(eventType, listener);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> listener) => eventCenterModel.RemoveListener_Func(eventType, listener);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => eventCenterModel.CallFunc<T, R>(eventType, parameter);

    }
}
