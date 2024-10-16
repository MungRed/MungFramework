using MungFramework.Extension.ComponentExtension;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// UiLayerGroup抽象类
    /// 包含若干UiLayer
    /// 可以在各个Layer之间切换
    /// 可以直接打开Group，功能是打开预设或关闭前的Layer(!!!!!!!!在有Group的情况下都优先通过Group打开层!!!!!!!!!!)
    /// 可以直接关闭Group，功能是关闭当前Layer
    /// 当Layer接收到Cancel时间时，会自动关闭Group
    /// </summary>
    public abstract class UiLayerGroupAbstract : UiEntityAbstract
    {
        public enum JumpDirection
        {
            Left, Right
        }

        public UiEventModel LayerGroupEvent = new();

        [SerializeField]
        protected UnityEvent openEvent = new();
        [SerializeField]
        protected UnityEvent closeEvent = new();

        [SerializeField]
        protected UiLayerGroupTittleAbstract groupTittle;
        [SerializeField]
        protected List<UiLayerAbstract> uiLayerList = new();
        [SerializeField]
        protected UiLayerAbstract nowLayer;
        [SerializeField]
        protected int nowLayerIndex;


        #region Countrol
        public virtual void LeftPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }

            int index = (nowLayerIndex + uiLayerList.Count - 1) % uiLayerList.Count;
            Jump(index, JumpDirection.Left);
        }

        public virtual void RightPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }

            int index = (nowLayerIndex + uiLayerList.Count + 1) % uiLayerList.Count;
            Jump(index, JumpDirection.Right);
        }

        protected virtual void Jump(int index, JumpDirection jumpDirection)
        {
            if (index < 0 || index >= uiLayerList.Count || index == nowLayerIndex)
            {
                return;
            }

            if (nowLayer != null)
            {
                nowLayer.Close();
            }

            groupTittle?.OnLayerChange(index);

            nowLayer = uiLayerList[index];
            nowLayerIndex = index;

            if (nowLayer != null)
            {
                nowLayer.Open();
            }
        }

        #endregion
        #region OpenAndClose
        public virtual void Open()
        {
            if (nowLayer == null && uiLayerList.Count > 0)
            {
                nowLayer = uiLayerList[0];
                nowLayerIndex = 0;
            }

            if (nowLayer != null)
            {
                nowLayer.Open();
                groupTittle?.OnLayerChange(nowLayerIndex);
            }

            gameObject.SetActive(true);
            openEvent.Invoke();
        }

        public virtual void Close()
        {
            if (nowLayer != null)
            {
                nowLayer.Close();
            }

            gameObject.SetActive(false);
            closeEvent.Invoke();
        }
        #endregion
        #region Event
        public void AddOpenEventListener(UnityAction action) => openEvent.AddListener(action);
        public void AddCloseEventListener(UnityAction action) => closeEvent.AddListener(action);
        public void RemoveOpenEventListener(UnityAction action) => openEvent.RemoveListener(action);
        public void RemoveCloseEventListener(UnityAction action) => closeEvent.RemoveListener(action);

        public virtual void OnButtonSelect(UiButtonAbstract uiButton)
        {
            LayerGroupEvent.Call_OnButtonSelect(uiButton);
        }
        public virtual void OnButtonUnSelect(UiButtonAbstract uiButton)
        {
            LayerGroupEvent.Call_OnButtonUnSelect(uiButton);
        }
        public virtual void OnButtonOK(UiButtonAbstract uiButton)
        {
            LayerGroupEvent.Call_OnButtonOK(uiButton);
        }
        public virtual void OnButtonSpecialAction(UiButtonAbstract uiButton)
        {
            LayerGroupEvent.Call_OnButtonSpecialAction(uiButton);
        }
        public virtual void OnLayerOpen(UiLayerAbstract uiLayer)
        {
            LayerGroupEvent.Call_OnLayerOpen(uiLayer);
        }
        public virtual void OnLayerClose(UiLayerAbstract uiLayer)
        {
            LayerGroupEvent.Call_OnLayerClose(uiLayer);
        }
        #endregion
    }

}