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
    }

    /// <summary>
    /// ����ֵ
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
#if UNITY_EDITOR
        private bool isBind
        {
            get
            {            
                bool res = !(inputDataManager!=null&&subGameManagerList.Contains(inputDataManager));
                if (res == true)
                {
                    Debug.LogError(name + "��Ҫ�����ӹ�����");
                }
                return res;
            }
        }
#endif


        [SerializeField]
        [Required("��Ҫ��ק����")]
        [InfoBox("��Ҫ�ϵ��ӹ�������","isBind", InfoMessageType = InfoMessageType.Error)]
        private InputDataManagerAbstract inputDataManager;

        public InputDeviceEnum InputDevice;


        [ShowInInspector]
        [ReadOnly]
        private List<IInputAcceptor> inputAcceptorStack = new();

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Performerd = new();

        private Dictionary<InputValueEnum, UnityEvent> InputActionListener_Canceled = new();

        private UnityEvent<InputKeyEnum> InputActionListener_AnyKeyDown = new();


        /// <summary>
        /// ���������������ջ
        /// </summary>
        /// <param name="acceptor"></param>
        public void Push_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Insert(0, acceptor);
        }

        /// <summary>
        /// ���������������ջ
        /// </summary>
        /// <param name="acceptor"></param>
        public void Pop_InputAcceptor(IInputAcceptor acceptor)
        {
            inputAcceptorStack.Remove(acceptor);
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
                InputDevice = InputDeviceEnum.����;
            }
            else if (Gamepad.current != null && Gamepad.current.IsActuated())
            {
                InputDevice = InputDeviceEnum.�ֱ�;
            }
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
                        action.performed += (CNT cnt) => { InputAction_Performerd((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), actionname)); };
                        action.canceled += (CNT cnt) => { InputAction_Canceled((InputKeyEnum)Enum.Parse(typeof(InputKeyEnum), actionname)); };
                    }
                }
            }

            AddMoveAxisBind();
        }

        /// <summary>
        /// �ı䰴����
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

                return res.normalized;
            }
        }

        /// <summary>
        /// �����ӽ���
        /// </summary>
        public Vector2 ViewAxis => inputDataManager.ViewAxis;

        /// <summary>
        /// ����ƶ���İ�
        /// </summary>
        protected virtual void AddMoveAxisBind()
        {
            Add_InputAction_Performerd(InputValueEnum.UP, () => IsUp = true);
            Add_InputAction_Performerd(InputValueEnum.DOWN, () => IsDown = true);
            Add_InputAction_Performerd(InputValueEnum.LEFT, () => IsLeft = true);
            Add_InputAction_Performerd(InputValueEnum.RIGHT, () => IsRight = true);

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
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Add_InputAction_Performerd(InputValueEnum type, UnityAction action)
        {
            if (!InputActionListener_Performerd.ContainsKey(type))
            {
                InputActionListener_Performerd.Add(type, new());
            }
            InputActionListener_Performerd[type].AddListener(action);
        }
        /// <summary>
        /// �Ƴ�ĳ������İ����¼�
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Remove_InputAction_Performerd(InputValueEnum type, UnityAction action)
        {
            if (InputActionListener_Performerd.ContainsKey(type))
            {
                InputActionListener_Performerd[type].RemoveListener(action);
                Debug.Log("RemovePerformed" + action);
            }
        }
        /// <summary>
        /// Ϊĳ���������ȡ���¼�
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
        /// <param name="type"></param>
        /// <param name="action"></param>
        public virtual void Add_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.AddListener(action);
        }
        /// <summary>
        /// �Ƴ���������µ��¼�
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public  virtual void Remove_InputAction_AnyKeyDown(UnityAction<InputKeyEnum> action)
        {
            InputActionListener_AnyKeyDown.RemoveListener(action);
        }

        #endregion

        #region ���º�ȡ���¼�
        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="inputkey"></param>
        protected virtual void InputAction_Performerd(InputKeyEnum inputkey)
        {
            //���������������ʹ���һ����������µ��¼�
            if (inputkey != InputKeyEnum.ANYKEY)
            {
                InputAction_AnyKeyDown(inputkey);
            }

            //���ݰ�����key��ȡ����ֵ
            foreach (var inputvalue in inputDataManager.GetInputValues(inputkey))
            {
                //Debug.Log(iv);
                if (InputActionListener_Performerd.ContainsKey(inputvalue))
                {
                    InputActionListener_Performerd[inputvalue].Invoke();
                }

                if (inputAcceptorStack.Count > 0)
                {
                    inputAcceptorStack[0].OnInput(inputvalue);
                }
            }
        }
        /// <summary>
        /// ȡ���¼�
        /// </summary>
        /// <param name="inputkey"></param>
        protected virtual void InputAction_Canceled(InputKeyEnum inputkey)
        {
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
        /// <param name="inputKey"></param>
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
        /// ��
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

