using MungFramework.Logic;
using MungFramework.Logic.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public abstract class UiLayer : MonoBehaviour, IInputAcceptor
    {
        [SerializeField]
        protected List<UiButton> Buttons = new();

        [SerializeField]
        protected UiButton NowSelectButton;

        //能否通过按键关闭当前层
        [SerializeField]
        protected bool KeyToClose = true;

        //打开和关闭事件
        [SerializeField]
        protected UnityEvent OpenEvent, CloseEvent;

        protected UiLayerGroup LayerGroup => GetComponentInParent<UiLayerGroup>();
        protected InputManager @InputManager => GameApplication.Instance.InputManager;

        public virtual void OnInput(InputTypeValue inputType)
        {
            LayerControll(inputType);
        }

        public virtual void LayerControll(InputTypeValue inputType)
        {
            switch (inputType)
            {
                case InputTypeValue.NONE:
                    break;
                case InputTypeValue.LEFT:
                    Left();
                    AddTapping(inputType, Left);
                    break;
                case InputTypeValue.UP:
                    Up();
                    AddTapping(inputType, Up);
                    break;
                case InputTypeValue.DOWN:
                    Down();
                    AddTapping(inputType, Down);
                    break;
                case InputTypeValue.RIGHT:
                    Right();
                    AddTapping(inputType, Right);
                    break;
                case InputTypeValue.OK:
                    OK();
                    break;
                case InputTypeValue.CANCEL:
                    Cancel();
                    break;
                case InputTypeValue.LEFT_PAGE:
                    LeftPage();
                    break;
                case InputTypeValue.RIGTH_PAGE:
                    RightPage();
                    break;
                case InputTypeValue.UP_ROLL:
                    UpRoll();
                    break;
                case InputTypeValue.DOWN_ROLL:
                    DownRoll();
                    break;
            }
        }
        private void AddTapping(InputTypeValue input, UnityAction action)
        {
            //开始连续敲击
            var cor = StartCoroutine(Tapping(action));
            UnityAction cancel = () =>
            {
                StopCoroutine(cor);
            };
            cancel += () => InputManager.Remove_ControllInputAction_Canceled(input, cancel);

            InputManager.Add_ControllInputAction_Canceled(input, cancel);
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
            Buttons = GetComponentsInChildren<UiButton>().ToList();

            //如果当前没有选中的按钮，或者选中的按钮无效
            //则选中第一个按钮
            if (NowSelectButton == null || NowSelectButton.gameObject.activeSelf == false)
            {
                if (FindFirstButton())
                {
                    NowSelectButton.SelectWithOutAudio();
                }
            }
            else
            {
                NowSelectButton.SelectWithOutAudio();
            }

            //将当前层推入栈
            InputManager.PushInputAcceptor(this);

            OpenEvent.Invoke();
        }
        public virtual void Close()
        {
            NowSelectButton?.UnSelect();
            Buttons.Clear();
            InputManager.PopInputAcceptor(this);

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

            List<UiButton> nextButtons = GetUpButtons(Buttons, NowSelectButton);

            UiButton nextButton;

            if (nextButtons.Count == 0)
            {
                nextButton = GetBottomButton(Buttons, NowSelectButton);
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


            List<UiButton> nextButtons = GetDownButtons(Buttons, NowSelectButton);
            UiButton nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetTopButton(Buttons, NowSelectButton);
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


            List<UiButton> nextButtons = GetLeftButtons(Buttons, NowSelectButton);
            UiButton nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetRightMostButton(Buttons, NowSelectButton);
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


            List<UiButton> nextButtons = GetRightButtons(Buttons, NowSelectButton);
            UiButton nextButton;
            if (nextButtons.Count == 0)
            {
                nextButton = GetLeftMostButton(Buttons, NowSelectButton);
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
            if (KeyToClose)
            {
                if (LayerGroup != null)
                {
                    LayerGroup.Close();
                    return;
                }
                Close();
            }
        }
        public virtual void LeftPage()
        {
            if (LayerGroup != null)
            {
                LayerGroup.LeftPage();
            }
        }
        public virtual void RightPage()
        {
            if (LayerGroup != null)
            {
                LayerGroup.RightPage();
            }
        }
        public virtual void UpRoll()
        {
        }
        public virtual void DownRoll()
        {
        }

        public void JumpButton(UiButton nextButton)
        {
            if (Buttons.Contains(nextButton))
            {
                if (NowSelectButton != null)
                {
                    NowSelectButton.UnSelect();
                }
                nextButton.Select();
                NowSelectButton = nextButton;
            }
        }

        public void AddButton(UiButton button)
        {
            if (!Buttons.Contains(button))
            {
                Buttons.Add(button);
            }
        }
        public void RemoveButton(UiButton button)
        {
            if (Buttons.Contains(button))
            {
                if (button.isSelected)
                {
                    button.UnSelect();
                    FindFirstButton();
                }
            }
            Buttons.Remove(button);
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
            if (Buttons.Count == 0)
            {
                NowSelectButton = null;
                return false;
            }

            UiButton nowButton = Buttons[0];
            foreach (UiButton button in Buttons)
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
        private static List<UiButton> GetUpButtons(List<UiButton> buttons, UiButton aim)
        {
            return buttons.Where(button => button.RightBottom.y > aim.LeftTop.y).ToList();
        }
        private static List<UiButton> GetDownButtons(List<UiButton> buttons, UiButton aim)
        {
            return buttons.Where(button => button.LeftTop.y < aim.RightBottom.y).ToList();
        }
        private static List<UiButton> GetLeftButtons(List<UiButton> buttons, UiButton aim)
        {
            return buttons.Where(button => button.RightBottom.x < aim.LeftTop.x).ToList();
        }
        private static List<UiButton> GetRightButtons(List<UiButton> buttons, UiButton aim)
        {
            return buttons.Where(button => button.LeftTop.x > aim.RightBottom.x).ToList();
        }

        private static UiButton GetTopButton(List<UiButton> buttons, UiButton aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }

            UiButton res = buttons[0];
            foreach (UiButton button in buttons)
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
        private static UiButton GetBottomButton(List<UiButton> buttons, UiButton aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButton res = buttons[0];
            foreach (UiButton button in buttons)
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
        private static UiButton GetLeftMostButton(List<UiButton> buttons, UiButton aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButton res = buttons[0];
            foreach (UiButton button in buttons)
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
        private static UiButton GetRightMostButton(List<UiButton> buttons, UiButton aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }
            UiButton res = buttons[0];
            foreach (UiButton button in buttons)
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

        private static UiButton GetNearstButton(List<UiButton> buttons, UiButton aim)
        {
            if (buttons.Count == 0)
            {
                return null;
            }

            UiButton nearstButton = buttons[0];
            foreach (UiButton button in buttons)
            {
                if (isNearThanOld(button, nearstButton, aim))
                {
                    nearstButton = button;
                }
            }
            return nearstButton;
        }
        private static bool isNearThanOld(UiButton newButton, UiButton oldButton, UiButton aim)
        {
            return Vector3.Magnitude(newButton.Position - aim.Position) < Vector3.Magnitude(oldButton.Position - aim.Position);
        }
    }
}

