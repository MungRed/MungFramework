using MungFramework.Logic;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CNT = UnityEngine.InputSystem.InputAction.CallbackContext;
using IK = MungFramework.Input.InputManager.InputTypeKey;
using IV = MungFramework.Input.InputManager.InputTypeValue;

namespace MungFramework.Input
{
    public interface IInputAcceptor
    {
        public void OnInput(IV inputType);
    }

    [Serializable]
    public class InputMap
    {
        [Serializable]
        public class InputMapItem
        {
            public IK key;
            public IV val;

            public InputMapItem(IK key, IV val)
            {
                this.key = key;
                this.val = val;
            }
        }

        [SerializeField]
        [ReadOnly]
        private List<InputMapItem> InputMapItems = new();

        public IEnumerable<IV> GetInputValue(IK key)
        {
            return InputMapItems.Where(x => x.key == key).Select(x => x.val);
        }

        public void DefaultInputMap()
        {
            InputMapItems.Clear();

            InputMapItems.Add(new(IK.W, IV.UP));
            InputMapItems.Add(new(IK.A, IV.LEFT));
            InputMapItems.Add(new(IK.S, IV.DOWN));
            InputMapItems.Add(new(IK.D, IV.RIGHT));

            InputMapItems.Add(new(IK.J, IV.OK));
            InputMapItems.Add(new(IK.K, IV.CANCEL));
        }


    }

    public class InputManager : GameSaveableManager
    {
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


        public enum InputTypeValue
        {
            NONE, LEFT, UP, DOWN, RIGHT, OK, CANCEL,
        }

        public enum InputDeviceEnum
        {
            键鼠, 手柄
        }


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

        private Dictionary<IV, UnityEvent> InputActions_Performerd = new();

        private Dictionary<IV, UnityEvent> InputActions_Canceled = new();



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
            InputSource = new();
            InputSource.Enable();

            GameApplication.Instance.SaveManager.AddManager(this);
            //从存档中读取设置
            Load();

            //初始化输入
            InitInput();
        }


        public override void Load()
        {
            //如果没有输入的存档文件，那么初始化，否则读取输入的存档文件
            InputMap.DefaultInputMap();
        }
        public override void Save()
        {
            string json = JsonUtility.ToJson(InputMap);
        }




        public override void OnGameUpdate(GameManager parentManage)
        {
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

        [SerializeField]
        [ReadOnly]
        private bool isUp, isDown, isLeft, isRight;

        public Vector2 MoveAxis
        {
            get
            {
                if (InputDevice == InputDeviceEnum.手柄)
                {
                    return InputSource == null ? Vector2.zero : InputSource.Controll.MoveAxis.ReadValue<Vector2>();
                }
                else
                {
                    float v = 0f, h = 0f;
                    if (isUp && !isDown)
                    {
                        v = 1;
                    }
                    if (!isUp && isDown)
                    {
                        v = -1;
                    }
                    if (isRight && !isLeft)
                    {
                        h = 1;
                    }
                    if (!isRight && isLeft)
                    {
                        h = -1;
                    }

                    return new Vector2(v, h).normalized;
                }
            }
        }
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
                    if (Enum.IsDefined(typeof(IK), actionname))
                    {
                        action.performed += (CNT cnt) => { InputAction_Performerd((IK)Enum.Parse(typeof(IK), actionname)); };
                        action.canceled += (CNT cnt) => { InputAction_Canceled((IK)Enum.Parse(typeof(IK), actionname)); };
                    }
                }
            }
            AddMoveAxisBind();

        }
        private void AddMoveAxisBind()
        {
            //当输入上下左右时，维护移动轴

            Add_ControllInputAction_Performerd(IV.UP, () => isUp = true);
            Add_ControllInputAction_Performerd(IV.DOWN, ()=>isDown = true);
            Add_ControllInputAction_Performerd(IV.LEFT, () => isLeft = true);
            Add_ControllInputAction_Performerd(IV.RIGHT, ()=> isRight = true);

            Add_ControllInputAction_Canceled(IV.UP,()=>isUp = false);
            Add_ControllInputAction_Canceled(IV.DOWN,()=>isDown = false);
            Add_ControllInputAction_Canceled(IV.LEFT,()=>isLeft = false);
            Add_ControllInputAction_Canceled(IV.RIGHT,()=>isRight = false);
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
                //Debug.Log("RemoveCanceled" + action);
            }
        }



        /// <summary>
        /// 按下事件
        /// </summary>
        /// <param name="inputkey"></param>
        private void InputAction_Performerd(IK inputkey)
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
        private void InputAction_Canceled(IK inputkey)
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
        /// <param name="pad"></param>
        /// <param name="_low"></param>
        /// <param name="_max"></param>
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
        /// <param name="pad"></param>
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

