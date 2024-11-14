using MungFramework.Algorithm;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// 左右选择按钮，用于设置界面
    /// 例如    难度设置：  简单|普通|困难|地狱
    /// 通过按左右键设置难度
    /// 一般在打开界面时根据当前设置的难度初始化，按下左右键时通过回调函数设置难度
    /// 至于回调时传什么参数，如果使用Enum的话，只能使用模板，会导致子类过多，不够通用
    /// 因此回调时传int，代表选项的index，至于index对应的选项，由外部自行处理
    /// </summary>
    public class UiSelectButton : UiButtonAbstract
    {
        public static string ON_OPTION_CHANGE = "ON_OPTION_CHANGE";

        [Serializable]
        public class SelectOptionItem
        {
            public RectTransform Button;
            public RectTransform SelectObject;
            public RectTransform UnSelectObject;
        }

        [SerializeField]
        protected RectTransform slider;
        [SerializeField]
        protected List<SelectOptionItem> selectOptionItemList = new();
        [SerializeField]
        protected SelectOptionItem nowSelectOptionItem;
        [SerializeField]
        protected int nowSelectIndex = 0;


        protected virtual void FixedUpdate()
        {
            if (slider != null)
            {
                if (nowSelectOptionItem != null && nowSelectOptionItem.Button != null)
                {
                    slider.gameObject.SetActive(true);
                    slider.position = Vector3.Lerp(slider.position, nowSelectOptionItem.Button.position,
                        StaticData.FixedDeltaTimeLerpValue_Faster);
                    slider.sizeDelta = Vector2.Lerp(slider.sizeDelta, nowSelectOptionItem.Button.sizeDelta,
                        StaticData.FixedDeltaTimeLerpValue_Faster);
                }
                else
                {
                    slider.gameObject.SetActive(false);
                }
            }
        }



        public void SetOption(int index,bool setSlider = true,bool callOnOptionChange = false)
        {
            index.Clamp(0, selectOptionItemList.Count - 1);
            foreach (var item in selectOptionItemList)
            {
                if (item.SelectObject != null)
                {
                    item.SelectObject.gameObject.SetActive(false);
                }
                if (item.UnSelectObject != null)
                {
                    item.UnSelectObject.gameObject.SetActive(true);
                }
            }
            nowSelectIndex = index;
            nowSelectOptionItem = selectOptionItemList[index];
            if (nowSelectOptionItem.SelectObject != null)
            {
                nowSelectOptionItem.SelectObject.gameObject.SetActive(true);
            }
            if (nowSelectOptionItem.UnSelectObject != null)
            {
                nowSelectOptionItem.UnSelectObject.gameObject.SetActive(false);
            }

            if (setSlider&&slider!=null)
            {
                if (nowSelectOptionItem != null && nowSelectOptionItem.Button != null)
                {
                    slider.gameObject.SetActive(true);
                    slider.position = nowSelectOptionItem.Button.position;
                    slider.sizeDelta = nowSelectOptionItem.Button.sizeDelta;
                }
                else
                {
                    slider.gameObject.SetActive(false);
                }
            }
            if (callOnOptionChange)
            {
                CallAction_OnOptionChange(nowSelectIndex);
            }
        }
        public override void OnLeft()
        {
            if (nowSelectIndex == 0)
            {
                return;
            }
            nowSelectIndex--;
            SetOption(nowSelectIndex,false,true);
        }
        public override void OnRight()
        {
            if (nowSelectIndex == selectOptionItemList.Count - 1)
            {
                return;
            }
            nowSelectIndex++;
            SetOption(nowSelectIndex,false,true);
        }

        public void AddListener_OnOptionChange(UnityAction<int> action)
        {
            AddListener_Action(ON_OPTION_CHANGE, action);
        }

        protected void CallAction_OnOptionChange(int index)
        {
            CallAction(ON_OPTION_CHANGE,index);
        }
    }
}
