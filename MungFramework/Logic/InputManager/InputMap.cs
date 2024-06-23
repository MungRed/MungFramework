using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 输入映射
    /// </summary>
    [Serializable]
    public class InputMap
    {
        [Serializable]
        public class InputMapItem
        {
            public InputTypeKey key;
            public InputTypeValue val;

            public InputMapItem(InputTypeKey key, InputTypeValue val)
            {
                this.key = key;
                this.val = val;
            }
        }

        [SerializeField]
        [ReadOnly]
        private List<InputMapItem> InputMapItems = new();

        /// <summary>
        /// 获取某个键对应的输入值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<InputTypeValue> GetInputValue(InputTypeKey key)
        {
            return InputMapItems.Where(x => x.key == key).Select(x => x.val);
        }

        /// <summary>
        /// 默认输入映射
        /// </summary>
        public void DefaultInputMap()
        {
            InputMapItems.Clear();

            InputMapItems.Add(new(InputTypeKey.W, InputTypeValue.UP));
            InputMapItems.Add(new(InputTypeKey.A, InputTypeValue.LEFT));
            InputMapItems.Add(new(InputTypeKey.S, InputTypeValue.DOWN));
            InputMapItems.Add(new(InputTypeKey.D, InputTypeValue.RIGHT));

            InputMapItems.Add(new(InputTypeKey.J, InputTypeValue.OK));
            InputMapItems.Add(new(InputTypeKey.K, InputTypeValue.CANCEL));
        }


    }
}
