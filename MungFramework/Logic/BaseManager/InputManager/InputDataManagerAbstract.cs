﻿using MungFramework.Logic.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MungFramework.Logic.Input
{
    public abstract class InputDataManagerAbstract : GameManagerAbstract
    {
        [SerializeField]
        private List<InputMapLayerDataSO> inputMapLayerDataSOList = new();

        [SerializeField]
        private List<InputMapLayer> inputMapLayerList = new();

        public InputSource InputSource;

        public bool UseMouse;

        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            InputSystem.DisableAllEnabledActions();

            InputSource = new();
            InputSource.Enable();
            Load();
        }

        public virtual IEnumerable<InputValueEnum> GetInputValues(InputKeyEnum inputKey)
        {
            foreach (var inputMap in inputMapLayerList)
            {
                yield return inputMap.GetInputValue(inputKey);
            }
        }

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




        public virtual void Load()
        {
            inputMapLayerList.Clear();
            foreach (var inputMapDataSO in inputMapLayerDataSOList)
            {
                var savename = "INPUTMAP_LAYER_" + inputMapDataSO.InputMapLayerName;
                var loadSuccess = SaveManagerAbstract.Instance.GetSystemSaveValue(savename);
                if (loadSuccess.hasValue == false)
                {
                    //Debug.Log("读取按键层" + savename + "不存在，新建");
                    inputMapLayerList.Add(new InputMapLayerSOModelStream().Stream(inputMapDataSO));

                }
                else
                {
                    //Debug.Log("读取按键层" + savename + "成功");
                    inputMapLayerList.Add(JsonUtility.FromJson<InputMapLayer>(loadSuccess.value));
                }
            }
            LoadUseMouse();
            Save();
        }

        private void LoadUseMouse()
        {
            var useMouse = SaveManagerAbstract.Instance.GetSystemSaveValue("USE_MOUSE");
            if (useMouse.hasValue)
            {
                UseMouse = bool.Parse(useMouse.value);
            }
        }

        public virtual void Save()
        {
            foreach (var inputMap in inputMapLayerList)
            {
                var savename = "INPUTMAP_LAYER_" + inputMap.InputMapLayerName;
                SaveManagerAbstract.Instance.SetSystemSaveValue(savename, JsonUtility.ToJson(inputMap));
            }
            SaveUseMouse();
        }
        public virtual void SaveUseMouse()
        {
            SaveManagerAbstract.Instance.SetSystemSaveValue("USE_MOUSE", UseMouse.ToString());
        }
    }
}