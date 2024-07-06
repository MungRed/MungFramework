using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

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
            //代表某个按键会触发某个值
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
            //Debug.Log(key);
            //Debug.Log(InputMapItemList.Where(x => x.InputKey == key).Count());
            return InputMapItemList.Where(x => x.InputKey == key).Select(x => x.InputValue);
        }
        public IEnumerable<InputKeyEnum> GetInputKey(InputValueEnum value)
        {
            return InputMapItemList.Where(x=>x.InputValue==value).Select(x => x.InputKey);
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
            InputMapItemList.Add(new(InputKeyEnum.Q, InputValueEnum.LEFT_PAGE));
            InputMapItemList.Add(new(InputKeyEnum.E, InputValueEnum.RIGTH_PAGE));
            InputMapItemList.Add(new(InputKeyEnum.O, InputValueEnum.UP_ROLL));
            InputMapItemList.Add(new(InputKeyEnum.P, InputValueEnum.DOWN_ROLL));



            InputMapItemList.Add(new(InputKeyEnum.GP_UP, InputValueEnum.UP));
            InputMapItemList.Add(new(InputKeyEnum.GP_LEFT, InputValueEnum.LEFT));
            InputMapItemList.Add(new(InputKeyEnum.GP_DOWN, InputValueEnum.DOWN));
            InputMapItemList.Add(new(InputKeyEnum.GP_RIGHT, InputValueEnum.RIGHT));

            InputMapItemList.Add(new(InputKeyEnum.GP_A, InputValueEnum.OK));
            InputMapItemList.Add(new(InputKeyEnum.GP_B, InputValueEnum.CANCEL));
            InputMapItemList.Add(new(InputKeyEnum.GP_LEFT_SHOULDER, InputValueEnum.LEFT_PAGE));
            InputMapItemList.Add(new(InputKeyEnum.GP_RIGHT_SHOULDER, InputValueEnum.RIGTH_PAGE));
            InputMapItemList.Add(new(InputKeyEnum.GP_LEFT_TRIGGER, InputValueEnum.UP_ROLL));
            InputMapItemList.Add(new(InputKeyEnum.GP_RIGHT_TRIGGER, InputValueEnum.DOWN_ROLL));
        }

        /// <summary>
        /// 给某个值绑定按键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddBind(InputKeyEnum key, InputValueEnum value)
        {
            if (InputMapItemList.Find(x => x.InputKey==key&&x.InputValue==value) != null)
            {
                Debug.LogError("已经有按键绑定了这个值");
                return false;
            }

            InputMapItemList.Add(new(key, value));
            return true;
        }
        /// <summary>
        /// 改变值的按键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ChangeBind(InputKeyEnum oldkey,InputKeyEnum newkey,InputValueEnum value)
        {
            var oldBind = InputMapItemList.Find(x => x.InputKey==oldkey&&x.InputValue == value);
            if (oldBind == null)
            {
                AddBind(newkey, value);
                return true;
            }
            oldBind.InputKey = newkey;
            return true;
        }

        /// <summary>
        /// 是否有某个按键的绑定
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HasBind(InputKeyEnum key, InputValueEnum value)
        {
            return InputMapItemList.Find(x => x.InputKey == key && x.InputValue == value) != null;
        }
    }
}
