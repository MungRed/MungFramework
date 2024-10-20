using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public class EventCenterAbstract : MungFramework.Logic.SingletonGameControllerAbstract<EventCenterAbstract>, IEventCenter
    {
        private EventCenterModel eventCenterModel = new();

        public void AddListener_Action(string eventType, UnityAction action)=> eventCenterModel.AddListener_Action(eventType, action);
        public void AddListener_Action<T>(string eventType, UnityAction<T> action) => eventCenterModel.AddListener_Action(eventType, action);
        public void AddListener_Func<R>(string eventType, Func<R> func) => eventCenterModel.AddListener_Func(eventType, func);
        public void AddListener_Func<T, R>(string eventType, Func<T, R> func) => eventCenterModel.AddListener_Func(eventType, func);
        public void CallAction(string eventType) => eventCenterModel.CallAction(eventType);
        public void CallAction<T>(string eventType, T parameter) => eventCenterModel.CallAction(eventType, parameter);
        public List<R> CallFunc<R>(string eventType) => eventCenterModel.CallFunc<R>(eventType);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => eventCenterModel.CallFunc<T, R>(eventType, parameter);
        public void RemoveListener_Action(string eventType, UnityAction action) => eventCenterModel.RemoveListener_Action(eventType, action);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> action) => eventCenterModel.RemoveListener_Action(eventType, action);
        public void RemoveListener_Func<R>(string eventType, Func<R> func) => eventCenterModel.RemoveListener_Func(eventType, func);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> func) => eventCenterModel.RemoveListener_Func(eventType, func);

    }
}
