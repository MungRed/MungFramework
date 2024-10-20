using MungFramework.Logic.EventCenter;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Entity
{
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// 实体组件的事件中心，用于实体组件之间通信
        /// </summary>
        private EventCenterModel entityEventCenterModel = new();

        public void AddListener_Action(string eventType, UnityAction action) => entityEventCenterModel.AddListener_Action(eventType, action);
        public void AddListener_Action<T>(string eventType, UnityAction<T> action) => entityEventCenterModel.AddListener_Action(eventType, action);
        public void AddListener_Func<R>(string eventType, Func<R> func) => entityEventCenterModel.AddListener_Func(eventType, func);
        public void AddListener_Func<T, R>(string eventType, Func<T, R> func) => entityEventCenterModel.AddListener_Func(eventType, func);
        public void CallAction(string eventType) => entityEventCenterModel.CallAction(eventType);
        public void CallAction<T>(string eventType, T parameter) => entityEventCenterModel.CallAction(eventType, parameter);
        public List<R> CallFunc<R>(string eventType) => entityEventCenterModel.CallFunc<R>(eventType);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => entityEventCenterModel.CallFunc<T, R>(eventType, parameter);
        public void RemoveListener_Action(string eventType, UnityAction action) => entityEventCenterModel.RemoveListener_Action(eventType, action);
        public void RemoveListener_Action<T>(string eventType, UnityAction<T> action) => entityEventCenterModel.RemoveListener_Action(eventType, action);
        public void RemoveListener_Func<R>(string eventType, Func<R> func) => entityEventCenterModel.RemoveListener_Func(eventType, func);
        public void RemoveListener_Func<T, R>(string eventType, Func<T, R> func) => entityEventCenterModel.RemoveListener_Func(eventType, func);
    }
}
