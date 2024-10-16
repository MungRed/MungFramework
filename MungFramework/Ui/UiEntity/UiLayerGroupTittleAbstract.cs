using MungFramework.Extension.LifeCycleExtension;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// LayerGroupToggle抽象类
    /// 附属于LayerGroup
    /// 当切换页面的时候会执行Toggle相应的动作
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
        [SerializeField]
        protected UnityEvent onLayerChange;


        protected virtual void FixedUpdate()
        {
            if (selectPoint != null)
            {
                if (nowSelectTittleButton != null && nowSelectTittleButton.Button != null)
                {
                    selectPoint.gameObject.SetActive(true);
                    selectPoint.position = Vector3.Lerp(selectPoint.position, nowSelectTittleButton.Button.position, 
                        StaticData.FixedDeltaTimeLerpValue_Bigger);
                    selectPoint.sizeDelta = Vector2.Lerp(selectPoint.sizeDelta,nowSelectTittleButton.Button.sizeDelta,
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
