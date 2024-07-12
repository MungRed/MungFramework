using MungFramework.Demo;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic.Input
{
    public abstract class InputDataManagerAbstract : SavableGameManagerAbstract
    {
        /// <summary>
        /// 每个按键对应输入事件的映射
        /// </summary>
        [SerializeField]
        public InputMap InputMap;

        public InputSource InputSource;

        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            InputMap = new();
            InputSource = new();
            InputSource.Enable();
            yield return Load();
        }

        /// <summary>
        /// 返回移动轴
        /// </summary>
        public Vector2 MoveAxis
        {
            get
            {
                Vector2 res = InputSource.Controll.MoveAxis.ReadValue<Vector2>();
                return res.normalized;
            }
        }
        /// <summary>
        /// 返回视角轴
        /// </summary>
        public Vector2 ViewAxis => InputSource == null ? Vector2.zero : InputSource.Controll.ViewAxis.ReadValue<Vector2>();






        public override IEnumerator Load()
        {
            var saveLoad = DemoSaveManager.Instance.GetSystemValue("KEYMAP");

            //如果没有输入的存档文件，那么初始化，否则读取输入的存档文件
            if (saveLoad.hasValue == false)
            {
                InputMap.DefaultInputMap();
                DemoSaveManager.Instance.SetSystemValue("KEYMAP", JsonUtility.ToJson(InputMap));
            }
            else
            {
                InputMap = JsonUtility.FromJson<InputMap>(saveLoad.value);
            }

            yield return null;
        }

        public override IEnumerator Save()
        {
            DemoSaveManager.Instance.SetSystemValue("KEYMAP", JsonUtility.ToJson(InputMap));

            yield return null;
        }
    }
}