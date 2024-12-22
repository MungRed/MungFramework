using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 输入映射,Key不重复
    /// </summary>
    [Serializable]
    public class InputMapLayer : MungFramework.Model.Model
    {
        public string InputMapLayerName;

        [SerializeField]
        public List<InputMapKeyValuePair> InputMapList = new();

        /// <summary>
        /// 获取某个键对应的输入值
        /// </summary>
        public InputValueEnum GetInputValue(InputKeyEnum key)
        {
            var find = InputMapList.Find(x => x.InputKey == key);
            return find?.InputValue ?? InputValueEnum.NONE;
        }

        public IEnumerable<InputKeyEnum> GetInputKey(InputValueEnum value)
        {
            return InputMapList.Where(x => x.InputValue == value).Select(x => x.InputKey);
        }

        /// <summary>
        /// 给某个值绑定按键
        /// </summary>
        public bool AddBind(InputKeyEnum key, InputValueEnum value)
        {
            if (InputMapList.Exists(x => x.InputKey == key))
            {
                Debug.LogError("按键重复！");
                return false;
            }
            else
            {
                InputMapList.Add(new(key, value));
                return true;
            }
        }

        /// <summary>
        /// 改变值的按键
        /// </summary>
        public bool ChangeBind(InputKeyEnum oldkey, InputKeyEnum newkey, InputValueEnum value)
        {
            if (oldkey == newkey)
            {
                return true;
            }

            var oldBind = InputMapList.Find(x => x.InputKey == oldkey && x.InputValue == value);
            if (oldBind == null)
            {
                return AddBind(newkey, value);
            }
            else
            {
                if (InputMapList.Exists(x => x.InputKey == newkey))
                {
                    Debug.LogError("按键重复！");
                    return false;
                }
                else
                {
                    oldBind.InputKey = newkey;
                    return true;
                }
            }
        }

        /// <summary>
        /// 是否有某个按键的绑定
        /// </summary>
        public bool HasBind(InputKeyEnum key, InputValueEnum value)
        {
            return InputMapList.Exists(x => x.InputKey == key && x.InputValue == value);
        }
    }
}
