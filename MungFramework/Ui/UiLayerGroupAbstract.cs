using MungFramework.ComponentExtension;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public abstract class UiLayerGroupAbstract : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        protected List<UiLayerAbstract> UiLayerList;

        //当前layer的索引
        [SerializeField]
        [ReadOnly]
        protected int NowOpenedLayerIndex;

        [SerializeField]
        [ReadOnly]
        protected UiLayerAbstract NowOpenedLayer;


        [SerializeField]
        [ReadOnly]
        protected UnityEvent OpenEvent, CloseEvent;



        public virtual void LeftPage()
        {
            if (UiLayerList.Empty())
            {
                return;
            }
            int index = (NowOpenedLayerIndex-1+UiLayerList.Count)%UiLayerList.Count;
            Jump(index,true);
        }
        public virtual void RightPage()
        {
            if (UiLayerList.Empty())
            {
                return;
            }
            int index = (NowOpenedLayerIndex + 1 + UiLayerList.Count) % UiLayerList.Count;
            Jump(index,false);
        }

        public virtual void Jump(int index,bool isleft)
        {
            if (index < 0 || index >= UiLayerList.Count)
            {
                return;
            }
            NowOpenedLayer.Close();
            NowOpenedLayer = UiLayerList[index];
            NowOpenedLayer.Open();
            NowOpenedLayerIndex = index;
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            if (NowOpenedLayer == null)
            {
                NowOpenedLayer = UiLayerList[0];
                NowOpenedLayerIndex = 0;
            }
            OpenEvent.Invoke();
            NowOpenedLayer?.Open();
        }
        public virtual void Close()
        {
            NowOpenedLayer?.Close();
            gameObject.SetActive(false);
            CloseEvent.Invoke();
        }
    }

}