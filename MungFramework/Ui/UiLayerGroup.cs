using MungFramework.ComponentExtend;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public abstract class UiLayerGroup : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        protected List<UiLayer> Layers;
        [SerializeField]
        [ReadOnly]
        protected UiLayer NowLayer;
        [SerializeField]
        [ReadOnly]
        protected UnityEvent OpenEvent, CloseEvent;

        //当前layer的索引
        [SerializeField]
        [ReadOnly]
        protected int NowIndex;

        public virtual void LeftPage()
        {
            if (Layers.Empty())
            {
                return;
            }
            int index = (NowIndex-1+Layers.Count)%Layers.Count;
            Jump(index,true);
        }
        public virtual void RightPage()
        {
            if (Layers.Empty())
            {
                return;
            }
            int index = (NowIndex + 1 + Layers.Count) % Layers.Count;
            Jump(index,false);
        }

        public virtual void Jump(int index,bool isleft)
        {
            if (index < 0 || index >= Layers.Count)
            {
                return;
            }
            NowLayer.Close();
            NowLayer = Layers[index];
            NowLayer.Open();
            NowIndex = index;
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            if (NowLayer == null)
            {
                NowLayer = Layers[0];
                NowIndex = 0;
            }
            OpenEvent.Invoke();
            NowLayer?.Open();
        }
        public virtual void Close()
        {
            NowLayer?.Close();
            gameObject.SetActive(false);
            CloseEvent.Invoke();
        }
    }

}