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
    public class InputMap : MungFramework.Model.Model
    {
        [Serializable]
        public class InputMapItem
        {
            public InputKeyEnum InputKey;
            public InputValueEnum InputValue;

            public InputMapItem(InputKeyEnum inputKey, InputValueEnum inputValue)
            {
                InputKey = inputKey;
                InputValue = inputValue;
            }
        }

        [SerializeField]
        [ReadOnly]
        private List<InputMapItem> InputMapItemList = new();

        /// <summary>
        /// 获取某个键对应的输入值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<InputValueEnum> GetInputValue(InputKeyEnum key)
        {
            return InputMapItemList.Where(x => x.InputKey == key).Select(x => x.InputValue);
        }

        /// <summary>
        /// 默认输入映射
        /// </summary>
        public void DefaultInputMap()
        {
            InputMapItemList.Clear();

            InputMapItemList.Add(new(InputKeyEnum.W, InputValueEnum.UP));
            InputMapItemList.Add(new(InputKeyEnum.A, InputValueEnum.LEFT));
            InputMapItemList.Add(new(InputKeyEnum.S, InputValueEnum.DOWN));
            InputMapItemList.Add(new(InputKeyEnum.D, InputValueEnum.RIGHT));

            InputMapItemList.Add(new(InputKeyEnum.J, InputValueEnum.OK));
            InputMapItemList.Add(new(InputKeyEnum.K, InputValueEnum.CANCEL));
        }


    }
}
