using MungFramework.Extension.LifeCycleExtension;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// UiLayerGroup的标题
    /// 切换页面时会相应切换标题
    /// </summary>
    public class UiLayerGroupTittleAbstract : UiEntityAbstract
    {
        [Serializable]
        public class TittleButton
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
        protected List<TittleButton> tittleButtonList;
        [SerializeField]
        protected TittleButton nowSelectTittleButton;


        protected virtual void FixedUpdate()
        {
            if (selectPoint != null)
            {
                if (nowSelectTittleButton != null && nowSelectTittleButton.Button != null)
                {
                    selectPoint.gameObject.SetActive(true);
                    selectPoint.position = Vector3.Lerp(selectPoint.position, nowSelectTittleButton.Button.position,
                        StaticData.FixedDeltaTimeLerpValue_Bigger);
                    selectPoint.sizeDelta = Vector2.Lerp(selectPoint.sizeDelta, nowSelectTittleButton.Button.sizeDelta,
                        StaticData.FixedDeltaTimeLerpValue_Bigger);
                }
                else
                {
                    selectPoint.gameObject.SetActive(false);
                }
            }
        }

        public virtual void OnLayerChange(int index)
        {
            foreach (var button in tittleButtonList)
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

            if (index >= 0 && index < tittleButtonList.Count)
            {
                if (tittleButtonList[index].SelectObject != null)
                {
                    tittleButtonList[index].SelectObject.gameObject.SetActive(true);
                }
                if (tittleButtonList[index].UnSelectObject != null)
                {
                    tittleButtonList[index].UnSelectObject.gameObject.SetActive(false);
                }
                nowSelectTittleButton = tittleButtonList[index];


                scrollView.UpdatePosition(tittleButtonList[index].Button.GetComponent<RectTransform>());

                UpdateScrollView(index);
            }
        }
        private void UpdateScrollView(int index)
        {
            void action()
            {
                scrollView.UpdatePosition(tittleButtonList[index].Button.GetComponent<RectTransform>());
            }
            if (scrollView != null)
            {
                LifeCycleExtension.LateUpdateHelp(action);
            }
        }
    }
}
