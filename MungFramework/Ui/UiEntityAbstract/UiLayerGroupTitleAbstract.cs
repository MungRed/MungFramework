using MungFramework.Algorithm;
using MungFramework.Extension.LifeCycleExtension;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// UiLayerGroup的标题
    /// 切换页面时会相应切换标题
    /// </summary>
    public abstract class UiLayerGroupTitleAbstract : UiEntityAbstract
    {
        [Serializable]
        public class TitleButton
        {
            public RectTransform Button;
            public RectTransform SelectObject;
            public RectTransform UnSelectObject;
        }

        [SerializeField]
        protected UiScrollViewAbstract scrollView;
        [SerializeField]
        protected RectTransform selectPoint;
        [SerializeField]
        protected List<TitleButton> titleButtonList;
        [SerializeField]
        protected TitleButton nowSelectTitleButton;


        protected virtual void FixedUpdate()
        {
            if (selectPoint != null)
            {
                if (nowSelectTitleButton != null && nowSelectTitleButton.Button != null)
                {
                    selectPoint.gameObject.SetActive(true);
                    selectPoint.position = Vector3.Lerp(selectPoint.position, nowSelectTitleButton.Button.position,
                        StaticData.FixedDeltaTimeLerpValue_20f);
                    selectPoint.sizeDelta = Vector2.Lerp(selectPoint.sizeDelta, nowSelectTitleButton.Button.sizeDelta,
                        StaticData.FixedDeltaTimeLerpValue_20f);
                }
                else
                {
                    selectPoint.gameObject.SetActive(false);
                }
            }
        }
        public virtual void OnLayerOpen(int index)
        {
            OnLayerChange(index);
            selectPoint.gameObject.SetActive(false);
            if (nowSelectTitleButton != null && nowSelectTitleButton.Button != null)
            {
                UnityAction action = () =>
                {
                    selectPoint.gameObject.SetActive(true);
                    selectPoint.position = nowSelectTitleButton.Button.position;
                    selectPoint.sizeDelta = nowSelectTitleButton.Button.sizeDelta;
                };
                action.LateInvoke();
            }
        }

        public virtual void OnLayerChange(int index)
        {
            index.Clamp(0, titleButtonList.Count - 1);
            foreach (var button in titleButtonList)
            {
                if (button.SelectObject != null)
                {
                    button.SelectObject.gameObject.SetActive(false);
                }
                if (button.UnSelectObject != null)
                {
                    button.UnSelectObject.gameObject.SetActive(true);
                }
            }
            nowSelectTitleButton = titleButtonList[index];
            if (nowSelectTitleButton.SelectObject != null)
            {
                nowSelectTitleButton.SelectObject.gameObject.SetActive(true);
            }
            if (nowSelectTitleButton.UnSelectObject != null)
            {
                nowSelectTitleButton.UnSelectObject.gameObject.SetActive(false);
            }
            UpdateScrollView(index);
        }
        private void UpdateScrollView(int index)
        {
            void action()
            {
                scrollView.UpdatePosition(titleButtonList[index].Button);
            }
            if (scrollView != null)
            {
                LifeCycleExtension.LateInvoke(action);
            }
        }
    }
}
