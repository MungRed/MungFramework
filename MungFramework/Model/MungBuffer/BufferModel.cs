using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MungFramework.Model.MungBuffer
{
    /// <summary>
    /// 缓存数据模型，用于缓存提前加载的图片等数据
    /// 将该模型组合到相应的控制器或管理器中
    /// </summary>
    [Serializable]
    public class BufferModel<T_Key, T_Value> : MungFramework.Model.Model
    {
        [SerializeField]
        private SerializedDictionary<T_Key, T_Value> buffer = new();

        public void UpdateBuffer(T_Key key, T_Value value)
        {
            if (buffer.ContainsKey(key))
            {
                buffer[key] = value;
            }
            else
            {
                buffer.Add(key, value);
            }
        }
        public IEnumerable<KeyValuePair<T_Key, T_Value>> GetAllBuffer()
        {
            return buffer;
        }

        public (bool hasValue, T_Value value) GetBuffer(T_Key key)
        {
            if (buffer.ContainsKey(key))
            {
                return (true, buffer[key]);
            }
            return (false, default);
        }

        public T_Value GetBufferOrDefault(T_Key key, Func<T_Value> value)
        {
            if (buffer.ContainsKey(key))
            {
                return buffer[key];
            }
            var res = value();
            buffer.Add(key, res);
            return res;
        }

        public void Clear()
        {
            buffer.Clear();
        }
    }
}
