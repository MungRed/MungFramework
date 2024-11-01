using MungFramework.Logic.EventCenter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Entity
{
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// 实体组件的事件中心，用于实体组件之间通信
        /// </summary>
        private EventCenterModel entityEvent = new();

        protected virtual void Awake()
        {
            RegisterListenerOnAwake();
        }
        protected virtual void RegisterListenerOnAwake()
        {

        }

        public void AddListener_OnActionCall(UnityAction<string> listener) => entityEvent.AddListener_OnActionCall(listener);
        public void RemoveListener_OnActionCall(UnityAction<string> listener) => entityEvent.RemoveListener_OnActionCall(listener);
        public void AddListener_OnFuncCall(UnityAction<string> listener) => entityEvent.AddListener_OnFuncCall(listener);
        public void RemoveListener_OnFuncCall(UnityAction<string> listener) => entityEvent.RemoveListener_OnFuncCall(listener);

        public void AddListener_Action(string eventType, UnityAction listener) => entityEvent.AddListener_Action(eventType, listener);
        public void AddListener_Action<T>(string eventType, UnityAction<T> listener) => entityEvent.AddListener_Action(eventType, listener);
        public void AddListener_Func<R>(string eventType, Func<R> listener) => entityEvent.AddListener_Func(eventType, listener);
        public void AddListener_Func<T, R>(string eventType, Func<T, R> listener) => entityEvent.AddListener_Func(eventType, listener);
        public void CallAction(string eventType) => entityEvent.CallAction(eventType);
        public void CallAction<T>(string eventType, T parameter) => entityEvent.CallAction(eventType, parameter);
        public List<R> CallFunc<R>(string eventType) => entityEvent.CallFunc<R>(eventType);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => entityEvent.CallFunc<T, R>(eventType, parameter);
        public void RemoveListener_Action(string eventType, UnityAction listener) => entityEvent.RemoveListener_Action(eventType, listener);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> listener) => entityEvent.RemoveListener_Action(eventType, listener);
        public void RemoveListener_Func<R>(string eventType, Func<R> listener) => entityEvent.RemoveListener_Func(eventType, listener);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> listener) => entityEvent.RemoveListener_Func(eventType, listener);
    }
}
