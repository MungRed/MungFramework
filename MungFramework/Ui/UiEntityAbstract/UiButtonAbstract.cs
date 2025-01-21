using MungFramework.Extension.LifeCycleExtension;
using MungFramework.Logic.Input;
using MungFramework.Logic.Sound;
using System;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// 按钮抽象类
    /// 每个按钮需要在Layer下
    /// 同时可以在ScrollView下
    /// </summary>
    public abstract class UiButtonAbstract : UiEntityAbstract
    {
        [Flags]
        private enum UiButtonMoveDirectionEnum
        {
            None = 0,
            All = Up | Down | Left | Right,

            Up = 1 << 0,
            Down = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
        }

        private UiScrollViewAbstract _uiScrollView;
        public UiScrollViewAbstract UiScrollView
        {
            get
            {
                if (_uiScrollView == null)
                {
                    _uiScrollView = GetComponentInParent<UiScrollViewAbstract>(true);
                }
                return _uiScrollView;
            }
        }

        private UiLayerAbstract _uiLayer;
        public UiLayerAbstract UiLayer
        {
            get
            {
                if (_uiLayer == null)
                {
                    _uiLayer = GetComponentInParent<UiLayerAbstract>(true);
                }
                return _uiLayer;
            }
            set
            {
                _uiLayer = value;
            }
        }

        #region MoveDirection
        [SerializeField]
        private UiButtonMoveDirectionEnum moveDirection = UiButtonMoveDirectionEnum.All;
        public bool CouldUp
        {
            get
            {
                return (moveDirection & UiButtonMoveDirectionEnum.Up) == UiButtonMoveDirectionEnum.Up;
            }
            set
            {
                if (value)
                {
                    moveDirection |= UiButtonMoveDirectionEnum.Up;
                }
                else
                {
                    moveDirection &= ~UiButtonMoveDirectionEnum.Up;
                }
            }
        }
        public bool CouldDown
        {
            get
            {
                return (moveDirection & UiButtonMoveDirectionEnum.Down) == UiButtonMoveDirectionEnum.Down;
            }
            set
            {
                if (value)
                {
                    moveDirection |= UiButtonMoveDirectionEnum.Down;
                }
                else
                {
                    moveDirection &= ~UiButtonMoveDirectionEnum.Down;
                }
            }
        }
        public bool CouldLeft
        {
            get
            {
                return (moveDirection & UiButtonMoveDirectionEnum.Left) == UiButtonMoveDirectionEnum.Left;
            }
            set
            {
                if (value)
                {
                    moveDirection |= UiButtonMoveDirectionEnum.Left;
                }
                else
                {
                    moveDirection &= ~UiButtonMoveDirectionEnum.Left;
                }
            }
        }
        public bool CouldRight
        {
            get
            {
                return (moveDirection & UiButtonMoveDirectionEnum.Right) == UiButtonMoveDirectionEnum.Right;
            }
            set
            {
                if (value)
                {
                    moveDirection |= UiButtonMoveDirectionEnum.Right;
                }
                else
                {
                    moveDirection &= ~UiButtonMoveDirectionEnum.Right;
                }
            }
        }
        #endregion
        #region Select
        [SerializeField]
        private bool couldMouseSelect = true;
        [SerializeField]
        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            private set => isSelected = value;
        }

        [SerializeField]
        protected GameObject selectObject;
        [SerializeField]
        protected AudioClip checkAudio;

        protected bool mouseIn;
        #endregion

        protected virtual void Update()
        {
            if (couldMouseSelect && InputManagerAbstract.Instance.UseMouse && UiLayer != null && UiLayer.IsTop)
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
        }


        #region 事件
        public void CallActionHelp(string actionType)
        {
            CallAction(actionType);
            CallAction_UiButton(actionType, this);
            UiLayer?.CallAction(actionType);
            UiLayer?.CallAction_UiButton(actionType, this);
            UiLayer?.UiLayerGroup?.CallAction(actionType);
            UiLayer?.UiLayerGroup?.CallAction_UiButton(actionType, this);
        }

        public virtual void OnSelect(bool playAudio = true)
        {
            IsSelected = true;
            if (checkAudio != null && playAudio)
            {
                SoundManagerAbstract.Instance.PlayAudio("effect", checkAudio, replace: true);
            }
            SetSelectObjectActive(true);
            UpdateScrollView();
            CallActionHelp(ON_BUTTON_SELECT);
        }

        public virtual void OnUnSelect()
        {
            IsSelected = false;
            SetSelectObjectActive(false);
            CallActionHelp(ON_BUTTON_UNSELECT);
        }

        public virtual void OnOK()
        {
            CallActionHelp(ON_BUTTON_OK);
        }
        public virtual void OnUp()
        {
            CallActionHelp(ON_BUTTON_UP);
        }
        public virtual void OnDown()
        {
            CallActionHelp(ON_BUTTON_DOWN);
        }
        public virtual void OnLeft()
        {
            CallActionHelp(ON_BUTTON_LEFT);
        }
        public virtual void OnRight()
        {
            CallActionHelp(ON_BUTTON_RIGHT);
        }
        public virtual void OnSpecialAction()
        {
            CallActionHelp(ON_BUTTON_SPECIAL_ACTION);
        }
        public virtual void MOnMouseEnter()
        {
            UiLayer?.JumpButton(this);
            CallActionHelp(ON_BUTTON_MOUSE_ENTER);
        }
        public virtual void MOnMouseExit()
        {
            CallActionHelp(ON_BUTTON_MOUSE_EXIT);
        }
        #endregion


        protected virtual void SetSelectObjectActive(bool active)
        {
            if (selectObject != null)
            {
                selectObject.SetActive(active);
            }
        }
        protected virtual void UpdateScrollView()
        {
            void action()
            {
                UiScrollView?.UpdatePosition(RectTransform);
            }
            if (UiScrollView != null)
            {
                LifeCycleExtension.LateInvoke(action);
            }
        }
    }

}