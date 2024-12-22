using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CNT = UnityEngine.InputSystem.InputAction.CallbackContext;

/*
 * Key与Value的关系：
 * Key和InputSystem静态绑定，即InputSystem检查到某个按键被按下时，会触发对应的Key
 * Key与Value动态绑定，当游戏运行中检测到Key被触发时，浏览Key绑定的Value，然后触发Value
 * Key与Value的绑定分层，例如Ui层，通用层，战斗层，每个层都有自己的Key与Value的绑定
 * 每个层中的Key不能冲突，但不同层中的Key可以冲突，例如在Ui层中，确认绑定J，取消就不能绑定J，但战斗层中的攻击可以绑定J
 * 每一层的Key和Value的关系需要通过存档系统保存
 * 
 * 输入系统的逻辑：
 * 1.输入管理器管理输入接收器栈，每次触发Key后，将对应的Value传给栈顶的接收器，接收器根据Value做出相应的操作
 * 2.除了输入栈之外，还能额外添加事件监听，当某个Value被触发时，触发对应的事件
 */

namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 按键
    /// </summary>
    public enum InputKeyEnum
    {
        NONE,
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


        MOUSE_LEFT, MOUSE_RIGHT,
    }
    /// <summary>
    /// 输入值
    /// </summary>
    public enum InputValueEnum
    {
        NONE = 0,

        LEFT = 10, UP = 11, DOWN = 12, RIGHT = 13,

        //UI
        OK = 20, CANCEL = 21, SPECIALACTION = 22,
        LEFT_PAGE = 23, RIGTH_PAGE = 24,
        UP_ROLL = 25, DOWN_ROLL = 26,

        //Option
        MENU = 40,
        MAP = 41,
        ACTION = 42,

        //Battle
        ATTACK = 60,
        SKILL = 61,
        MAGICK = 62,
        DEFENSE = 63,
        OVERDRIVE = 64,
        EXTRA = 65,
        DEPLOY = 66,
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
        [SerializeField]
        [Required("需要拖拽挂载")]
        private InputDataManagerAbstract inputDataManager;

        public InputDeviceEnum InputDevice;

        [ShowInInspector]
        [ReadOnly]
        private List<IInputAcceptor> inputAcceptorStack = new();
        private Dictionary<InputValueEnum, UnityEvent> inputActionListener_Performed = new();
        private Dictionary<InputValueEnum, UnityEvent> inputActionListener_Canceled = new();
        private UnityEvent<InputKeyEnum> inputActionListener_AnyKeyDown = new();


        #region InputAcceptorStack
        /// <summary>
        /// 将输入接收器推入栈
        /// </summary>
        public void Push_InputAcceptor(IInputAcceptor acceptor)
        {
            if (inputAcceptorStack.Count > 0 && inputAcceptorStack[0] == acceptor)
            {
                return;
            }
            inputAcceptorStack.Insert(0, acceptor);
        }

        /// <summary>
        /// 将输入接收器弹出栈
        /// </summary>
        public void Pop_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Remove(acceptor);
        }

        /// <summary>
        /// 判断某个输入接收器是否在栈顶
        /// </summary>
        public bool IsTop(IInputAcceptor acceptor)
        {
            return inputAcceptorStack.Count > 0 && inputAcceptorStack[0] == acceptor;
        }
        #endregion

        #region MungFramework
        public override void OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            //初始化输入
            InitInput();
        }

        public override void OnGameUpdate(GameManagerAbstract parentManage)
        {
            base.OnGameUpdate(parentManage);
            //切换键鼠或者鼠标
            if (Keyboard.current != null && Keyboard.current.anyKey.IsPressed())
            {
                if (InputDevice == InputDeviceEnum.手柄)
                {
                    InputDevice = InputDeviceEnum.键鼠;
                    OnChangeDevice();
                }
            }
            else if (Gamepad.current != null && Gamepad.current.IsActuated())
            {
                if (InputDevice == InputDeviceEnum.键鼠)
                {
                    InputDevice = InputDeviceEnum.手柄;
                    OnChangeDevice();
                }
            }
        }
        #endregion

        /// <summary>
        /// 当输入设备改变时
        /// </summary>
        protected virtual void OnChangeDevice()
        {

        }

        /// <summary>
        /// 初始化输入事件，即将Key与InputSystem绑定
        /// </summary>
        protected virtual void InitInput()
        {
            void InputActionPerformedHelp(CNT cnt)
            {
                InputAction_Performed((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), cnt.action.name));
            }
            void InputActionCanceledHelp(CNT cnt)
            {
                InputAction_Canceled((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), cnt.action.name));
            }
            void AddMoveAxisBind()
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

        #region KeyBind
        /// <summary>
        /// 改变按键绑定
        /// </summary>
        public virtual IEnumerator ChangeKeyBind(string inputMapName, InputKeyEnum oldkey, InputValueEnum value, InputDeviceEnum inputDevice, UnityAction<bool> changeBindCallback)
        {
            var inputMap = inputDataManager.GetInputMap(inputMapName);
            if (inputMap == null)
            {
                Debug.Log(1);
                changeBindCallback?.Invoke(false);
                yield break;
            }


            bool isKeyDown = false;
            bool changeSuccess = false;

            //如果旧键位是任意键，说明是添加键位而不是改变键位
            if (oldkey == InputKeyEnum.NONE)
            {
                UnityAction<InputKeyEnum> addBind = (InputKeyEnum newkey) =>
                {
                    isKeyDown = true;
                    if (inputDevice == InputDeviceEnum.手柄 && InputDataManagerAbstract.isGamepad(newkey))
                    {
                        changeSuccess = inputMap.AddBind(newkey, value);
                    }
                    else if (inputDevice == InputDeviceEnum.键鼠 && InputDataManagerAbstract.isKeyboard(newkey))
                    {
                        changeSuccess = inputMap.AddBind(newkey, value);
                    }
                };
                addBind += (x) => Remove_InputAction_AnyKeyDown(addBind);
                Push_InputAcceptor(null);
                Add_InputAction_AnyKeyDown(addBind);
                yield return new WaitUntil(() => isKeyDown);
            }
            else
            {
                if (!inputMap.HasBind(oldkey, value))
                {
                    Debug.LogError("按键绑定切换错误，不存在旧的绑定");
                    changeBindCallback?.Invoke(false);
                    yield break;
                }
                UnityAction<InputKeyEnum> changeBind = (InputKeyEnum newkey) =>
                {
                    isKeyDown = true;
                    if (inputDevice == InputDeviceEnum.手柄 && InputDataManagerAbstract.isGamepad(newkey))
                    {
                        changeSuccess = inputMap.ChangeBind(oldkey, newkey, value);
                    }
                    else if (inputDevice == InputDeviceEnum.键鼠 && InputDataManagerAbstract.isKeyboard(newkey))
                    {
                        changeSuccess = inputMap.ChangeBind(oldkey, newkey, value);
                    }
                };
                changeBind += (x) => Remove_InputAction_AnyKeyDown(changeBind);
                Push_InputAcceptor(null);
                Add_InputAction_AnyKeyDown(changeBind);
                yield return new WaitUntil(() => isKeyDown);
            }

            if (changeSuccess)
            {
                inputDataManager.Save();
                OnChangeKeyBind();
            }

            changeBindCallback?.Invoke(changeSuccess);
        }
        public virtual void OnChangeKeyBind()
        {

        }


        public virtual IEnumerable<InputValueEnum> GetInputValues(InputKeyEnum inputKey) => inputDataManager.GetInputValues(inputKey);
        public virtual InputValueEnum GetInputValue(string inputMapLayerName, InputKeyEnum inputKey) => inputDataManager.GetInputValue(inputMapLayerName, inputKey);

        public virtual IEnumerable<InputKeyEnum> GetInputKeys(InputValueEnum value) => inputDataManager.GetInputKeys(value);
        public virtual IEnumerable<InputKeyEnum> GetInputKeys(string inputMapLayerName, InputValueEnum inputValue) => inputDataManager.GetInputKeys(inputMapLayerName, inputValue);
        public virtual InputKeyEnum GetInputKey(string inputMapLayerName, InputValueEnum inputValue, InputDeviceEnum inputDevice) => inputDataManager.GetInputKey(inputMapLayerName, inputValue, inputDevice);
        public virtual void DefaultKeyConfig() => inputDataManager.DefaultKeyConfig();
        #endregion

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
        /// 鼠标位置
        /// </summary>
        public Vector2 MousePosition => Mouse.current.position.value;

        public bool UseMouse
        {
            get
            {
                return inputDataManager.UseMouse;
            }
            set
            {
                inputDataManager.UseMouse = value;
            }
        }
        #endregion

        #region 添加/移除 事件监听
        /// <summary>
        /// 为某个输入添加按下事件
        /// </summary>
        public virtual void Add_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (!inputActionListener_Performed.ContainsKey(type))
            {
                inputActionListener_Performed.Add(type, new());
            }
            inputActionListener_Performed[type].AddListener(action);
        }

        /// <summary>
        /// 移除某个输入的按下事件
        /// </summary>
        public virtual void Remove_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (inputActionListener_Performed.ContainsKey(type))
            {
                inputActionListener_Performed[type].RemoveListener(action);
                Debug.Log("RemovePerformed" + action);
            }
        }

        /// <summary>
        /// 为某个输入添加取消事件
        /// </summary>
        public virtual void Add_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (!inputActionListener_Canceled.ContainsKey(type))
            {
                inputActionListener_Canceled.Add(type, new());
            }
            inputActionListener_Canceled[type].AddListener(action);
        }

        /// <summary>
        /// 移除某个输入的取消事件
        /// </summary>
        public virtual void Remove_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (inputActionListener_Canceled.ContainsKey(type))
            {
                inputActionListener_Canceled[type].RemoveListener(action);
            }
        }

        /// <summary>
        /// 添加任意键按下的事件
        /// </summary>
        public virtual void Add_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            inputActionListener_AnyKeyDown.AddListener(action);
        }

        /// <summary>
        /// 移除任意键按下的事件
        /// </summary>
        public virtual void Remove_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            inputActionListener_AnyKeyDown.RemoveListener(action);
        }
        #endregion

        #region 按下和取消事件
        /// <summary>
        /// 按下事件
        /// </summary>
        protected virtual void InputAction_Performed(InputKeyEnum inputkey)
        {
            if (inputkey.ToString().Contains("MOUSE") && !UseMouse)
            {
                return;
            }
            //Debug.Log(inputkey);
            //触发一次任意键按下的事件
            InputAction_AnyKeyDown(inputkey);


            IInputAcceptor inputAcceptor = null;


            if (inputAcceptorStack.Count > 0)
            {
                //如果栈顶的接收器不为空，就使用栈顶的接收器
                if (inputAcceptorStack[0] != null)
                {
                    inputAcceptor = inputAcceptorStack[0];
                }
                //否则移除栈顶的接收器，然后跳过这次输入
                else
                {
                    inputAcceptorStack.RemoveAt(0);
                    return;
                }
            }

            //用一个HashSet来存储已经触发的Value，防止重复触发
            HashSet<InputValueEnum> valueHashSet = new();
            //根据按键的key获取输入值
            foreach (var inputValue in inputDataManager.GetInputValues(inputkey))
            {
                if (inputValue != InputValueEnum.NONE && valueHashSet.Add(inputValue))
                {
                    if (inputActionListener_Performed.ContainsKey(inputValue))
                    {
                        inputActionListener_Performed[inputValue].Invoke();
                    }
                    inputAcceptor?.OnInput(inputValue);
                }
            }
        }


        /// <summary>
        /// 取消事件
        /// </summary>
        protected virtual void InputAction_Canceled(InputKeyEnum inputkey)
        {
            if (inputkey.ToString().Contains("MOUSE") && !UseMouse)
            {
                return;
            }
            //根据按键的key获取输入值
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                if (inputActionListener_Canceled.ContainsKey(inputvalue))
                {
                    inputActionListener_Canceled[inputvalue].Invoke();
                }
            }
        }

        /// <summary>
        /// 任意键按下
        /// </summary>
        protected virtual void InputAction_AnyKeyDown(InputKeyEnum inputKey)
        {
            inputActionListener_AnyKeyDown.Invoke(inputKey);
        }
        #endregion

        #region 手柄震动
        /// <summary>
        /// 默认震动
        /// </summary>
        public virtual void DefaultMoter(float time)
        {
            IEnumerator defaultMoter(float time)
            {
                var gamepad = Gamepad.current;
                Moter(gamepad, 1f, 1f);
                yield return new WaitForSeconds(time);
                CancelMoter(gamepad);
            }

            if (InputDevice != InputDeviceEnum.手柄)
            {
                return;
            }
            StartCoroutine(defaultMoter(time));
        }

        /// <summary>
        /// 震动
        /// </summary>
        public virtual void Moter(Gamepad pad, float low, float max)
        {
            if (InputDevice != InputDeviceEnum.手柄)
            {
                return;
            }
            if (pad == null)
            {
                return;
            }
            pad.SetMotorSpeeds(low, max);
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

