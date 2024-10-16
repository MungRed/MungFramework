using MungFramework.Algorithm;
using MungFramework.Extension.ComponentExtension;
using MungFramework.Extension.LifeCycleExtension;
using MungFramework.Extension.MathExtension;
using MungFramework.Logic.Input;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// UiLayer������
    /// ���򿪲��ʱ�����뵽����������У����տ��ƣ�ͨ�����������ư�ť��ѡ��
    /// ���Թ���ĳ��LayerGroup��
    /// </summary>
    public abstract class UiLayerAbstract : UiEntityAbstract, IInputAcceptor
    {
        public InputManagerAbstract InputManager => InputManagerAbstract.Instance;

        private UiLayerGroupAbstract uiLayerGroup;
        public UiLayerGroupAbstract UiLayerGroup
        {
            get
            {
                if (uiLayerGroup == null)
                {
                    uiLayerGroup = GetComponentInParent<UiLayerGroupAbstract>();
                }
                return uiLayerGroup;
            }
        }

        private UiScrollViewAbstract uiScrollView;
        public UiScrollViewAbstract UiScrollView
        {
            get
            {
                if (uiScrollView == null)
                {
                    uiScrollView = GetComponentInChildren<UiScrollViewAbstract>();
                }
                return uiScrollView;
            }
        }

        public UiEventModel LayerEvent = new();

        [SerializeField]
        protected UnityEvent openEvent = new();
        [SerializeField]
        protected UnityEvent closeEvent = new();

        [SerializeField]
        protected List<UiButtonAbstract> buttonList = new(); //��ť�б�
        [SerializeField]
        protected UiButtonAbstract nowSelectButton; //��ǰѡ�еİ�ť
        [SerializeField]
        protected RectTransform selectPoint;
        [SerializeField]
        protected bool couldKeyToClose = true;      //�ܷ�ͨ�������رյ�ǰ��

        #region Jump
        [SerializeField]
        protected bool LeftRightJump = false;
        [SerializeField]
        [ShowIf("LeftRightJump")]
        protected int LeftRightJumpCount;
        [SerializeField]
        protected bool RollJump = false;
        [SerializeField]
        [ShowIf("RollJump")]
        protected int RollJumpCount;
        #endregion

        [SerializeField]
        protected bool isOpen;

        public bool IsTop => InputManager.IsTopInputAcceptor(this);

        protected virtual void FixedUpdate()
        {
            if (isOpen && selectPoint != null)
            {
                if (nowSelectButton != null)
                {
                    selectPoint.gameObject.SetActive(true);
                    selectPoint.LerpRectTransform(nowSelectButton.RectTransform, StaticData.FixedDeltaTimeLerpValue_Bigger);
                }
                else
                {
                    selectPoint.gameObject.SetActive(false);
                }
            }
        }

        #region Input
        public virtual void OnInput(InputValueEnum inputType)
        {
            LayerControll(inputType);
        }
        protected virtual void LayerControll(InputValueEnum inputType)
        {
            switch (inputType)
            {
                case InputValueEnum.NONE:
                    break;
                case InputValueEnum.LEFT:
                    if (LeftRightJump)
                    {
                        UpJump(LeftRightJumpCount);
                        AddTapping(inputType, () => UpJump(LeftRightJumpCount));
                    }
                    else
                    {
                        Left();
                        AddTapping(inputType, Left);
                    }
                    break;
                case InputValueEnum.UP:
                    Up();
                    AddTapping(inputType, Up);
                    break;
                case InputValueEnum.DOWN:
                    Down();
                    AddTapping(inputType, Down);
                    break;
                case InputValueEnum.RIGHT:
                    if (LeftRightJump)
                    {
                        DownJump(LeftRightJumpCount);
                        AddTapping(inputType, () => DownJump(LeftRightJumpCount));
                    }
                    else
                    {
                        Right();
                        AddTapping(inputType, Right);
                    }
                    break;
                case InputValueEnum.OK:
                    OK();
                    break;
                case InputValueEnum.CANCEL:
                    Cancel();
                    break;
                case InputValueEnum.SPECIALACTION:
                    SpecialAction();
                    break;
                case InputValueEnum.LEFT_PAGE:
                    LeftPage();
                    break;
                case InputValueEnum.RIGTH_PAGE:
                    RightPage();
                    break;
                case InputValueEnum.UP_ROLL:
                    if (RollJump)
                    {
                        UpJump(RollJumpCount);
                        AddTapping(inputType, () => UpJump(RollJumpCount));
                    }
                    else
                    {
                        UpRoll();
                    }

                    break;
                case InputValueEnum.DOWN_ROLL:
                    if (RollJump)
                    {
                        DownJump(RollJumpCount);
                        AddTapping(inputType, () => DownJump(RollJumpCount));
                    }
                    else
                    {
                        DownRoll();
                    }
                    break;
            }
        }
        protected void AddTapping(InputValueEnum input, UnityAction action)
        {
            IEnumerator Tapping(UnityAction handle)
            {
                yield return new WaitForSeconds(0.5f);
                while (true)
                {
                    if (IsTop)
                    {
                        handle.Invoke();
                    }
                    else
                    {
                        yield break;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }

            //��ʼ�����û�
            var cor = StartCoroutine(Tapping(action));
            UnityAction cancel = () =>
            {
                try
                {
                    StopCoroutine(cor);
                }
                catch { }
            };
            cancel += () => InputManager.Remove_InputAction_Canceled(input, cancel);
            InputManager.Add_InputAction_Canceled(input, cancel);
        }
        #endregion
        #region OpenAndClose
        public virtual void Open()
        {
            //��ȡ���еİ�ť
            buttonList = GetComponentsInChildren<UiButtonAbstract>().ToList();
            if (buttonList.Count == 0&& selectPoint != null)
            {
                selectPoint.gameObject.SetActive(false);
            }
            //�����ǰû��ѡ�еİ�ť������ѡ�еİ�ť��Ч
            //��ѡ�е�һ����ť
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    nowSelectButton.OnSelect(false);
                }
            }
            else
            {
                nowSelectButton.OnSelect(false);
            }


            //����Ⱦ����������isOpen
            LifeCycleExtension.LateUpdateHelp(() => isOpen = true);

            gameObject.SetActive(true);
            InputManager.Push_InputAcceptor(this);
            OnLayerOpen();
        }

        public virtual void Close()
        {
            //�����ǰ��ѡ�еİ�ť��ȡ��ѡ��
            if (nowSelectButton != null)
            {
                nowSelectButton.OnUnSelect();
            }

            //��հ�ť�б�
            buttonList.Clear();

            InputManager.Pop_InputAcceptor(this);

            gameObject.SetActive(false);
            isOpen = false;
            StopAllCoroutines();
            OnLayerClose();
        }
        #endregion
        #region Direction
        protected virtual void Up()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnUp();

            if (!nowSelectButton.CouldUp)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetUpButtons(buttonList, nowSelectButton), nowSelectButton, () => GetBottomButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        protected virtual void Down()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnDown();

            if (!nowSelectButton.CouldDown)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetDownButtons(buttonList, nowSelectButton), nowSelectButton, () => GetTopButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        protected virtual void Left()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnLeft();

            if (!nowSelectButton.CouldLeft)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetLeftButtons(buttonList, nowSelectButton), nowSelectButton, () => GetRightMostButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        protected virtual void Right()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnRight();

            if (!nowSelectButton.CouldRight)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetRightButtons(buttonList, nowSelectButton), nowSelectButton, () => GetLeftMostButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        #endregion
        #region Controll
        protected virtual void OK()
        {
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                return;
            }
            nowSelectButton.OnOK();
        }

        protected virtual void Cancel()
        {
            if (couldKeyToClose)
            {
                //�����ĳ��Group�£��͹ر�Group��ͨ��Group���ر�
                //����ֱ�ӹر�
                if (UiLayerGroup != null)
                {
                    UiLayerGroup.Close();
                }
                else
                {
                    Close();
                }
            }
        }
        protected virtual void SpecialAction()
        {
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                return;
            }
            nowSelectButton.OnSpecialAction();
        }
        protected virtual void LeftPage()
        {
            if (UiLayerGroup != null)
            {
                UiLayerGroup.LeftPage();
            }
        }
        protected virtual void RightPage()
        {
            if (UiLayerGroup != null)
            {
                UiLayerGroup.RightPage();
            }
        }
        protected virtual void UpRoll()
        {
        }
        protected virtual void DownRoll()
        {
        }

        /// <summary>
        /// ����������JumpCount����ť����0��ʼ
        /// </summary>
        protected virtual void UpJump(int JumpCount)
        {
            var nextButton = GetKthNearstButtonOrDefault(
                GetUpButtonsGroupByY(buttonList, nowSelectButton),
                nowSelectButton,
                JumpCount,
                () => GetTopButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }

        /// <summary>
        /// ����������JumpCount����ť����0��ʼ
        /// </summary>
        protected virtual void DownJump(int JumpCount)
        {
            var nextButton = GetKthNearstButtonOrDefault(
                GetDownButtonsGroupByY(buttonList, nowSelectButton),
                nowSelectButton,
                JumpCount,
                () => GetBottomButton(buttonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }

        /// <summary>
        /// ��ת��ĳ��Button
        /// </summary>
        public void JumpButton(UiButtonAbstract nextButton)
        {
            if (nowSelectButton != null)
            {
                if (nextButton != nowSelectButton)
                {
                    nowSelectButton.OnUnSelect();
                }
            }
            if (nextButton != null)
            {
                if (nowSelectButton != nextButton)
                {
                    nextButton.OnSelect();
                }
            }
            nowSelectButton = nextButton;
        }
        #endregion
        #region AddButton
        public void AddButton(UiButtonAbstract button)
        {
            if (!buttonList.Contains(button))
            {
                buttonList.Add(button);
                button.UiLayer = this;
            }
        }
        public void RemoveButton(UiButtonAbstract button)
        {
            //�����ǰ��ť��ѡ��
            if (nowSelectButton == button)
            {
                //����������ѡ��һ������İ�ťѡ��
                var nextButton = GetNextButtonOnButtonRemove(buttonList, button);
                JumpButton(nextButton);
            }
            buttonList.Remove(button);
            button.UiLayer = null;
        }
        #endregion
        #region Event
        public void AddOpenEventListener(UnityAction action) => openEvent.AddListener(action);
        public void AddCloseEventListener(UnityAction action) => closeEvent.AddListener(action);
        public void RemoveOpenEventListener(UnityAction action) => openEvent.RemoveListener(action);
        public void RemoveCloseEventListener(UnityAction action) => closeEvent.RemoveListener(action);

        public virtual void OnButtonSelect(UiButtonAbstract uiButton)
        {
            LayerEvent.Call_OnButtonSelect(uiButton);
            UiLayerGroup?.OnButtonSelect(uiButton);
        }
        public virtual void OnButtonUnSelect(UiButtonAbstract uiButton)
        {
            LayerEvent.Call_OnButtonUnSelect(uiButton);
            UiLayerGroup?.OnButtonUnSelect(uiButton);
        }
        public virtual void OnButtonOK(UiButtonAbstract uiButton)
        {
            LayerEvent.Call_OnButtonOK(uiButton);
            UiLayerGroup?.OnButtonOK(uiButton);
        }
        public virtual void OnButtonSpecialAction(UiButtonAbstract uiButton)
        {
            LayerEvent.Call_OnButtonSpecialAction(uiButton);
            UiLayerGroup?.OnButtonSpecialAction(uiButton);
        }
        protected virtual void OnLayerOpen()
        {
            openEvent.Invoke();
            LayerEvent.Call_OnLayerOpen(this);
            UiLayerGroup?.OnLayerOpen(this);
        }
        protected virtual void OnLayerClose()
        {
            closeEvent.Invoke();
            LayerEvent.Call_OnLayerClose(this);
            UiLayerGroup?.OnLayerClose(this);
        }
        #endregion

        #region ѡ��
        /// <summary>
        /// ѡ�е�һ����ť
        /// </summary>
        protected bool SelectFirstButton()
        {
            //�����ǰû��ѡ�а�ť���ߵ�ǰѡ�еİ�ť��Ч,�ͳ���ѡ�е�һ����ť
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    nowSelectButton.OnSelect();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// ��ȡ��һ����ť�����Ͻǣ�
        /// </summary>
        protected bool FindFirstButton()
        {
            if (buttonList.Count == 0)
            {
                nowSelectButton = null;
                return false;
            }

            UiButtonAbstract nowButton = buttonList[0];
            foreach (UiButtonAbstract button in buttonList)
            {
                button.OnUnSelect();
                if (Mathf.Abs(button.CanvasPosition.x - nowButton.CanvasPosition.x) < 1)
                {
                    if (button.CanvasPosition.y > nowButton.CanvasPosition.y)
                    {
                        nowButton = button;
                    }
                }
                else if (button.CanvasPosition.x < nowButton.CanvasPosition.x)
                {
                    nowButton = button;
                }
            }
            nowSelectButton = nowButton;
            return true;
        }

        /// <summary>
        /// ��ĳ����ť�Ƴ�ʱ����ȡ��һ����ť
        /// </summary>
        protected UiButtonAbstract GetNextButtonOnButtonRemove(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (aim.CouldLeft)
            {
                //��ȡ��ߵ�����İ�ť
                var button = GetNearstButton(GetLeftButtons(buttons, aim), aim);
                if (button != null)
                {
                    return button;
                }
            }
            if (aim.CouldRight)
            {
                //��ȡ�ұߵ�����İ�ť
                var button = GetNearstButton(GetRightButtons(buttons, aim), aim);
                if (button != null)
                {
                    return button;
                }
            }
            if (aim.CouldUp)
            {
                //��ȡ�ϱߵ�����İ�ť
                var button = GetNearstButton(GetUpButtons(buttons, aim), aim);
                if (button != null)
                {
                    return button;
                }
            }
            if (aim.CouldDown)
            {
                //��ȡ�±ߵ�����İ�ť
                var button = GetNearstButton(GetDownButtons(buttons, aim), aim);
                if (button != null)
                {
                    return button;
                }
            }
            return null;
        }
        #endregion
        #region ��ȡ�������ҷ���İ�ť
        protected static IEnumerable<UiButtonAbstract> GetUpButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.CanvasBottom > aim.CanvasTop);
        }
        /// <summary>
        /// ��ȡ�Ϸ��İ�ť����ͬ��Y����ֻȡ������С��
        /// </summary>
        protected static IEnumerable<UiButtonAbstract> GetUpButtonsGroupByY(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            Dictionary<int, UiButtonAbstract> dic = new();
            foreach (var button in GetUpButtons(buttons, aim))
            {
                if (dic.ContainsKey(button.CanvasPosition.y.Round()))
                {
                    if (isNearThanOld(button, dic[button.CanvasPosition.y.Round()], aim))
                    {
                        dic[button.CanvasPosition.y.Round()] = button;
                    }
                }
                else
                {
                    dic[button.CanvasPosition.y.Round()] = button;
                }
            }
            return dic.Select(pair => pair.Value);
        }
        protected static IEnumerable<UiButtonAbstract> GetDownButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.CanvasTop < aim.CanvasBottom);
        }
        protected static IEnumerable<UiButtonAbstract> GetDownButtonsGroupByY(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            Dictionary<int, UiButtonAbstract> dic = new();
            foreach (var button in GetDownButtons(buttons, aim))
            {
                if (dic.ContainsKey(button.CanvasPosition.y.Round()))
                {
                    if (isNearThanOld(button, dic[button.CanvasPosition.y.Round()], aim))
                    {
                        dic[button.CanvasPosition.y.Round()] = button;
                    }
                }
                else
                {
                    dic[button.CanvasPosition.y.Round()] = button;
                }
            }
            return dic.Select(pair => pair.Value);
        }
        protected static IEnumerable<UiButtonAbstract> GetLeftButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.CanvasRight < aim.CanvasLeft);
        }
        protected static IEnumerable<UiButtonAbstract> GetRightButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.CanvasLeft > aim.CanvasRight);
        }
        #endregion
        #region ��ȡ���������ҵİ�ť
        protected static UiButtonAbstract GetTopButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }

            UiButtonAbstract res = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.CanvasPosition.y - res.CanvasPosition.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.CanvasPosition.y > res.CanvasPosition.y)
                {
                    res = button;
                }
            }
            return res;
        }
        protected static UiButtonAbstract GetBottomButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.CanvasPosition.y - res.CanvasPosition.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.CanvasPosition.y < res.CanvasPosition.y)
                {
                    res = button;
                }
            }
            return res;
        }
        protected static UiButtonAbstract GetLeftMostButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.CanvasPosition.x - res.CanvasPosition.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.CanvasPosition.x < res.CanvasPosition.x)
                {
                    res = button;
                }
            }
            return res;
        }
        protected static UiButtonAbstract GetRightMostButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons.First();

            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.CanvasPosition.x - res.CanvasPosition.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.CanvasPosition.x > res.CanvasPosition.x)
                {
                    res = button;
                }
            }
            return res;
        }
        #endregion
        #region ��ȡ����İ�ť
        protected static UiButtonAbstract GetNearstButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }

            UiButtonAbstract nearstButton = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (isNearThanOld(button, nearstButton, aim))
                {
                    nearstButton = button;
                }
            }
            return nearstButton;
        }
        protected static bool isNearThanOld(UiButtonAbstract newButton, UiButtonAbstract oldButton, UiButtonAbstract aim)
        {
            return (newButton.CanvasPosition - aim.CanvasPosition).sqrMagnitude < (oldButton.CanvasPosition - aim.CanvasPosition).sqrMagnitude;
        }
        protected static UiButtonAbstract GetNearstButtonOrDefault(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim, Func<UiButtonAbstract> defaultButton)
        {
            if (buttons.Count() == 0)
            {
                return defaultButton();
            }

            UiButtonAbstract nearstButton = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (isNearThanOld(button, nearstButton, aim))
                {
                    nearstButton = button;
                }
            }
            return nearstButton;
        }
        /// <summary>
        /// ��õ�k���İ�ť�����û�оͷ���Ĭ�ϰ�ť
        /// </summary>
        protected static UiButtonAbstract GetKthNearstButtonOrDefault(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim, int k, Func<UiButtonAbstract> defaultButton)
        {
            if (buttons.Count() < k + 1)
            {
                return defaultButton();
            }
            var buttonList = buttons.ToList();
            return GetKthItemStatic.GetKthItem(buttonList, k, (a, b) => isNearThanOld(a, b, aim));

        }
        #endregion
    }
}

