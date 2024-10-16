using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public class EventCenterAbstract : MungFramework.Logic.SingletonGameControllerAbstract<EventCenterAbstract>, IEventCenter
    {
        private EventCenterModel eventCenterModel = new();

        public void AddAction(string eventType, UnityAction action)=> eventCenterModel.AddAction(eventType, action);
        public void AddAction<T>(string eventType, UnityAction<T> action) => eventCenterModel.AddAction(eventType, action);
        public void AddFunc<R>(string eventType, Func<R> func) => eventCenterModel.AddFunc(eventType, func);
        public void AddFunc<T, R>(string eventType, Func<T, R> func) => eventCenterModel.AddFunc(eventType, func);
        public void CallAction(string eventType) => eventCenterModel.CallAction(eventType);
        public void CallAction<T>(string eventType, T parameter) => eventCenterModel.CallAction(eventType, parameter);
        public List<R> CallFunc<R>(string eventType) => eventCenterModel.CallFunc<R>(eventType);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => eventCenterModel.CallFunc<T, R>(eventType, parameter);
        public void RemoveAction(string eventType, UnityAction action) => eventCenterModel.RemoveAction(eventType, action);
        public void RemoveAction<T>(string eventType, UnityAction<T> action) => eventCenterModel.RemoveAction(eventType, action);
        public void RemoveFunc<R>(string eventType, Func<R> func) => eventCenterModel.RemoveFunc(eventType, func);
        public void RemoveFunc<T, R>(string eventType, Func<T, R> func) => eventCenterModel.RemoveFunc(eventType, func);

    }
}
