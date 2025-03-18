using MungFramework.Algorithm;
using MungFramework.Extension.LifeCycleExtension;
using MungFramework.Extension.RectTransformExtension;
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
                    uiLayerGroup = GetComponentInParent<UiLayerGroupAbstract>(true);
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
                    uiScrollView = GetComponentInChildren<UiScrollViewAbstract>(true);
                }
                return uiScrollView;
            }
        }

        [SerializeField]
        protected List<UiButtonAbstract> buttonList = new(); //��ť�б�
        protected IEnumerable<UiButtonAbstract> enabledButtonList => buttonList.Where(x => x.gameObject.activeSelf);

        [SerializeField]
        protected UiButtonAbstract nowSelectButton; //��ǰѡ�еİ�ť
        [SerializeField]
        protected RectTransform selectPoint;
        [SerializeField]
        protected bool couldKeyToClose = true;      //�ܷ�ͨ�������رյ�ǰ��

        #region Jump
        [SerializeField]
        private bool LeftRightJump = false;
        [SerializeField]
        [ShowIf("LeftRightJump")]
        private int LeftRightJumpCount;
        [SerializeField]
        private bool RollJump = false;
        [SerializeField]
        [ShowIf("RollJump")]
        private int RollJumpCount;
        #endregion

        public bool IsOpen;
        public bool IsTop => InputManager.IsTop(this);

        protected virtual void Update()
        {
            if (IsOpen && IsTop && InputManagerAbstract.Instance.UseMouse)
            {
                foreach (var button in buttonList)
                {
                    if (button.CouldMouseSelect)
                    {
                        button.UpdateMouse();
                    }
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (IsOpen && selectPoint != null)
            {
                if (nowSelectButton != null && nowSelectButton.gameObject.activeSelf)
                {
                    selectPoint.gameObject.SetActive(true);
                    // Debug.LogError("1    "+selectPoint.position);
                    selectPoint.LerpRectTransform(nowSelectButton.RectTransform, StaticData.FixedDeltaTimeLerpValue_20f);
                    //  Debug.LogError("2    "+selectPoint.position);
                }
                else
                {
                    selectPoint.gameObject.SetActive(false);
                }
            }
        }

        #region Input
        public virtual void OnInput(InputValueEnum inputType, InputKeyEnum inputkey)
        {
            if (IsOpen)
            {
                if (inputkey == InputKeyEnum.MOUSE_LEFT || inputkey == InputKeyEnum.MOUSE_RIGHT)
                {
                    LayerControll(inputType, true);
                }
                else
                {
                    LayerControll(inputType, false);
                }
            }
        }
        protected virtual void LayerControll(InputValueEnum inputType, bool useMouse)
        {
            switch (inputType)
            {
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
                    OK(useMouse);
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
            if (gameObject.activeSelf)
            {
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
        }
        #endregion
        #region OpenAndClose
        public virtual void Open()
        {
            //��ȡ���еİ�ť
            buttonList = GetComponentsInChildren<UiButtonAbstract>(true).ToList();
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

            if (selectPoint != null && (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false))
            {
                selectPoint.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
            InputManager.Push_InputAcceptor(this);
            CallActionHelp(ON_LAYER_OPEN);

            //����Ⱦ����������isOpen
            void openHelp()
            {
                IsOpen = true;
                if (selectPoint != null && nowSelectButton != null && nowSelectButton.gameObject.activeSelf)
                {
                    selectPoint.LerpRectTransform(nowSelectButton.RectTransform, 1);
                }
            }
            LifeCycleExtension.LateInvoke(openHelp);
        }
        public virtual void Close()
        {
            //�����ǰ��ѡ�еİ�ť��ȡ��ѡ��
            nowSelectButton?.OnUnSelect();

            //��հ�ť�б�
            buttonList.Clear();

            InputManager.Pop_InputAcceptor(this);

            gameObject.SetActive(false);
            IsOpen = false;
            StopAllCoroutines();
            CallActionHelp(ON_LAYER_CLOSE);
        }
        #endregion

        #region Direction
        public virtual void Up()
        {
            CallActionHelp(ON_LAYER_UP);

            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnUp();

            if (!nowSelectButton.CouldUp)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetUpButtons(enabledButtonList, nowSelectButton), nowSelectButton, () => GetBottomButton(enabledButtonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void Down()
        {
            CallActionHelp(ON_LAYER_DOWN);
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnDown();

            if (!nowSelectButton.CouldDown)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetDownButtons(enabledButtonList, nowSelectButton), nowSelectButton, () => GetTopButton(enabledButtonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void Left()
        {
            CallActionHelp(ON_LAYER_LEFT);
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnLeft();

            if (!nowSelectButton.CouldLeft)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetLeftButtons(enabledButtonList, nowSelectButton), nowSelectButton, () => GetRightMostButton(enabledButtonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void Right()
        {
            CallActionHelp(ON_LAYER_RIGHT);
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.OnRight();

            if (!nowSelectButton.CouldRight)
            {
                return;
            }

            UiButtonAbstract nextButton = GetNearstButtonOrDefault(GetRightButtons(enabledButtonList, nowSelectButton), nowSelectButton, () => GetLeftMostButton(enabledButtonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        #endregion
        #region Controll
        protected virtual void OK(bool useMouse)
        {
            CallActionHelp(ON_LAYER_OK);
            if (nowSelectButton != null && !nowSelectButton.gameObject.activeSelf == false)
            {
                if (useMouse)
                {
                    if (nowSelectButton.MouseIn)
                    {
                        nowSelectButton.OnOK();
                    }
                }
                else
                {
                    nowSelectButton.OnOK();
                }
            }
        }

        protected virtual void Cancel()
        {
            CallActionHelp(ON_LAYER_CANCEL);
            if (couldKeyToClose)
            {
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
            CallActionHelp(ON_LAYER_SPECIAL_ACTION);
            if (nowSelectButton != null && nowSelectButton.gameObject.activeSelf != false)
            {
                nowSelectButton.OnSpecialAction();
            }
        }
        public virtual void LeftPage()
        {
            CallActionHelp(ON_LAYER_LEFT_PAGE);
            UiLayerGroup?.LeftPage();
        }
        public virtual void RightPage()
        {
            CallActionHelp(ON_LAYER_RIGHT_PAGE);
            UiLayerGroup?.RightPage();
        }
        protected virtual void UpRoll()
        {
            CallActionHelp(ON_LAYER_UP_ROLL);
        }
        protected virtual void DownRoll()
        {
            CallActionHelp(ON_LAYER_DOWN_ROLL);
        }

        /// <summary>
        /// ����������JumpCount����ť����0��ʼ
        /// </summary>
        protected virtual void UpJump(int JumpCount)
        {
            var nextButton = GetKthNearstButtonOrDefault(
                GetUpButtonsGroupByY(enabledButtonList, nowSelectButton),
                nowSelectButton,
                JumpCount,
                () => GetTopButton(enabledButtonList, nowSelectButton));

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
                GetDownButtonsGroupByY(enabledButtonList, nowSelectButton),
                nowSelectButton,
                JumpCount,
                () => GetBottomButton(enabledButtonList, nowSelectButton));

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        /// <summary>
        /// ��ת��ĳ��Button
        /// </summary>
        public virtual void JumpButton(UiButtonAbstract nextButton)
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
        public void AddButton(UiButtonAbstract button, UiButtonAbstract preButton = null)
        {
            if (!buttonList.Contains(button))
            {
                buttonList.Add(button);
                button.UiLayer = this;

                //���ǰһ����ť��Ϊ��
                if (preButton != null)
                {
                    //�ڰ�ť�ڵ�������ǰһ����ť�ĺ���
                    button.transform.SetParent(preButton.transform.parent);
                    var parentIndex = preButton.transform.GetSiblingIndex();
                    button.transform.SetSiblingIndex(parentIndex + 1);
                }
            }
        }
        public void RemoveButton(UiButtonAbstract button)
        {
            //�����ǰ��ť��ѡ��
            if (nowSelectButton == button)
            {
                //����������ѡ��һ������İ�ťѡ��
                var nextButton = GetNextButtonOnButtonRemove(enabledButtonList, button);
                JumpButton(nextButton);
            }
            buttonList.Remove(button);
            button.UiLayer = null;
        }
        #endregion
        #region Event
        public void CallActionHelp(string actionType)
        {
            CallAction(actionType);
            CallAction_UiLayer(actionType, this);
            UiLayerGroup?.CallAction(actionType);
            UiLayerGroup?.CallAction_UiLayer(actionType, this);
        }
        #endregion
        #region Select
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
            var enabledButtonList = this.enabledButtonList.ToList();
            if (enabledButtonList.Count() == 0)
            {
                nowSelectButton = null;
                return false;
            }

            UiButtonAbstract nowButton = enabledButtonList[0];
            foreach (UiButtonAbstract button in enabledButtonList)
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
        /// <summary>
        /// ��ĳ����ť�Ƴ�ʱ����ȡ��һ����ť
        /// </summary>
        protected static UiButtonAbstract GetNextButtonOnButtonRemove(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
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
            return GetKthItem.GetKth(buttonList, k, (a, b) => isNearThanOld(a, b, aim));

        }
        #endregion
    }
}

