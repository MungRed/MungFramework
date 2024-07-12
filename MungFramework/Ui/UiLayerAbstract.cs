using MungFramework.Logic;
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
        [SerializeField]
        protected List<UiButtonAbstract> ButtonList = new();

        [SerializeField]
        protected UiButtonAbstract NowSelectButton;

        //能否通过按键关闭当前层
        [SerializeField]
        protected bool CouldKeyToClose = true;

        //打开和关闭事件
        [SerializeField]
        protected UnityEvent OpenEvent, CloseEvent;

        protected UiLayerGroupAbstract UiLayerGroup => GetComponentInParent<UiLayerGroupAbstract>();

        public InputManagerAbstract InputManager =>InputManagerAbstract.Instance;

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
        private void AddTapping(InputValueEnum input, UnityAction action)
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

        private IEnumerator Tapping(UnityAction handle)
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                handle?.Invoke();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);

            //获取所有的按钮
            ButtonList = GetComponentsInChildren<UiButtonAbstract>().ToList();

            //如果当前没有选中的按钮，或者选中的按钮无效
            //则选中第一个按钮
            if (NowSelectButton == null || NowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    NowSelectButton.SelectWithoutAudio();
                }
            }
            else
            {
                NowSelectButton.SelectWithoutAudio();
            }

            //将当前层推入栈
            InputManager.Push_InputAcceptor(this);

            OpenEvent.Invoke();
        }
        public virtual void Close()
        {
            NowSelectButton?.UnSelect();
            ButtonList.Clear();
            InputManager.Pop_InputAcceptor(this);

            gameObject.SetActive(false);
            CloseEvent.Invoke();
        }
        public virtual void Up()
        {
            if (SelectFirstButton())
            {
                return;
            }

            NowSelectButton.Up();

            if (!NowSelectButton.CouldUp)
            {
                return;
            }

            List<UiButtonAbstract> nextButtons = GetUpButtons(ButtonList, NowSelectButton);

            UiButtonAbstract nextButton;

            if (nextButtons.Count == 0)
            {
                nextButton = GetBottomButton(ButtonList, NowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, NowSelectButton);
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
            NowSelectButton.Down();

            if (!NowSelectButton.CouldDown)
            {
                return;
            }


            List<UiButtonAbstract> nextButtons = GetDownButtons(ButtonList, NowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetTopButton(ButtonList, NowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, NowSelectButton);
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
            NowSelectButton.Left();

            if (!NowSelectButton.CouldLeft)
            {
                return;
            }


            List<UiButtonAbstract> nextButtons = GetLeftButtons(ButtonList, NowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetRightMostButton(ButtonList, NowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, NowSelectButton);
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

            NowSelectButton.Right();

            if (!NowSelectButton.CouldRight)
            {
                return;
            }


            List<UiButtonAbstract> nextButtons = GetRightButtons(ButtonList, NowSelectButton);
            UiButtonAbstract nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetLeftMostButton(ButtonList, NowSelectButton);
            }
            else
            {
                nextButton = GetNearstButton(nextButtons, NowSelectButton);
            }
            if (nextButton != null)
            {
                JumpButton(nextButton);
            }
        }
        public virtual void OK()
        {
            if (NowSelectButton == null || NowSelectButton.gameObject.activeSelf == false)
            {
                return;
            }

            NowSelectButton.OK();
        }
        public virtual void Cancel()
        {
            if (CouldKeyToClose)
            {
                if (UiLayerGroup != null)
                {
                    UiLayerGroup.Close();
                    return;
                }
                Close();
            }
        }
        public virtual void LeftPage()
        {
            if (UiLayerGroup != null)
            {
                UiLayerGroup.LeftPage();
            }
        }
        public virtual void RightPage()
        {
            if (UiLayerGroup != null)
            {
                UiLayerGroup.RightPage();
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
            if (ButtonList.Contains(nextButton))
            {
                if (NowSelectButton != null)
                {
                    NowSelectButton.UnSelect();
                }
                nextButton.Select();
                NowSelectButton = nextButton;
            }
        }

        public void AddButton(UiButtonAbstract button)
        {
            if (!ButtonList.Contains(button))
            {
                ButtonList.Add(button);
            }
        }
        public void RemoveButton(UiButtonAbstract button)
        {
            if (ButtonList.Contains(button))
            {
                if (button.IsSelected)
                {
                    button.UnSelect();
                    FindFirstButton();
                }
            }
            ButtonList.Remove(button);
        }

        private bool SelectFirstButton()
        {
            //如果当前没有选中按钮或者当前选中的按钮无效,就尝试选中第一个按钮
            if (NowSelectButton == null || NowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    NowSelectButton.Select();
                }
                return true;
            }
            return false;
        }
        //获取第一个按钮（左上角）
        private bool FindFirstButton()
        {
            if (ButtonList.Count == 0)
            {
                NowSelectButton = null;
                return false;
            }

            UiButtonAbstract nowButton = ButtonList[0];
            foreach (UiButtonAbstract button in ButtonList)
            {
                button.UnSelect();
                if (Mathf.Abs(button.Position.x - nowButton.Position.x) < 1)
                {
                    if (button.Position.y > nowButton.Position.y)
                    {
                        nowButton = button;
                    }
                }
                else if (button.Position.x < nowButton.Position.x)
                {
                    nowButton = button;
                }
            }
            NowSelectButton = nowButton;
            return true;
        }

        //获取上下左右方向的按钮
        private static List<UiButtonAbstract> GetUpButtons(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.RightBottom.y > aim.LeftTop.y).ToList();
        }
        private static List<UiButtonAbstract> GetDownButtons(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.LeftTop.y < aim.RightBottom.y).ToList();
        }
        private static List<UiButtonAbstract> GetLeftButtons(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.RightBottom.x < aim.LeftTop.x).ToList();
        }
        private static List<UiButtonAbstract> GetRightButtons(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            return buttons.Where(button => button.LeftTop.x > aim.RightBottom.x).ToList();
        }

        private static UiButtonAbstract GetTopButton(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }

            UiButtonAbstract res = buttons[0];
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.Position.y - res.Position.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.Position.y > res.Position.y)
                {
                    res = button;
                }
            }
            return res;
        }
        private static UiButtonAbstract GetBottomButton(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons[0];
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.Position.y - res.Position.y) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.Position.y < res.Position.y)
                {
                    res = button;
                }
            }
            return res;
        }
        private static UiButtonAbstract GetLeftMostButton(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons[0];
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.Position.x - res.Position.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.Position.x < res.Position.x)
                {
                    res = button;
                }
            }
            return res;
        }
        private static UiButtonAbstract GetRightMostButton(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButtonAbstract res = buttons[0];
            foreach (UiButtonAbstract button in buttons)
            {
                if (Mathf.Abs(button.Position.x - res.Position.x) < 1)
                {
                    if (isNearThanOld(button, res, aim))
                    {
                        res = button;
                    }
                }
                else if (button.Position.x > res.Position.x)
                {
                    res = button;
                }
            }
            return res;
        }

        private static UiButtonAbstract GetNearstButton(List<UiButtonAbstract> buttons, UiButtonAbstract aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }

            UiButtonAbstract nearstButton = buttons[0];
            foreach (UiButtonAbstract button in buttons)
            {
                if (isNearThanOld(button, nearstButton, aim))
                {
                    nearstButton = button;
                }
            }
            return nearstButton;
        }
        private static bool isNearThanOld(UiButtonAbstract newButton, UiButtonAbstract oldButton, UiButtonAbstract aim)
        {
            return Vector3.Magnitude(newButton.Position - aim.Position) < Vector3.Magnitude(oldButton.Position - aim.Position);
        }
    }
}

