using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MungFramework.Logic.EventCenter
{
    public class EventCenterModel : MungFramework.Model.Model, IEventCenter
    {
        private Dictionary<string, UnityEvent> eventDictionary_NoParameter = new();
        private Dictionary<string, Dictionary<Type, HashSet<object>>> eventDictionary_NoParameterHaveReturn = new();
        private Dictionary<string, Dictionary<Type, HashSet<object>>> eventDictionary_HaveParameter = new();
        private Dictionary<string, Dictionary<(Type, Type), HashSet<object>>> eventDictionary_HaveParameterHaveReturn = new();

        public void AddAction(string eventType, UnityAction action)
        {
            if (!eventDictionary_NoParameter.ContainsKey(eventType))
            {
                eventDictionary_NoParameter.Add(eventType, new());
            }
            eventDictionary_NoParameter[eventType].AddListener(action);
        }

        public void RemoveAction(string eventType, UnityAction action)
        {
            if (eventDictionary_NoParameter.ContainsKey(eventType))
            {
                eventDictionary_NoParameter[eventType].RemoveListener(action);
            }
        }

        public void CallAction(string eventType)
        {
            if (eventDictionary_NoParameter.ContainsKey(eventType))
            {
                eventDictionary_NoParameter[eventType].Invoke();
            }
        }

        public void AddAction<T>(string eventType, UnityAction<T> action)
        {
            Type parameterType = typeof(T);
            if (!eventDictionary_HaveParameter.ContainsKey(eventType))
            {
                eventDictionary_HaveParameter.Add(eventType, new());
            }
            if (!eventDictionary_HaveParameter[eventType].ContainsKey(parameterType))
            {
                eventDictionary_HaveParameter[eventType].Add(parameterType, new());
            }
            eventDictionary_HaveParameter[eventType][parameterType].Add(action);
        }
        public void RemoveAction<T>(string eventType, UnityAction<T> action)
        {
            Type parameterType = typeof(T);
            if (eventDictionary_HaveParameter.ContainsKey(eventType) && eventDictionary_HaveParameter[eventType].ContainsKey(parameterType))
            {
                eventDictionary_HaveParameter[eventType][parameterType].Remove(action);
            }
        }

        public void CallAction<T>(string eventType, T parameter)
        {
            Type parameterType = typeof(T);
            if (eventDictionary_HaveParameter.ContainsKey(eventType) && eventDictionary_HaveParameter[eventType].ContainsKey(parameterType))
            {
                foreach (UnityAction<T> action in eventDictionary_HaveParameter[eventType][parameterType])
                {
                    action?.Invoke(parameter);
                }
            }
        }


        public void AddFunc<R>(string eventType, Func<R> func)
        {
            Type returnType = typeof(R);
            if (!eventDictionary_NoParameterHaveReturn.ContainsKey(eventType))
            {
                eventDictionary_NoParameterHaveReturn.Add(eventType, new());
            }
            if (!eventDictionary_NoParameterHaveReturn[eventType].ContainsKey(returnType))
            {
                eventDictionary_NoParameterHaveReturn[eventType].Add(returnType, new());
            }
            eventDictionary_NoParameterHaveReturn[eventType][returnType].Add(func);
        }

        public void RemoveFunc<R>(string eventType, Func<R> func)
        {
            Type returnType = typeof(R);
            if (eventDictionary_NoParameterHaveReturn.ContainsKey(eventType) && eventDictionary_NoParameterHaveReturn[eventType].ContainsKey(returnType))
            {
                eventDictionary_NoParameterHaveReturn[eventType][returnType].Remove(func);
            }
        }

        public List<R> CallFunc<R>(string eventType)
        {
            Type returnType = typeof(R);
            List<R> result = new();
            if (eventDictionary_NoParameterHaveReturn.ContainsKey(eventType) && eventDictionary_NoParameterHaveReturn[eventType].ContainsKey(returnType))
            {
                foreach (Func<R> action in eventDictionary_NoParameterHaveReturn[eventType][returnType])
                {
                    if (action != null)
                    {
                        var res = action.Invoke();
                        result.Add(res);
                    }
                }
            }
            return result;
        }
        public void AddFunc<T, R>(string eventType, Func<T, R> func)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);
            if (!eventDictionary_HaveParameterHaveReturn.ContainsKey(eventType))
            {
                eventDictionary_HaveParameterHaveReturn.Add(eventType, new());
            }
            if (!eventDictionary_HaveParameterHaveReturn[eventType].ContainsKey(type))
            {
                eventDictionary_HaveParameterHaveReturn[eventType].Add(type, new());
            }
            eventDictionary_HaveParameterHaveReturn[eventType][type].Add(func);
        }
        public void RemoveFunc<T, R>(string eventType, Func<T, R> func)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);
            if (eventDictionary_HaveParameterHaveReturn.ContainsKey(eventType) && eventDictionary_HaveParameterHaveReturn[eventType].ContainsKey(type))
            {
                eventDictionary_HaveParameterHaveReturn[eventType][type].Remove(func);
            }
        }

        public List<R> CallFunc<T, R>(string eventType, T parameter)
        {
            Type parameterType = typeof(T);
            Type returnType = typeof(R);
            var type = (parameterType, returnType);

            List<R> result = new();
            if (eventDictionary_HaveParameterHaveReturn.ContainsKey(eventType) && eventDictionary_HaveParameterHaveReturn[eventType].ContainsKey(type))
            {
                foreach (Func<T, R> action in eventDictionary_HaveParameterHaveReturn[eventType][type])
                {
                    if (action != null)
                    {
                        var res = action.Invoke(parameter);
                        result.Add(res);
                    }
                }
            }
            return result;
        }

        public void Clear()
        {
            eventDictionary_NoParameter.Clear();
            eventDictionary_HaveParameter.Clear();
            eventDictionary_NoParameterHaveReturn.Clear();
            eventDictionary_HaveParameterHaveReturn.Clear();
        }
    }
}
