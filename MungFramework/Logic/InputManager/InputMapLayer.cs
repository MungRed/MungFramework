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
        public  List<InputMapKeyValuePair> InputMapList = new();

        /// <summary>
        /// 获取某个键对应的输入值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public InputValueEnum GetInputValue(InputKeyEnum key)
        {
            var find = InputMapList.Find(x => x.InputKey == key);
            return find?.InputValue??InputValueEnum.NONE;
        }
        public IEnumerable<InputKeyEnum> GetInputKey(InputValueEnum value)
        {
            return InputMapList.Where(x=>x.InputValue==value).Select(x => x.InputKey);
        }

        /// <summary>
        /// 默认输入映射
        /// </summary>
/*        public void DefaultInputMap()
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
        }*/


        /// <summary>
        /// 给某个值绑定按键
        /// </summary>
        public bool AddBind(InputKeyEnum key, InputValueEnum value)
        {
            if (InputMapList.Find(x => x.InputKey==key) != null)
            {
                Debug.LogError("按键重复！");
                return false;
            }

            InputMapList.Add(new(key, value));
            return true;
        }

        /// <summary>
        /// 改变值的按键
        /// </summary>
        public bool ChangeBind(InputKeyEnum oldkey,InputKeyEnum newkey,InputValueEnum value)
        {
            var oldBind = InputMapList.Find(x => x.InputKey==oldkey&&x.InputValue == value);
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
        public bool HasBind(InputKeyEnum key, InputValueEnum value)
        {
            return InputMapList.Find(x => x.InputKey == key && x.InputValue == value) != null;
        }
    }
}
