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

        public void AddAction(string eventType, UnityAction action) => entityEventCenterModel.AddAction(eventType, action);
        public void AddAction<T>(string eventType, UnityAction<T> action) => entityEventCenterModel.AddAction(eventType, action);
        public void AddFunc<R>(string eventType, Func<R> func) => entityEventCenterModel.AddFunc(eventType, func);
        public void AddFunc<T, R>(string eventType, Func<T, R> func) => entityEventCenterModel.AddFunc(eventType, func);
        public void CallAction(string eventType) => entityEventCenterModel.CallAction(eventType);
        public void CallAction<T>(string eventType, T parameter) => entityEventCenterModel.CallAction(eventType, parameter);
        public List<R> CallFunc<R>(string eventType) => entityEventCenterModel.CallFunc<R>(eventType);
        public List<R> CallFunc<T, R>(string eventType, T parameter) => entityEventCenterModel.CallFunc<T, R>(eventType, parameter);
        public void RemoveAction(string eventType, UnityAction action) => entityEventCenterModel.RemoveAction(eventType, action);
        public void RemoveAction<T>(string eventType, UnityAction<T> action) => entityEventCenterModel.RemoveAction(eventType, action);
        public void RemoveFunc<R>(string eventType, Func<R> func) => entityEventCenterModel.RemoveFunc(eventType, func);
        public void RemoveFunc<T, R>(string eventType, Func<T, R> func) => entityEventCenterModel.RemoveFunc(eventType, func);
    }
}
