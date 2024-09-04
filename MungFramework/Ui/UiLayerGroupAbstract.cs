using MungFramework.Extension.ComponentExtension;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public abstract class UiLayerGroupAbstract : MonoBehaviour
    {
        [SerializeField]
        protected List<UiLayerAbstract> uiLayerList = new();

        //当前layer的索引
        [SerializeField]
        protected int nowLayerIndex;

        [SerializeField]
        protected UiLayerAbstract nowLayer;


        [SerializeField]
        protected UnityEvent openEvent = new(), closeEvent = new();



        public virtual void LeftPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }
            int index = (nowLayerIndex + uiLayerList.Count - 1) % uiLayerList.Count;
            Jump(index, true);
        }
        public virtual void RightPage()
        {
            if (uiLayerList.Empty())
            {
                return;
            }
            int index = (nowLayerIndex + uiLayerList.Count + 1) % uiLayerList.Count;
            Jump(index, false);
        }

        public virtual void Jump(int index, bool isleft)
        {
            if (index < 0 || index >= uiLayerList.Count)
            {
                return;
            }

            if (nowLayer != null)
            {
                nowLayer.Close();
            }

            nowLayer = uiLayerList[index];

            if (nowLayer != null)
            {
                nowLayer.Open();
            }

            nowLayerIndex = index;
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            if (nowLayer == null)
            {
                nowLayer = uiLayerList[0];
                nowLayerIndex = 0;
            }
            openEvent.Invoke();

            if (nowLayer != null)
            {
                nowLayer.Open();
            }            
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
    }

}