using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public interface IEventCenter
    {
        public void AddAction(string eventType, UnityAction action);
        public void RemoveAction(string eventType, UnityAction action);
        public void CallAction(string eventType); 

        public void AddAction<T>(string eventType, UnityAction<T> action);
        public void RemoveAction<T>(string eventType, UnityAction<T> action);
        public void CallAction<T>(string eventType, T parameter);

        public void AddFunc<R>(string eventType, Func<R> func);
        public void RemoveFunc<R>(string eventType, Func<R> func);
        public List<R> CallFunc<R>(string eventType);

        public void AddFunc<T, R>(string eventType, Func<T, R> func);
        public void RemoveFunc<T, R>(string eventType, Func<T, R> func);
        public List<R> CallFunc<T, R>(string eventType, T parameter);

    }
}
