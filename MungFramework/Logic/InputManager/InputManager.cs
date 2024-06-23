using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CNT = UnityEngine.InputSystem.InputAction.CallbackContext;


namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 按键
    /// </summary>
    public enum InputTypeKey
    {
        NUM1, NUM2, NUM3, NUM4, NUM5, NUM6, NUM7, NUM8, NUM9, NUM0,
        ESC, TAB, CAPS,
        LEFT_SHIFT, LEFT_CONTROLL, LEFT_ALT,
        RIGTH_SHIFT, RIGHT_CONTROLL, RIGHT_ALT,
        SPACE, ENTER, BACK,
        HOME, END, PAGE_UP, PAGE_DOWN,
        ARROW_LEFT, ARROW_RIGHT, ARROW_UP, ARROW_DOWN,
        Q, W, E, R, T, Y, U, I, O, P,
        A, S, D, F, G, H, J, K, L,
        Z, X, C, V, B, N, M,


        GP_LEFT, GP_RIGHT, GP_UP, GP_DOWN,
        GP_A, GP_B, GP_X, GP_Y,
        GP_LEFT_SHOULDER, GP_RIGHT_SHOULDER,
        GP_LEFT_TRIGGER, GP_RIGHT_TRIGGER,
        GP_SELECT, GP_START,
    }

    /// <summary>
    /// 输入值
    /// </summary>
    public enum InputTypeValue
    {
        NONE,
        LEFT, UP, DOWN, RIGHT,
        OK, CANCEL,
    }

    /// <summary>
    /// 输入设备
    /// </summary>
    public enum InputDeviceEnum
    {
        键鼠, 手柄
    }



    /// <summary>
    /// 输入管理器
    /// </summary>
    public class InputManager : GameSaveableManager
    {
        /// <summary>
        /// 每个按键对应输入事件的映射
        /// </summary>
        [SerializeField]
        private InputMap @InputMap;

        private InputSource @InputSource;

        public InputDeviceEnum InputDevice;


        [ShowInInspector]
        [ReadOnly]
        private List<IInputAcceptor> InputAcceptorStack = new();

        private Dictionary<InputTypeValue, UnityEvent> InputActions_Performerd = new();

        private Dictionary<InputTypeValue, UnityEvent> InputActions_Canceled = new();



        /// <summary>
        /// 将输入接收器推入栈
        /// </summary>
        /// <param name="acceptor"></param>
        public void PushInputAcceptor(IInputAcceptor acceptor)
        {
            InputAcceptorStack.Insert(0, acceptor);
        }

        /// <summary>
        /// 将输入接收器弹出栈
        /// </summary>
        /// <param name="acceptor"></param>
        public void PopInputAcceptor(IInputAcceptor acceptor)
        {
            InputAcceptorStack.Remove(acceptor);
        }


        public override void OnSceneLoad(GameManager parentManager)
        {
            base.OnSceneLoad(parentManager);

            InputMap = new();

            InputSource = new();
            InputSource.Enable();

            //从存档中读取设置
            Load();

            //初始化输入
            InitInput();
        }

        public override void OnGameUpdate(GameManager parentManage)
        {
            base.OnGameUpdate(parentManage);

            //切换键鼠或者鼠标
            if (Keyboard.current != null && Keyboard.current.anyKey.IsPressed())
            {
                InputDevice = InputDeviceEnum.键鼠;
            }
            else if (Gamepad.current != null && Gamepad.current.IsActuated())
            {
                InputDevice = InputDeviceEnum.手柄;
            }
        }


        public override void Load()
        {
            var saveLoad = SaveManager.GetSystemValue("KEYMAP");
            //如果没有输入的存档文件，那么初始化，否则读取输入的存档文件
            if (saveLoad.Item2 == false)
            {
                InputMap.DefaultInputMap();
                SaveManager.SetSystemValue("KEYMAP", JsonUtility.ToJson(InputMap));
            }
            else
            {
                InputMap = JsonUtility.FromJson<InputMap>(saveLoad.Item1);
            }
        }
        public override void Save()
        {
            SaveManager.SetSystemValue("KEYMAP", JsonUtility.ToJson(InputMap));
        }



        /// <summary>
        /// 是否按下上下左右
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private bool isUp, isDown, isLeft, isRight;

        /// <summary>
        /// 返回移动轴
        /// </summary>
        public Vector2 MoveAxis
        {
            get
            {
                Vector2 res = Vector2.zero;
                if (InputDevice == InputDeviceEnum.手柄)
                {
                    if (InputSource != null)
                    {
                        res = InputSource.Controll.MoveAxis.ReadValue<Vector2>();
                    }
                }
                else
                {
                    if (isUp)
                        res.x += 1;
                    if (isDown)
                        res.x -= 1;
                    if (isRight)
                        res.y += 1;
                    if (isLeft)
                        res.y -= 1;
                }
                if (res.magnitude > 1)
                {
                    res.Normalize();
                }
                return res;
            }
        }

        /// <summary>
        /// 返回视角轴
        /// </summary>
        public Vector2 ViewAxis => InputSource == null ? Vector2.zero : InputSource.Controll.ViewAxis.ReadValue<Vector2>();


        /// <summary>
        /// 初始化输入事件
        /// </summary>
        private void InitInput()
        {
            var inputactions = InputSource.GetEnumerator();
            while (inputactions.MoveNext())
            {
                var action = inputactions.Current;
                var mapname = action.actionMap.name;
                var actionname = action.name;

                if (mapname == "Controll")
                {
                    if (Enum.IsDefined(typeof(InputTypeKey), actionname))
                    {
                        action.performed += (CNT cnt) => { InputAction_Performerd((InputTypeKey)Enum.Parse(typeof(InputTypeKey), actionname)); };
                        action.canceled += (CNT cnt) => { InputAction_Canceled((InputTypeKey)Enum.Parse(typeof(InputTypeKey), actionname)); };
                    }
                }
            }

            AddMoveAxisBind();
        }

        /// <summary>
        /// 添加移动轴的绑定
        /// </summary>
        private void AddMoveAxisBind()
        {
            Add_ControllInputAction_Performerd(InputTypeValue.UP, () => isUp = true);
            Add_ControllInputAction_Performerd(InputTypeValue.DOWN, () => isDown = true);
            Add_ControllInputAction_Performerd(InputTypeValue.LEFT, () => isLeft = true);
            Add_ControllInputAction_Performerd(InputTypeValue.RIGHT, () => isRight = true);

            Add_ControllInputAction_Canceled(InputTypeValue.UP, () => isUp = false);
            Add_ControllInputAction_Canceled(InputTypeValue.DOWN, () => isDown = false);
            Add_ControllInputAction_Canceled(InputTypeValue.LEFT, () => isLeft = false);
            Add_ControllInputAction_Canceled(InputTypeValue.RIGHT, () => isRight = false);
        }

        /// <summary>
        /// 为某个输入添加按下事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void Add_ControllInputAction_Performerd(InputTypeValue type, UnityAction action)
        {
            if (!InputActions_Performerd.ContainsKey(type))
            {
                InputActions_Performerd.Add(type, new());
            }
            InputActions_Performerd[type].AddListener(action);
        }

        /// <summary>
        /// 移除某个输入的按下事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void Remove_ControllInputAction_Performerd(InputTypeValue type, UnityAction action)
        {
            if (InputActions_Performerd.ContainsKey(type))
            {
                InputActions_Performerd[type].RemoveListener(action);
                Debug.Log("RemovePerformed" + action);
            }
        }

        /// <summary>
        /// 为某个输入添加取消事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void Add_ControllInputAction_Canceled(InputTypeValue type, UnityAction action)
        {
            if (!InputActions_Canceled.ContainsKey(type))
            {
                InputActions_Canceled.Add(type, new());
            }
            InputActions_Canceled[type].AddListener(action);
        }

        /// <summary>
        /// 移除某个输入的取消事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public void Remove_ControllInputAction_Canceled(InputTypeValue type, UnityAction action)
        {
            if (InputActions_Canceled.ContainsKey(type))
            {
                InputActions_Canceled[type].RemoveListener(action);
            }
        }



        /// <summary>
        /// 按下事件
        /// </summary>
        /// <param name="inputkey"></param>
        private void InputAction_Performerd(InputTypeKey inputkey)
        {
            //根据按键的key获取输入值
            foreach (var iv in InputMap.GetInputValue(inputkey))
            {
                if (InputActions_Performerd.ContainsKey(iv))
                {
                    InputActions_Performerd[iv].Invoke();
                }

                if (InputAcceptorStack.Count > 0)
                {
                    InputAcceptorStack[0].OnInput(iv);
                }
            }
        }



        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="inputkey"></param>
        private void InputAction_Canceled(InputTypeKey inputkey)
        {
            //根据按键的key获取输入值
            foreach (var iv in InputMap.GetInputValue(inputkey))
            {
                if (InputActions_Canceled.ContainsKey(iv))
                {
                    InputActions_Canceled[iv].Invoke();
                }
            }
        }


        /// <summary>
        /// 默认震动
        /// </summary>
        /// <param name="time"></param>
        public void DefaultMoter(float time)
        {
            StartCoroutine(_DefaultMoter(time));
        }

        private IEnumerator _DefaultMoter(float time)
        {
            var gamepad = Gamepad.current;
            Moter(gamepad, 1f, 1f);
            yield return new WaitForSeconds(time);
            CancelMoter(gamepad);
        }


        /// <summary>
        /// 震动
        /// </summary>
        public void Moter(Gamepad pad, float _low, float _max)
        {
            if (pad == null)
            {
                return;
            }
            pad.SetMotorSpeeds(_low, _max);
        }

        /// <summary>
        /// 取消震动
        /// </summary>
        public void CancelMoter(Gamepad pad)
        {
            if (pad == null)
            {
                return;
            }
            pad.SetMotorSpeeds(0, 0);
        }

    }
}

