using MungFramework.Extension.LifeCycleExtension;
using MungFramework.Logic.Input;
using MungFramework.Logic.Sound;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace MungFramework.Ui
{
    /// <summary>
    /// 按钮抽象类
    /// 每个按钮需要在Layer下
    /// 同时可以在ScrollView下
    /// </summary>
    public abstract class UiButtonAbstract : UiEntityAbstract
    {
        public enum UiButtonActionType
        {
            Up, Down, Left, Right, 
            OK, 
            Select, UnSelect,
            SpecialAction,

            MouseEnter,
            MouseExit,
        }
        [Flags]
        public enum UiButtonMoveDirection
        {
            None = 0,
            All = Up | Down | Left | Right,

            Up = 1<<0, 
            Down = 1<<1, 
            Left = 1<<2, 
            Right = 1<<3,
        }

        protected UiScrollViewAbstract _uiScrollView;
        public UiScrollViewAbstract UiScrollView
        {
            get
            {
                if (_uiScrollView == null)
                {
                    _uiScrollView = GetComponentInParent<UiScrollViewAbstract>();
                }
                return _uiScrollView;
            }
        }

        protected UiLayerAbstract _uiLayer;
        public UiLayerAbstract UiLayer
        {
            get
            {
                if (_uiLayer == null)
                {
                    _uiLayer = GetComponentInParent<UiLayerAbstract>();
                }
                return _uiLayer;
            }
            set
            {
                _uiLayer = value;
            }
        }


        [SerializeField]
        protected UiButtonMoveDirection _moveDirection = UiButtonMoveDirection.All;

        public bool CouldUp => (_moveDirection & UiButtonMoveDirection.Up) == UiButtonMoveDirection.Up;
        public bool CouldDown => (_moveDirection & UiButtonMoveDirection.Down) == UiButtonMoveDirection.Down;
        public bool CouldLeft => (_moveDirection & UiButtonMoveDirection.Left) == UiButtonMoveDirection.Left;
        public bool CouldRight => (_moveDirection & UiButtonMoveDirection.Right) == UiButtonMoveDirection.Right;

        [SerializeField]
        protected SerializedDictionary<UiButtonActionType, UnityEvent> uiButtonActionMap = new();


        [SerializeField]
        protected bool couldMouseSelect = true;
        [SerializeField]
        protected bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            protected set => isSelected = value;
        }
        

        //选中特效
        [SerializeField]
        protected GameObject selectObject;

        [SerializeField]
        protected AudioClip checkAudio;

        private bool mouseIn;

        protected virtual void Update()
        {
            if (InputManagerAbstract.Instance.UseMouse&&UiLayer != null && UiLayer.IsTop)
            {
                CheckMouse();
            }
        }

        protected virtual void CheckMouse()
        {
            if (MouseIn)
            {
                if (mouseIn == false)
                {
                    mouseIn = true;
                    MOnMouseEnter();
                }
            }
            else
            {
                if (mouseIn == true)
                {
                    mouseIn = false;
                    MOnMouseExit();
                }
            }
        }

        #region 事件
        public void AddAction(UiButtonActionType type, UnityAction action)
        {
            if (!uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap.Add(type, new());
            }
            uiButtonActionMap[type].AddListener(action);
        }
        public void RemoveAction(UiButtonActionType type, UnityAction action)
        {
            if (uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap[type].RemoveListener(action);
            }
        }
        public void DoAction(UiButtonActionType type)
        {
            if (uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap[type].Invoke();
            }
        }

        public virtual void OnSelect(bool playAudio = true)
        {
            IsSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (checkAudio != null)
            {
                SoundManagerAbstract.Instance.PlayAudio("effect", checkAudio,replace:true);
            }

            if (selectObject != null)
            {
                selectObject.SetActive(true);
            }
            if (UiLayer != null)
            {
                UiLayer.OnButtonSelect(this);
            }
        }

        public virtual void OnUnSelect()
        {
            IsSelected = false;
            DoAction(UiButtonActionType.UnSelect);
            if (selectObject != null)
            {
                selectObject.SetActive(false);
            }
            if (UiLayer != null)
            {
                UiLayer.OnButtonUnSelect(this);
            }
        }

        public virtual void OnOK()
        {
            DoAction(UiButtonActionType.OK);
            if (UiLayer != null)
            {
                UiLayer.OnButtonOK(this);
            }
        }
        public virtual void OnUp()
        {
            DoAction(UiButtonActionType.Up);
        }
        public virtual void OnDown()
        {
            DoAction(UiButtonActionType.Down);
        }
        public virtual void OnLeft()
        {
            DoAction(UiButtonActionType.Left);
        }
        public virtual void OnRight()
        {
            DoAction(UiButtonActionType.Right);
        }
        public virtual void OnSpecialAction()
        {
            DoAction(UiButtonActionType.SpecialAction);
            if (UiLayer != null)
            {
                UiLayer.OnButtonSpecialAction(this);
            }
        }
        public virtual void MOnMouseEnter()
        {
            DoAction(UiButtonActionType.MouseEnter);
            if (UiLayer != null)
            {
                UiLayer.JumpButton(this);
            }
        }
        public virtual void MOnMouseExit()
        {
            DoAction(UiButtonActionType.MouseExit);
        }
        #endregion

        /// <summary>
        /// 更新滚动视图
        /// </summary>
        protected virtual void UpdateScrollView()
        {
            void action()
            {
                UiScrollView.UpdatePosition(RectTransform);
            }

            if (UiScrollView != null)
            {
                LifeCycleExtension.LateUpdateHelp(action);
            }
        }
    }

}