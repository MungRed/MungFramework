using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Entity
{
    public abstract class EntityComponent<E> : MonoBehaviour where E : Entity
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

        protected void AddListener_OnActionCall(UnityAction<string> listener) => Entity?.AddListener_OnActionCall(listener);
        protected void RemoveListener_OnActionCall(UnityAction<string> listener) => Entity?.RemoveListener_OnActionCall(listener);
        protected void AddListener_OnFuncCall(UnityAction<string> listener) => Entity?.AddListener_OnFuncCall(listener);
        protected void RemoveListener_OnFuncCall(UnityAction<string> listener) => Entity?.RemoveListener_OnFuncCall(listener);

        protected void AddActionFromEntity(string eventType, UnityAction listener) => Entity?.AddListener_Action(eventType, listener);
        protected void AddActionFromEntity<T>(string eventType, UnityAction<T> listener) => Entity?.AddListener_Action(eventType, listener);
        protected void AddFuncFromEntity<R>(string eventType, Func<R> listener) => Entity?.AddListener_Func(eventType, listener);
        protected void AddFuncFromEntity<T, R>(string eventType, Func<T, R> listener) => Entity?.AddListener_Func(eventType, listener);
        protected void CallActionFromEntity(string eventType) => Entity?.CallAction(eventType);
        protected void CallActionFromEntity<T>(string eventType, T parameter) => Entity?.CallAction(eventType, parameter);
        protected List<R> CallFuncFromEntity<R>(string eventType) => Entity?.CallFunc<R>(eventType);
        protected List<R> CallFuncFromEntity<T, R>(string eventType, T parameter) => Entity?.CallFunc<T, R>(eventType, parameter);
        protected void RemoveActionFromEntity(string eventType, UnityAction action) => Entity?.RemoveListener_Action(eventType, action);
        protected void RemoveActionFromEntity<T>(string eventType, UnityAction<T> listener) => Entity?.RemoveListener_Action(eventType, listener);
        protected void RemoveFuncFromEntity<R>(string eventType, Func<R> listener) => Entity?.RemoveListener_Func(eventType, listener);
        protected void RemoveFuncFromEntity<T, R>(string eventType, Func<T, R> listener) => Entity?.RemoveListener_Func(eventType, listener);
    }
}