using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CNT = UnityEngine.InputSystem.InputAction.CallbackContext;

/*
 * Key��Value�Ĺ�ϵ��
 * Key��InputSystem��̬�󶨣���InputSystem��鵽ĳ������������ʱ���ᴥ����Ӧ��Key
 * Key��Value��̬�󶨣�����Ϸ�����м�⵽Key������ʱ�����Key�󶨵�Value��Ȼ�󴥷�Value
 * Key��Value�İ󶨷ֲ㣬����Ui�㣬ͨ�ò㣬ս���㣬ÿ���㶼���Լ���Key��Value�İ�
 * ÿ�����е�Key���ܳ�ͻ������ͬ���е�Key���Գ�ͻ��������Ui���У�ȷ�ϰ�J��ȡ���Ͳ��ܰ�J����ս�����еĹ������԰�J
 * ÿһ���Key��Value�Ĺ�ϵ��Ҫͨ���浵ϵͳ����
 * 
 * ����ϵͳ���߼���
 * 1.����������������������ջ��ÿ�δ���Key�󣬽���Ӧ��Value����ջ���Ľ�����������������Value������Ӧ�Ĳ���
 * 2.��������ջ֮�⣬���ܶ�������¼���������ĳ��Value������ʱ��������Ӧ���¼�
 */

namespace MungFramework.Logic.Input
{
    /// <summary>
    /// ����
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
    /// ����ֵ
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
    /// �����豸
    /// </summary>
    public enum InputDeviceEnum
    {
        ����, �ֱ�
    }

    /// <summary>
    /// ���������
    /// </summary>
    public abstract class InputManagerAbstract : SingletonGameManagerAbstract<InputManagerAbstract>
    {
        [SerializeField]
        [Required("��Ҫ��ק����")]
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
        /// ���������������ջ
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
        /// ���������������ջ
        /// </summary>
        public void Pop_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Remove(acceptor);
        }

        /// <summary>
        /// �ж�ĳ������������Ƿ���ջ��
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
            //��ʼ������
            InitInput();
        }

        public override void OnGameUpdate(GameManagerAbstract parentManage)
        {
            base.OnGameUpdate(parentManage);
            //�л�����������
            if (Keyboard.current != null && Keyboard.current.anyKey.IsPressed())
            {
                if (InputDevice == InputDeviceEnum.�ֱ�)
                {
                    InputDevice = InputDeviceEnum.����;
                    OnChangeDevice();
                }
            }
            else if (Gamepad.current != null && Gamepad.current.IsActuated())
            {
                if (InputDevice == InputDeviceEnum.����)
                {
                    InputDevice = InputDeviceEnum.�ֱ�;
                    OnChangeDevice();
                }
            }
        }
        #endregion

        /// <summary>
        /// �������豸�ı�ʱ
        /// </summary>
        protected virtual void OnChangeDevice()
        {

        }

        /// <summary>
        /// ��ʼ�������¼�������Key��InputSystem��
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
        /// �ı䰴����
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

            //����ɼ�λ���������˵������Ӽ�λ�����Ǹı��λ
            if (oldkey == InputKeyEnum.NONE)
            {
                UnityAction<InputKeyEnum> addBind = (InputKeyEnum newkey) =>
                {
                    isKeyDown = true;
                    if (inputDevice == InputDeviceEnum.�ֱ� && InputDataManagerAbstract.isGamepad(newkey))
                    {
                        changeSuccess = inputMap.AddBind(newkey, value);
                    }
                    else if (inputDevice == InputDeviceEnum.���� && InputDataManagerAbstract.isKeyboard(newkey))
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
                    Debug.LogError("�������л����󣬲����ھɵİ�");
                    changeBindCallback?.Invoke(false);
                    yield break;
                }
                UnityAction<InputKeyEnum> changeBind = (InputKeyEnum newkey) =>
                {
                    isKeyDown = true;
                    if (inputDevice == InputDeviceEnum.�ֱ� && InputDataManagerAbstract.isGamepad(newkey))
                    {
                        changeSuccess = inputMap.ChangeBind(oldkey, newkey, value);
                    }
                    else if (inputDevice == InputDeviceEnum.���� && InputDataManagerAbstract.isKeyboard(newkey))
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

        #region ��
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        [SerializeField]
        [ReadOnly]
        protected bool IsUp, IsDown, IsLeft, IsRight;

        /// <summary>
        /// �����ƶ���
        /// </summary>
        public Vector2 MoveAxis
        {
            get
            {
                Vector2 res = Vector2.zero;
                if (InputDevice == InputDeviceEnum.�ֱ�)
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
        /// �����ӽ���
        /// </summary>
        public Vector2 ViewAxis => inputDataManager.ViewAxis;

        /// <summary>
        /// ���λ��
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

        #region ���/�Ƴ� �¼�����
        /// <summary>
        /// Ϊĳ��������Ӱ����¼�
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
        /// �Ƴ�ĳ������İ����¼�
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
        /// Ϊĳ���������ȡ���¼�
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
        /// �Ƴ�ĳ�������ȡ���¼�
        /// </summary>
        public virtual void Remove_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (inputActionListener_Canceled.ContainsKey(type))
            {
                inputActionListener_Canceled[type].RemoveListener(action);
            }
        }

        /// <summary>
        /// �����������µ��¼�
        /// </summary>
        public virtual void Add_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            inputActionListener_AnyKeyDown.AddListener(action);
        }

        /// <summary>
        /// �Ƴ���������µ��¼�
        /// </summary>
        public virtual void Remove_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            inputActionListener_AnyKeyDown.RemoveListener(action);
        }
        #endregion

        #region ���º�ȡ���¼�
        /// <summary>
        /// �����¼�
        /// </summary>
        protected virtual void InputAction_Performed(InputKeyEnum inputkey)
        {
            if (inputkey.ToString().Contains("MOUSE") && !UseMouse)
            {
                return;
            }
            //Debug.Log(inputkey);
            //����һ����������µ��¼�
            InputAction_AnyKeyDown(inputkey);


            IInputAcceptor inputAcceptor = null;


            if (inputAcceptorStack.Count > 0)
            {
                //���ջ���Ľ�������Ϊ�գ���ʹ��ջ���Ľ�����
                if (inputAcceptorStack[0] != null)
                {
                    inputAcceptor = inputAcceptorStack[0];
                }
                //�����Ƴ�ջ���Ľ�������Ȼ�������������
                else
                {
                    inputAcceptorStack.RemoveAt(0);
                    return;
                }
            }

            //��һ��HashSet���洢�Ѿ�������Value����ֹ�ظ�����
            HashSet<InputValueEnum> valueHashSet = new();
            //���ݰ�����key��ȡ����ֵ
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
        /// ȡ���¼�
        /// </summary>
        protected virtual void InputAction_Canceled(InputKeyEnum inputkey)
        {
            if (inputkey.ToString().Contains("MOUSE") && !UseMouse)
            {
                return;
            }
            //���ݰ�����key��ȡ����ֵ
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                if (inputActionListener_Canceled.ContainsKey(inputvalue))
                {
                    inputActionListener_Canceled[inputvalue].Invoke();
                }
            }
        }

        /// <summary>
        /// ���������
        /// </summary>
        protected virtual void InputAction_AnyKeyDown(InputKeyEnum inputKey)
        {
            inputActionListener_AnyKeyDown.Invoke(inputKey);
        }
        #endregion

        #region �ֱ���
        /// <summary>
        /// Ĭ����
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

            if (InputDevice != InputDeviceEnum.�ֱ�)
            {
                return;
            }
            StartCoroutine(defaultMoter(time));
        }

        /// <summary>
        /// ��
        /// </summary>
        public virtual void Moter(Gamepad pad, float low, float max)
        {
            if (InputDevice != InputDeviceEnum.�ֱ�)
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
        /// ȡ����
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

