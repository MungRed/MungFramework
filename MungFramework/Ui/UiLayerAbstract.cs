using MungFramework.Logic.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public abstract class UiLayerAbstract : MonoBehaviour, IInputAcceptor
    {
        public InputManagerAbstract InputManager => InputManagerAbstract.Instance;

        protected UiLayerGroupAbstract _uiLayerGroup;
        protected UiLayerGroupAbstract uiLayerGroup
        {
            get
            {
                if (_uiLayerGroup == null)
                {
                    _uiLayerGroup = GetComponentInParent<UiLayerGroupAbstract>();
                }
                return _uiLayerGroup;
            }
        }



        [SerializeField]
        protected List<UiButtonAbstract> buttonList = new(); //按钮列表
        [SerializeField]
        protected UiButtonAbstract nowSelectButton; //当前选中的按钮
        [SerializeField]
        protected bool couldKeyToClose = true;      //能否通过按键关闭当前层
        [SerializeField]
        protected UnityEvent openEvent = new(), closeEvent = new();        //打开和关闭事件


        public virtual void OnInput(InputValueEnum inputType)
        {
            //Debug.Log(inputType);
            LayerControll(inputType);
        }

        public virtual void LayerControll(InputValueEnum inputType)
        {
            switch (inputType)
            {
                case InputValueEnum.NONE:
                    break;
                case InputValueEnum.LEFT:
                    Left();
                    AddTapping(inputType, Left);
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
                    Right();
                    AddTapping(inputType, Right);
                    break;
                case InputValueEnum.OK:
                    OK();
                    break;
                case InputValueEnum.CANCEL:
                    Cancel();
                    break;
                case InputValueEnum.LEFT_PAGE:
                    LeftPage();
                    break;
                case InputValueEnum.RIGTH_PAGE:
                    RightPage();
                    break;
                case InputValueEnum.UP_ROLL:
                    UpRoll();
                    break;
                case InputValueEnum.DOWN_ROLL:
                    DownRoll();
                    break;
            }
        }

        protected void AddTapping(InputValueEnum input, UnityAction action)
        {
            //开始连续敲击
            var cor = StartCoroutine(Tapping(action));
            UnityAction cancel = () =>
            {
                StopCoroutine(cor);
            };
            cancel += () => InputManager.Remove_InputAction_Canceled(input, cancel);
            InputManager.Add_InputAction_Canceled(input, cancel);
        }
        protected IEnumerator Tapping(UnityAction handle)
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                handle.Invoke();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);

            //获取所有的按钮
            buttonList = GetComponentsInChildren<UiButtonAbstract>().ToList();

            //如果当前没有选中的按钮，或者选中的按钮无效
            //则选中第一个按钮
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    nowSelectButton.SelectWithoutAudio();
                }
            }
            else
            {
                nowSelectButton.SelectWithoutAudio();
            }

            //将当前层推入栈
            InputManager.Push_InputAcceptor(this);

            openEvent.Invoke();
        }
        public virtual void Close()
        {
            if (nowSelectButton != null)
            {
                nowSelectButton.UnSelect();
            }

            buttonList.Clear();

            InputManager.Pop_InputAcceptor(this);

            gameObject.SetActive(false);
            closeEvent.Invoke();
        }
        public virtual void Up()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.Up();

            if (!nowSelectButton.CouldUp)
            {
                return;
            }

            var nextButtons = GetUpButtons(buttonList, nowSelectButton);

            UiButtonAbstract nextButton;

            if (nextButtons.Count() == 0)
            {
                nextButton = GetBottomButton(buttonList, nowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, nowSelectButton);
            }

            if (nextButton != null)
            {
                JumpButton(nextButton);
            }

        }
        public virtual void Down()
        {
            if (SelectFirstButton())
            {
                return;
            }
            nowSelectButton.Down();

            if (!nowSelectButton.CouldDown)
            {
                return;
            }


            var nextButtons = GetDownButtons(buttonList, nowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count() == 0)
            {
                nextButton = GetTopButton(buttonList, nowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, nowSelectButton);
            }
            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void Left()
        {
            if (SelectFirstButton())
            {
                return;
            }
            nowSelectButton.Left();

            if (!nowSelectButton.CouldLeft)
            {
                return;
            }


            var nextButtons = GetLeftButtons(buttonList, nowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count() == 0)
            {
                nextButton = GetRightMostButton(buttonList, nowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, nowSelectButton);
            }
            if (nextButton != null)
            {
                JumpButton(nextButton);
            }

        }
        public virtual void Right()
        {
            if (SelectFirstButton())
            {
                return;
            }

            nowSelectButton.Right();

            if (!nowSelectButton.CouldRight)
            {
                return;
            }


            var nextButtons = GetRightButtons(buttonList, nowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count() == 0)
            {
                nextButton = GetLeftMostButton(buttonList, nowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, nowSelectButton);
            }
            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void OK()
        {
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                return;
            }

            nowSelectButton.OK();
        }
        public virtual void Cancel()
        {
            if (couldKeyToClose)
            {
                if (uiLayerGroup != null)
                {
                    uiLayerGroup.Close();
                    return;
                }
                Close();
            }
        }
        public virtual void LeftPage()
        {
            if (uiLayerGroup != null)
            {
                uiLayerGroup.LeftPage();
            }
        }
        public virtual void RightPage()
        {
            if (uiLayerGroup != null)
            {
                uiLayerGroup.RightPage();
            }
        }
        public virtual void UpRoll()
        {
        }
        public virtual void DownRoll()
        {
        }

        public void JumpButton(UiButtonAbstract nextButton)
        {
            if (buttonList.Contains(nextButton))
            {
                if (nowSelectButton != null)
                {
                    nowSelectButton.UnSelect();
                }
                nextButton.Select();
                nowSelectButton = nextButton;
            }
        }

        public void AddButton(UiButtonAbstract button)
        {
            if (!buttonList.Contains(button))
            {
                buttonList.Add(button);
            }
        }
        public void RemoveButton(UiButtonAbstract button)
        {
            if (buttonList.Contains(button))
            {
                if (button.IsSelected)
                {
                    button.UnSelect();
                    FindFirstButton();
                }
            }
            buttonList.Remove(button);
        }

        #region Listener
        public void AddOpenEventListener(UnityAction action)
        {
            openEvent.AddListener(action);
        }
        public void AddCloseEventListener(UnityAction action)
        {
            closeEvent.AddListener(action);
        }
        public void RemoveOpenEventListener(UnityAction action)
        {
            openEvent.RemoveListener(action);
        }
        public void RemoveCloseEventListener(UnityAction action)
        {
            closeEvent.RemoveListener(action);
        }
        #endregion

        protected bool SelectFirstButton()
        {
            //如果当前没有选中按钮或者当前选中的按钮无效,就尝试选中第一个按钮
            if (nowSelectButton == null || nowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    nowSelectButton.Select();
                }
                return true;
            }
            return false;
        }
        //获取第一个按钮（左上角）
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
                button.UnSelect();
                if (Mathf.Abs(button.AnchoredPosition.x - nowButton.AnchoredPosition.x) < 1)
                {
                    if (button.AnchoredPosition.y > nowButton.AnchoredPosition.y)
                    {
                        nowButton = button;
                    }
                }
                else if (button.AnchoredPosition.x < nowButton.AnchoredPosition.x)
                {
                    nowButton = button;
                }
            }
            nowSelectButton = nowButton;
            return true;
        }

        //获取上下左右方向的按钮
        protected static IEnumerable<UiButtonAbstract> GetUpButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.RightBottom.y > aim.LeftTop.y);
        }
        protected static IEnumerable<UiButtonAbstract> GetDownButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.LeftTop.y < aim.RightBottom.y);
        }
        protected static IEnumerable<UiButtonAbstract> GetLeftButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.RightBottom.x < aim.LeftTop.x);
        }
        protected static IEnumerable<UiButtonAbstract> GetRightButtons(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.LeftTop.x > aim.RightBottom.x);
        }

        protected static UiButtonAbstract GetTopButton(IEnumerable<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count() == 0)
            {
                return null;
            }

            UiButtonAbstract res = buttons.First();
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.AnchoredPosition.y - res.AnchoredPosition.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.AnchoredPosition.y > res.AnchoredPosition.y)
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
                if (Mathf.Abs(button.AnchoredPosition.y - res.AnchoredPosition.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.AnchoredPosition.y < res.AnchoredPosition.y)
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
                if (Mathf.Abs(button.AnchoredPosition.x - res.AnchoredPosition.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.AnchoredPosition.x < res.AnchoredPosition.x)
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
                if (Mathf.Abs(button.AnchoredPosition.x - res.AnchoredPosition.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.AnchoredPosition.x > res.AnchoredPosition.x)
                {
                    res = button;
                }
            }
            return res;
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
            return (newButton.AnchoredPosition - aim.AnchoredPosition).sqrMagnitude < (oldButton.AnchoredPosition - aim.AnchoredPosition).sqrMagnitude;
        }
    }
}

