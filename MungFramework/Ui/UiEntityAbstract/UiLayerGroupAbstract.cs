using MungFramework.Algorithm;
using MungFramework.Extension.ListExtension;
using MungFramework.Logic.Sound;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField]
        protected UiLayerGroupTitleAbstract groupTitle;
        [SerializeField]
        protected List<UiLayerAbstract> uiLayerList = new();
        [SerializeField]
        protected UiLayerAbstract nowLayer;
        [SerializeField]
        protected int nowLayerIndex;
        [SerializeField]
        protected AudioClip pageAudio;

        #region Countrol
        public virtual void LeftPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }
            if (pageAudio != null)
            {
                SoundManagerAbstract.Instance.PlayAudioOneShot(VolumeTypeEnum.Effect,pageAudio);
            }
            int nextIndex = nowLayerIndex;
            nextIndex.RollNum(0, uiLayerList.Count - 1, -1);
            Jump(nextIndex, JumpDirection.Left);
        }

        public virtual void RightPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }
            if (pageAudio != null)
            {
                SoundManagerAbstract.Instance.PlayAudioOneShot(VolumeTypeEnum.Effect, pageAudio);
            }
            int nextIndex = nowLayerIndex;
            nextIndex.RollNum(0, uiLayerList.Count - 1, 1);
            Jump(nextIndex, JumpDirection.Right);
        }

        protected virtual void Jump(int index, JumpDirection jumpDirection)
        {
            if (index < 0 || index >= uiLayerList.Count || index == nowLayerIndex)
            {
                return;
            }

            nowLayer?.Close();
            groupTitle?.OnLayerChange(index);

            nowLayerIndex = index;
            nowLayer = uiLayerList[nowLayerIndex];
            nowLayer?.Open();
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
                groupTitle?.OnLayerOpen(nowLayerIndex);
            }
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            nowLayer?.Close();
            CallAction(ON_LAYERGROUP_CLOSE);
            gameObject.SetActive(false);
        }
        #endregion
    }

}