using DG.Tweening;
using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    public abstract class UiScrollViewAbstract : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform Viewport;
        [SerializeField]
        protected RectTransform Content;

        //��ť����߽�ľ���
        [SerializeField]
        protected float UpLimit,DownLimit,LeftLimit,RightLimit;

        [ShowInInspector]
        protected Vector2 ViewportPosition => Viewport == null ? Vector2.zero : Viewport.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 ViewportLeftTop => Viewport == null ? Vector2.zero : Viewport.MLeftTop();
        [ShowInInspector]
        protected Vector2 ViewportRightBottom => Viewport == null ? Vector2.zero : Viewport.MRightBottom();

        [ShowInInspector]
        protected Vector2 ContentPosition => Content == null ? Vector2.zero : Content.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 ContentLeftTop => Content == null ? Vector2.zero : Content.MLeftTop();
        [ShowInInspector]
        protected Vector2 ContentRightBottom => Content == null ? Vector2.zero : Content.MRightBottom();



        //����Content��λ��
        public virtual void UpdatePosition(UiButtonAbstract button)
        {
            Vector2 AimPos = Vector2.zero;


            var btnLeftTop = button.LeftTop + ContentPosition;//��ť�����Ͻ�����
            var btnRightBottom = button.RightBottom + ContentPosition;//��ť�����½�����

            var dViewPortLeftTop = ViewportLeftTop + new Vector2(LeftLimit, -UpLimit);
            var dViewPortRightBottom = ViewportRightBottom + new Vector2(-RightLimit, DownLimit);

            float deltay = 0f;
            float deltax = 0f;
            //������ݵĸ߶ȴ���ViewPort�ĸ߶ȣ�����λ��
            if (Content.MRectSize().y > Viewport.MRectSize().y)
            {
                //�����ť��view���棬ContentӦ�������ƶ�
                if (btnLeftTop.y > dViewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - dViewPortLeftTop.y + UpLimit);
                }
                //�����ť��view���棬ContentӦ�������ƶ�
                else if (btnRightBottom.y < dViewPortRightBottom.y)
                {
                    deltay = dViewPortRightBottom.y - btnRightBottom.y + DownLimit;
                }
            }
            if (Content.MRectSize().x > Viewport.MRectSize().x)
            {
                //�����ť����ߣ�ContentӦ�������ƶ�
                if (btnLeftTop.x < dViewPortLeftTop.x)
                {
                    deltax = dViewPortLeftTop.x - btnLeftTop.x + LeftLimit;
                }
                //�����ť���ұߣ�ContentӦ�������ƶ�
                else if (btnRightBottom.x > dViewPortRightBottom.x)
                {
                    deltax = -(btnRightBottom.x - dViewPortRightBottom.x + RightLimit);
                }
            }
            AimPos = Content.MAnchoredPosition() + new Vector2(deltax, deltay);
            //�ƶ����Content�����Ͻ���������½�����
            Vector2 AimLeftTop = ContentLeftTop + new Vector2(deltax, deltay);
            Vector2 AimRightBottom = ContentRightBottom + new Vector2(deltax, deltay);

            if (Content.MRectSize().y > Viewport.MRectSize().y)
            {
                if (AimLeftTop.y < ViewportLeftTop.y)
                {
                    AimPos.y += ViewportLeftTop.y - AimLeftTop.y;
                }
                else if (AimRightBottom.y > ViewportRightBottom.y)
                {
                    AimPos.y -= AimRightBottom.y - ViewportRightBottom.y;
                }
            }
            if (Content.MRectSize().x > Viewport.MRectSize().x)
            {
                if (AimLeftTop.x > ViewportLeftTop.x)
                {
                    AimPos.x -= AimLeftTop.x - ViewportLeftTop.x;
                }
                else if (AimRightBottom.x < ViewportRightBottom.x)
                {
                    AimPos.x += ViewportRightBottom.x - AimRightBottom.x;
                }
            }

            Content.DOKill();
            Content.DOAnchorPos(AimPos, 0.15f).SetEase(Ease.OutCirc);
        }

    }
}