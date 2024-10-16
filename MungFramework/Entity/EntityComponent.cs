using MungFramework.Logic.EventCenter;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Entity
{
    public abstract class EntityComponent<E> : MonoBehaviour where E: Entity
    {
        private E entity;

        [ShowInInspector]
        public E Entity
        {
            get
            {
                if (entity == null)
                {
                    entity = GetComponentInParent<E>();
                }
                return entity;
            }
        }

        protected void AddActionFromEntity(string eventType, UnityAction action) => Entity?.AddAction(eventType, action);
        protected void AddActionFromEntity<T>(string eventType, UnityAction<T> action) => Entity?.AddAction(eventType, action);
        protected void AddFuncFromEntity<R>(string eventType, Func<R> func) => Entity?.AddFunc(eventType, func);
        protected void AddFuncFromEntity<T, R>(string eventType, Func<T, R> func) => Entity?.AddFunc(eventType, func);
        protected void CallActionFromEntity(string eventType) => Entity?.CallAction(eventType);
        protected void CallActionFromEntity<T>(string eventType, T parameter) => Entity?.CallAction(eventType, parameter);
        protected List<R> CallFuncFromEntity<R>(string eventType) => Entity?.CallFunc<R>(eventType);
        protected List<R> CallFuncFromEntity<T, R>(string eventType, T parameter) => Entity?.CallFunc<T, R>(eventType, parameter);
        protected void RemoveActionFromEntity(string eventType, UnityAction action) => Entity?.RemoveAction(eventType, action);
        protected void RemoveActionFromEntity<T>(string eventType, UnityAction<T> action) => Entity?.RemoveAction(eventType, action);
        protected void RemoveFuncFromEntity<R>(string eventType, Func<R> func) => Entity?.RemoveFunc(eventType, func);
        protected void RemoveFuncFromEntity<T, R>(string eventType, Func<T, R> func) => Entity?.RemoveFunc(eventType, func);
    }
}