using DG.Tweening;
using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// ������ͼ������
    /// ���Ը���ѡ�е�RectTransform������Content��λ��
    /// </summary>
    public abstract class UiScrollViewAbstract : UiEntityAbstract
    {
        [SerializeField]
        [Required]
        protected RectTransform viewport;
        [SerializeField]
        [Required]
        protected RectTransform content;

        //��ť����߽�ľ���
        [SerializeField]
        protected float upLimit,downLimit,leftLimit,rightLimit;

        public virtual void SetContentZero()
        {
            content.DOKill();
            content.MCanvasPosition_LeftTop_Set(Canvas, viewport.MCanvasPosition_LeftTop(Canvas));
        }

        public virtual void UpdatePosition(RectTransform rectTransform)
        {
            Vector2 contentPosition = content.MCanvasPosition(Canvas);
            Vector2 viewPortLeftTop = viewport.MCanvasPosition_LeftTop(Canvas)+ new Vector2(leftLimit, -upLimit);//ʵ����ͼ�����Ͻ�����
            Vector2 viewPortRightBottom = viewport.MCanvasPosition_RightBottom(Canvas) + new Vector2(-rightLimit, downLimit);//ʵ����ͼ�����½�����

            Vector2 btnLeftTop = rectTransform.MCanvasPosition_LeftTop(Canvas);//��ť�����Ͻ�����
            Vector2 btnRightBottom = rectTransform.MCanvasPosition_RightBottom(Canvas);//��ť�����½�����

            //��Ҫ�ƶ��ľ���
            float deltay = 0f;
            float deltax = 0f;

            //������ݵĸ߶ȴ���ViewPort�ĸ߶ȣ�����λ��
            if (content.MCanvasSize().y > viewport.MCanvasSize().y)
            {
                //�����ť��view���棬ContentӦ�������ƶ�
                if (btnLeftTop.y > viewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - viewPortLeftTop.y);
                }
                //�����ť��view���棬ContentӦ�������ƶ�
                else if (btnRightBottom.y < viewPortRightBottom.y)
                {
                    deltay = viewPortRightBottom.y - btnRightBottom.y;
                }
            }

            if (content.MCanvasSize().x > viewport.MCanvasSize().x)
            {
                //�����ť����ߣ�ContentӦ�������ƶ�
                if (btnLeftTop.x < viewPortLeftTop.x)
                {
                    deltax = viewPortLeftTop.x - btnLeftTop.x;
                }
                //�����ť���ұߣ�ContentӦ�������ƶ�
                else if (btnRightBottom.x > viewPortRightBottom.x)
                {
                    deltax = -(btnRightBottom.x - viewPortRightBottom.x);
                }
            }

            contentPosition += new Vector2(deltax, deltay);

            content.DOKill();
            content.DOCanvasPosition(Canvas,contentPosition, 0.15f).SetEase(Ease.OutCirc);
        }




        [ShowInInspector]
        private Vector2 viewportPosition => viewport == null ? Vector2.zero : viewport.MCanvasPosition(Canvas);

        [ShowInInspector]
        private Vector2 viewportLeftTop => viewport == null ? Vector2.zero : viewport.MCanvasPosition_LeftTop(Canvas);

        [ShowInInspector]
        private Vector2 viewportRightBottom => viewport == null ? Vector2.zero : viewport.MCanvasPosition_RightBottom(Canvas);

        [ShowInInspector]
        private Vector2 contentPosition => content == null ? Vector2.zero : content.MCanvasPosition(Canvas);

        [ShowInInspector]
        private Vector2 contentLeftTop => content == null ? Vector2.zero : content.MCanvasPosition_LeftTop(Canvas);

        [ShowInInspector]
        private Vector2 contentRightBottom => content == null ? Vector2.zero : content.MCanvasPosition_RightBottom(Canvas);
    }
}