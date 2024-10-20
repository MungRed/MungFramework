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
    /// ����
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


        MOUSE_LEFT,MOUSE_RIGHT,
    }

    /// <summary>
    /// ����ֵ
    /// </summary>
    public enum InputValueEnum
    {
        NONE  = 0,
        ANYKEY = 1,

        LEFT = 10, UP=11, DOWN=12, RIGHT=13,

        //UI
        OK=20, CANCEL=21, SPECIALACTION = 22,
        LEFT_PAGE=23, RIGTH_PAGE=24,
        UP_ROLL=25, DOWN_ROLL=26,

        //Option
        MENU=40,
        MAP=41,
        ACTION=42,

        //Battle
        ATTACK=60,
        SKILL=61,
        MAGICK=62,
        DEFENSE=63,
        OVERDRIVE=64, 
        EXTRA=65,
        DEPLOY=66,
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

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Performed = new();

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Canceled = new();

        private UnityEvent<InputKeyEnum> InputActionListener_AnyKeyDown = new();


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
        public  bool  IsTopInputAcceptor(IInputAcceptor acceptor)
        {
            if (inputAcceptorStack.Count > 0 && inputAcceptorStack[0] == acceptor)
            {
                return true;
            }
            return false;
        }


        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);    
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

        public virtual void OnChangeDevice()
        {

        }


        /// <summary>
        /// ��ʼ�������¼�
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
        /// �ı䰴����
        /// </summary>
        public virtual IEnumerator ChangeKeyBind(string inputMapName,InputKeyEnum oldkey, InputValueEnum value)
        {
            var inputMap = inputDataManager.GetInputMap(inputMapName);
            if (inputMap == null)
            {
                yield break;
            }

            //����ɼ�λ���������˵������Ӽ�λ�����Ǹı��λ
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
                    Debug.LogError("�������л����󣬲����ھɵİ�");
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
            OnChangeKeyBind();
        }
        public virtual void OnChangeKeyBind()
        {
        }

        public virtual IEnumerable<InputKeyEnum> GetCurrentBind(InputValueEnum value)
        {
            return inputDataManager.GetInputKeys(value);
        }

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
                inputDataManager.Save();
            }
        }

        /// <summary>
        /// ����ƶ���İ�
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

        #region ���/�Ƴ� �¼�����

        /// <summary>
        /// Ϊĳ��������Ӱ����¼�
        /// </summary>
        public virtual void Add_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (!InputActionListener_Performed.ContainsKey(type))
            {
                InputActionListener_Performed.Add(type, new());
            }
            InputActionListener_Performed[type].AddListener(action);
        }
        /// <summary>
        /// �Ƴ�ĳ������İ����¼�
        /// </summary>
        public virtual void Remove_InputAction_Performed(InputValueEnum type, UnityAction action)
        {
            if (InputActionListener_Performed.ContainsKey(type))
            {
                InputActionListener_Performed[type].RemoveListener(action);
                Debug.Log("RemovePerformed" + action);
            }
        }
        /// <summary>
        /// Ϊĳ���������ȡ���¼�
        /// </summary>
        public  virtual void Add_InputAction_Canceled(InputValueEnum type, UnityAction action)
        {
            if (!InputActionListener_Canceled.ContainsKey(type))
            {
                InputActionListener_Canceled.Add(type, new());
            }
            InputActionListener_Canceled[type].AddListener(action);
        }
        /// <summary>
        /// �Ƴ�ĳ�������ȡ���¼�
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
        /// �����������µ��¼�
        /// </summary>
        public virtual void Add_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.AddListener(action);
        }
        /// <summary>
        /// �Ƴ���������µ��¼�
        /// </summary>
        public  virtual void Remove_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.RemoveListener(action);
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
            // Debug.Log(inputkey);
            //���������������ʹ���һ����������µ��¼�
            if (inputkey != InputKeyEnum.ANYKEY)
            {
                InputAction_AnyKeyDown(inputkey);
            }

            //���ݰ�����key��ȡ����ֵ
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                if (InputActionListener_Performed.ContainsKey(inputvalue))
                {
                    InputActionListener_Performed[inputvalue].Invoke();
                }

                if (inputAcceptorStack.Count > 0)
                {
                    if (inputAcceptorStack[0] != null)
                    {
                        inputAcceptorStack[0].OnInput(inputvalue);
                    }
                    else
                    {
                        inputAcceptorStack.RemoveAt(0);
                    }
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
                if (InputActionListener_Canceled.ContainsKey(inputvalue))
                {
                    InputActionListener_Canceled[inputvalue].Invoke();
                }
            }
        }
        /// <summary>
        /// ��������£�������InputkeyEnum�е������
        /// </summary>
        protected virtual void InputAction_AnyKeyDown(InputKeyEnum inputKey)
        {
            //Debug.Log("anykeydown " + inputKey);
            InputActionListener_AnyKeyDown.Invoke(inputKey);
        }
        #endregion

        #region �ֱ���
        /// <summary>
        /// Ĭ����
        /// </summary>
        public virtual void DefaultMoter(float time)
        {
            if (InputDevice != InputDeviceEnum.�ֱ�)
            {
                return;
            }
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
        /// ��
        /// </summary>
        public virtual void Moter(Gamepad pad, float _low, float _max)
        {
            if (InputDevice != InputDeviceEnum.�ֱ�)
            {
                return;
            }

            if (pad == null)
            {
                return;
            }
            pad.SetMotorSpeeds(_low, _max);
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

