using MungFramework.Logic.Save;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 输入数据管理器
    /// </summary>
    public abstract class InputDataManagerAbstract : GameManagerAbstract
    {
        [SerializeField]
        private List<InputMapLayerDataSO> inputMapLayerDataSOList = new();

        [SerializeField]
        private List<InputMapLayer> inputMapLayerList = new();

        public InputSource InputSource;

        private bool useMouse;
        public bool UseMouse
        {
            get
            {
                return useMouse;
            }
            set
            {
                useMouse = value;
                SaveManagerAbstract.Instance.SetSystemSaveValue("USE_MOUSE", useMouse ? "true" : "false");
            }
        }

        public override void OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            InputSystem.DisableAllEnabledActions();
            InputSource = new();
            InputSource.Enable();
            Load();
        }

        public static bool isKeyboard(InputKeyEnum inputKey)
        {
            return !inputKey.ToString().StartsWith("GP") && !inputKey.ToString().StartsWith("MOUSE");
        }
        public static bool isGamepad(InputKeyEnum inputKey)
        {
            return inputKey.ToString().StartsWith("GP");
        }


        #region Key&Aixs

        /// <summary>
        /// 获得某个Key在不同层绑定的所有Value
        /// </summary>
        public virtual IEnumerable<InputValueEnum> GetInputValues(InputKeyEnum inputKey)
        {
            for (int i = 0; i < inputMapLayerList.Count; i++)
            {
                yield return inputMapLayerList[i].GetInputValue(inputKey);
            }
        }
        public virtual InputValueEnum GetInputValue(string inputMapLayerName, InputKeyEnum inputKey)
        {
            var inputMap = inputMapLayerList.Find(x => x.InputMapLayerName == inputMapLayerName);
            if (inputMap == null)
            {
                return InputValueEnum.NONE;
            }
            return inputMap.GetInputValue(inputKey);
        }

        /// <summary>
        /// 获得Value在不同层绑定的所有Key
        /// </summary>
        public virtual IEnumerable<InputKeyEnum> GetInputKeys(InputValueEnum inputValue)
        {
            foreach (var inputMap in inputMapLayerList)
            {
                foreach (var inputKey in inputMap.GetInputKey(inputValue))
                {
                    yield return inputKey;
                }
            }
        }
        public virtual IEnumerable<InputKeyEnum> GetInputKeys(string inputMapLayerName, InputValueEnum inputValue)
        {
            var inputMap = inputMapLayerList.Find(x => x.InputMapLayerName == inputMapLayerName);
            if (inputMap == null)
            {
                return new List<InputKeyEnum>();
            }
            return inputMap.GetInputKey(inputValue);
        }
        public virtual InputKeyEnum GetInputKey(string inputMapLayerName, InputValueEnum inputValue, InputDeviceEnum inputDevice)
        {
            var inputMap = inputMapLayerList.Find(x => x.InputMapLayerName == inputMapLayerName);
            if (inputMap == null)
            {
                return InputKeyEnum.NONE;
            }
            var inputKeys = inputMap.GetInputKey(inputValue);
            var keybord = inputKeys.Where(x => isKeyboard(x));
            var gamepad = inputKeys.Where(x => isGamepad(x));
            if (inputDevice == InputDeviceEnum.键鼠)
            {
                if (keybord.Count() > 0)
                {
                    return keybord.First();
                }
            }
            else
            {
                if (gamepad.Count() > 0)
                {
                    return gamepad.First();
                }
            }
            return InputKeyEnum.NONE;
        }

        /// <summary>
        /// 根据inputMap名称获取inputMap
        /// </summary>
        public virtual InputMapLayer GetInputMap(string inputMapName)
        {
            return inputMapLayerList.Find(x => x.InputMapLayerName == inputMapName);
        }


        /// <summary>
        /// 返回移动轴
        /// </summary>
        public Vector2 MoveAxis
        {
            get
            {
                Vector2 res = InputSource.Controll.MoveAxis.ReadValue<Vector2>();
                if (res.sqrMagnitude > 1)
                {
                    return res.normalized;
                }
                return res;
            }
        }

        /// <summary>
        /// 返回视角轴
        /// </summary>
        public Vector2 ViewAxis
        {
            get
            {
                return InputSource == null ? Vector2.zero : InputSource.Controll.ViewAxis.ReadValue<Vector2>();
            }
        }
        #endregion

        /// <summary>
        /// 加载默认按键配置
        /// </summary>
        [Button]
        public virtual void DefaultKeyConfig()
        {
            inputMapLayerList.Clear();
            var inputMapStream = new InputMapLayerSOModelStream();
            foreach (var inputMapDataSO in inputMapLayerDataSOList)
            {
                inputMapLayerList.Add(inputMapStream.Stream(inputMapDataSO));
            }
            Save();
        }


        /// <summary>
        /// 加载按键数据
        /// </summary>
        [Button]
        public virtual void Load()
        {
            inputMapLayerList.Clear();
            var inputMapStream = new InputMapLayerSOModelStream();

            foreach (var inputMapDataSO in inputMapLayerDataSOList)
            {
                //获取存档名
                var savename = "INPUTMAP_LAYER_" + inputMapDataSO.InputMapLayerName;
                var loadSuccess = SaveManagerAbstract.Instance.GetSystemSaveValue(savename);
                //如果有按键的储存信息，则通过储存信息初始化按键，否则通过SO初始化按键
                if (loadSuccess.hasValue)
                {
                    inputMapLayerList.Add(JsonUtility.FromJson<InputMapLayer>(loadSuccess.value));
                }
                else
                {
                    inputMapLayerList.Add(inputMapStream.Stream(inputMapDataSO));
                }
            }
            //加载是否使用鼠标
            var useMouse = SaveManagerAbstract.Instance.GetSystemSaveValue("USE_MOUSE");
            UseMouse = useMouse.hasValue && useMouse.value == "true";
            Save();
        }

        [Button]
        public virtual void Save()
        {
            foreach (var inputMap in inputMapLayerList)
            {
                var savename = "INPUTMAP_LAYER_" + inputMap.InputMapLayerName;
                Debug.Log("Save:" + savename);
                SaveManagerAbstract.Instance.SetSystemSaveValue(savename, JsonUtility.ToJson(inputMap));
            }
        }
    }
}