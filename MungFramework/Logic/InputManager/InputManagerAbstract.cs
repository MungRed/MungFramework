using MungFramework.ActionTreeEditor;
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
    public enum InputKeyEnum
    {
        ANYKEY,
        NUM1, NUM2, NUM3, NUM4, NUM5, NUM6, NUM7, NUM8, NUM9, NUM0,
        ESC, TAB, CAPS,
        LEFT_SHIFT, LEFT_CONTROLL, LEFT_ALT,
        RIGHT_SHIFT, RIGHT_CONTROLL, RIGHT_ALT,
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
    public enum InputValueEnum
    {
        NONE,
        ANYKEY,
        LEFT, UP, DOWN, RIGHT,
        OK, CANCEL,
        LEFT_PAGE, RIGTH_PAGE,
        UP_ROLL, DOWN_ROLL,
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
    public abstract class InputManagerAbstract : SingletonGameManagerAbstract<InputManagerAbstract>
    {
#if UNITY_EDITOR
        private bool isBind
        {
            get
            {            
                bool res = !(inputDataManager!=null&&subGameManagerList.Contains(inputDataManager));
                if (res == true)
                {
                    Debug.LogError(name + "需要挂载子管理器");
                }
                return res;
            }
        }
#endif


        [SerializeField]
        [Required("需要拖拽挂载")]
        [InfoBox("需要拖到子管理器中","isBind", InfoMessageType = InfoMessageType.Error)]
        private InputDataManagerAbstract inputDataManager;

        public InputDeviceEnum InputDevice;


        [ShowInInspector]
        [ReadOnly]
        private List<IInputAcceptor> inputAcceptorStack = new();

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Performed = new();

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Canceled = new();

        private UnityEvent<InputKeyEnum> InputActionListener_AnyKeyDown = new();


        /// <summary>
        /// 将输入接收器推入栈
        /// </summary>
        /// <param name="acceptor"></param>
        public void Push_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Insert(0, acceptor);
        }

        /// <summary>
        /// 将输入接收器弹出栈
        /// </summary>
        /// <param name="acceptor"></param>
        public void Pop_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Remove(acceptor);
        }


        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);    
            //初始化输入
            InitInput();
        }

        public override void OnGameUpdate(GameManagerAbstract parentManage)
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



        /// <summary>
        /// 初始化输入事件
        /// </summary>
        protected virtual void InitInput()
        {
            var inputactions = inputDataManager.InputSource.GetEnumerator();
            while (inputactions.MoveNext())
            {
                var action = inputactions.Current;
                var mapname = action.actionMap.name;
                var actionname = action.name;

                if (mapname == "Controll")
                {
                    if (Enum.IsDefined(typeof(InputKeyEnum), actionname))
                    {
                        action.performed += InputActionPerformedHelp;
                        action.canceled += InputActionCanceledHelp;
                    }
                }
            }

            AddMoveAxisBind();
        }
        private void InputActionPerformedHelp(CNT cnt)
        {
            InputAction_Performed((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), cnt.action.name));
        }
        private void InputActionCanceledHelp(CNT cnt)
        {
            InputAction_Canceled((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), cnt.action.name));
        }

        /// <summary>
        /// 改变按键绑定
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IEnumerator ChangeKeyBind(string inputMapName,InputKeyEnum oldkey, InputValueEnum value)
        {
            var inputMap = inputDataManager.GetInputMap(inputMapName);
            if (inputMap == null)
            {
                yield break;
            }

            //如果旧键位是任意键，说明是添加键位而不是改变键位
            if (oldkey == InputKeyEnum.ANYKEY)
            {
                bool added = false;
                UnityAction<InputKeyEnum> addBind = (InputKeyEnum newkey) =>
                {
                    inputMap.AddBind(newkey, value);
                    added = true;
                };

                Add_InputAction_AnyKeyDown(addBind);

                yield return new WaitUntil(() => added);

                Remove_InputAction_AnyKeyDown(addBind);
            }
            else
            {
                if (!inputMap.HasBind(oldkey, value))
                {
                    Debug.LogError("按键绑定切换错误，不存在旧的绑定");
                    yield break;
                }

                bool changed = false;

                UnityAction<InputKeyEnum> changeBind = (InputKeyEnum newkey) =>
                {
                    inputMap.ChangeBind(oldkey, newkey, value);
                    changed = true;
                };

                Add_InputAction_AnyKeyDown(changeBind);

                yield return new WaitUntil(() => changed);

                Remove_InputAction_AnyKeyDown(changeBind);
            }
            inputDataManager.Save();
        }

        public virtual IEnumerable<InputKeyEnum> GetCurrentBind(InputValueEnum value)
        {
            return inputDataManager.GetInputKeys(value);
        }


        #region 轴
        /// <summary>
        /// 是否按下上下左右
        /// </summary>
        [SerializeField]
        [ReadOnly]
        protected bool IsUp, IsDown, IsLeft, IsRight;




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
                    return inputDataManager.MoveAxis;
                }
                else
                {
                    if (IsUp)
                        res.y += 1;
                    if (IsDown)
                        res.y -= 1;
                    if (IsRight)
                        res.x += 1;
                    if (IsLeft)
                        res.x -= 1;
                }

                if (res.sqrMagnitude > 1)
                {
                    return res.normalized;
                }
                else
                {
                    return res;
                }
            }
        }

        /// <summary>
        /// 返回视角轴
        /// </summary>
        public Vector2 ViewAxis => inputDataManager.ViewAxis;

        /// <summary>
        /// 添加移动轴的绑定
        /// </summary>
        protected virtual void AddMoveAxisBind()
        {
            Add_InputAction_Performed(InputValueEnum.UP, () => IsUp = true);
            Add_InputAction_Performed(InputValueEnum.DOWN, () => IsDown = true);
            Add_InputAction_Performed(InputValueEnum.LEFT, () => IsLeft = true);
            Add_InputAction_Performed(InputValueEnum.RIGHT, () => IsRight = true);

            Add_InputAction_Canceled(InputValueEnum.UP, () => IsUp = false);
            Add_InputAction_Canceled(InputValueEnum.DOWN, () => IsDown = false);
            Add_InputAction_Canceled(InputValueEnum.LEFT, () => IsLeft = false);
            Add_InputAction_Canceled(InputValueEnum.RIGHT, () => IsRight = false);
        }
        #endregion

        #region 添加/移除 事件监听

        /// <summary>
        /// 为某个输入添加按下事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Add_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (!InputActionListener_Performed.ContainsKey(type))
            {
                InputActionListener_Performed.Add(type, new());
            }
            InputActionListener_Performed[type].AddListener(action);
        }
        /// <summary>
        /// 移除某个输入的按下事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Remove_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (InputActionListener_Performed.ContainsKey(type))
            {
                InputActionListener_Performed[type].RemoveListener(action);
                Debug.Log("RemovePerformed" + action);
            }
        }
        /// <summary>
        /// 为某个输入添加取消事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public  virtual void Add_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (!InputActionListener_Canceled.ContainsKey(type))
            {
                InputActionListener_Canceled.Add(type, new());
            }
            InputActionListener_Canceled[type].AddListener(action);
        }
        /// <summary>
        /// 移除某个输入的取消事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Remove_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (InputActionListener_Canceled.ContainsKey(type))
            {
                InputActionListener_Canceled[type].RemoveListener(action);
            }
        }
        /// <summary>
        /// 添加任意键按下的事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Add_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.AddListener(action);
        }
        /// <summary>
        /// 移除任意键按下的事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public  virtual void Remove_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.RemoveListener(action);
        }

        #endregion

        #region 按下和取消事件
        /// <summary>
        /// 按下事件
        /// </summary>
        /// <param name="inputkey"></param>
        protected virtual void InputAction_Performed(InputKeyEnum inputkey)
        {
            // Debug.Log(inputkey);
            //如果不是任意键，就触发一次任意键按下的事件
            if (inputkey != InputKeyEnum.ANYKEY)
            {
                InputAction_AnyKeyDown(inputkey);
            }

            //根据按键的key获取输入值
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                if (InputActionListener_Performed.ContainsKey(inputvalue))
                {
                    InputActionListener_Performed[inputvalue].Invoke();
                }

                if (inputAcceptorStack.Count > 0)
                {
                    inputAcceptorStack[0].OnInput(inputvalue);
                }
            }
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="inputkey"></param>
        protected virtual void InputAction_Canceled(InputKeyEnum inputkey)
        {
            //根据按键的key获取输入值
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                if (InputActionListener_Canceled.ContainsKey(inputvalue))
                {
                    InputActionListener_Canceled[inputvalue].Invoke();
                }
            }
        }
        /// <summary>
        /// 任意键按下，但包括InputkeyEnum中的任意键
        /// </summary>
        /// <param name="inputKey"></param>
        protected virtual void InputAction_AnyKeyDown(InputKeyEnum inputKey)
        {
            //Debug.Log("anykeydown " + inputKey);
            InputActionListener_AnyKeyDown.Invoke(inputKey);
        }
        #endregion

        #region 手柄震动
        /// <summary>
        /// 默认震动
        /// </summary>
        /// <param name="time"></param>
        public virtual void DefaultMoter(float time)
        {
            StartCoroutine(_DefaultMoter(time));
        }
        protected virtual IEnumerator _DefaultMoter(float time)
        {
            var gamepad = Gamepad.current;
            Moter(gamepad, 1f, 1f);
            yield return new WaitForSeconds(time);
            CancelMoter(gamepad);
        }
        /// <summary>
        /// 震动
        /// </summary>
        public virtual void Moter(Gamepad pad, float _low, float _max)
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
        public virtual void CancelMoter(Gamepad pad)
        {
            if (pad == null)
            {
                return;
            }
            pad.SetMotorSpeeds(0, 0);
        }
        #endregion
    }
}

