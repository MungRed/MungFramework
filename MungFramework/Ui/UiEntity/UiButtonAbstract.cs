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
        public enum UiButtonActionTypeEnum
        {
            Up, Down, Left, Right, 
            OK, 
            Select, UnSelect,
            SpecialAction,

            MouseEnter,
            MouseExit,
        }
        [Flags]
        public enum UiButtonMoveDirectionEnum
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
        protected UiButtonMoveDirectionEnum _moveDirection = UiButtonMoveDirectionEnum.All;

        public bool CouldUp => (_moveDirection & UiButtonMoveDirectionEnum.Up) == UiButtonMoveDirectionEnum.Up;
        public bool CouldDown => (_moveDirection & UiButtonMoveDirectionEnum.Down) == UiButtonMoveDirectionEnum.Down;
        public bool CouldLeft => (_moveDirection & UiButtonMoveDirectionEnum.Left) == UiButtonMoveDirectionEnum.Left;
        public bool CouldRight => (_moveDirection & UiButtonMoveDirectionEnum.Right) == UiButtonMoveDirectionEnum.Right;

        [SerializeField]
        protected SerializedDictionary<UiButtonActionTypeEnum, UnityEvent> uiButtonActionMap = new();


        [SerializeField]
        protected bool couldMouseSelect = true;
        [SerializeField]
        protected bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            protected set => isSelected = value;
        }
        
        [SerializeField]
        protected GameObject selectObject;

        [SerializeField]
        protected AudioClip checkAudio;

        private bool mouseIn;

        protected virtual void Update()
        {
            if (InputManagerAbstract.Instance.UseMouse&&UiLayer != null && UiLayer.isTop)
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
        public void AddAction(UiButtonActionTypeEnum type, UnityAction action)
        {
            if (!uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap.Add(type, new());
            }
            uiButtonActionMap[type].AddListener(action);
        }
        public void RemoveAction(UiButtonActionTypeEnum type, UnityAction action)
        {
            if (uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap[type].RemoveListener(action);
            }
        }
        public void DoAction(UiButtonActionTypeEnum type)
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
            DoAction(UiButtonActionTypeEnum.Select);
            if (checkAudio != null&&playAudio)
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
            DoAction(UiButtonActionTypeEnum.UnSelect);
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
            DoAction(UiButtonActionTypeEnum.OK);
            if (UiLayer != null)
            {
                UiLayer.OnButtonOK(this);
            }
        }
        public virtual void OnUp()
        {
            DoAction(UiButtonActionTypeEnum.Up);
        }
        public virtual void OnDown()
        {
            DoAction(UiButtonActionTypeEnum.Down);
        }
        public virtual void OnLeft()
        {
            DoAction(UiButtonActionTypeEnum.Left);
        }
        public virtual void OnRight()
        {
            DoAction(UiButtonActionTypeEnum.Right);
        }
        public virtual void OnSpecialAction()
        {
            DoAction(UiButtonActionTypeEnum.SpecialAction);
            if (UiLayer != null)
            {
                UiLayer.OnButtonSpecialAction(this);
            }
        }
        public virtual void MOnMouseEnter()
        {
            DoAction(UiButtonActionTypeEnum.MouseEnter);
            if (UiLayer != null)
            {
                UiLayer.JumpButton(this);
            }
        }
        public virtual void MOnMouseExit()
        {
            DoAction(UiButtonActionTypeEnum.MouseExit);
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