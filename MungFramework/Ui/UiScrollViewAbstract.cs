using DG.Tweening;
using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    public abstract class UiScrollViewAbstract : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform viewport;
        [SerializeField]
        protected RectTransform content;

        //��ť����߽�ľ���
        [SerializeField]
        protected float upLimit,downLimit,leftLimit,rightLimit;

        [ShowInInspector]
        protected Vector2 viewportPosition => viewport == null ? Vector2.zero : viewport.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 viewportLeftTop => viewport == null ? Vector2.zero : viewport.MLeftTop();
        [ShowInInspector]
        protected Vector2 viewportRightBottom => viewport == null ? Vector2.zero : viewport.MRightBottom();

        [ShowInInspector]
        protected Vector2 contentPosition => content == null ? Vector2.zero : content.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 contentLeftTop => content == null ? Vector2.zero : content.MLeftTop();
        [ShowInInspector]
        protected Vector2 contentRightBottom => content == null ? Vector2.zero : content.MRightBottom();



        //����Content��λ��
        public virtual void UpdatePosition(UiButtonAbstract button)
        {
            Vector2 aimPos = Vector2.zero;


            var btnLeftTop = button.LeftTop + contentPosition;//��ť�����Ͻ�����
            var btnRightBottom = button.RightBottom + contentPosition;//��ť�����½�����

            var dViewPortLeftTop = viewportLeftTop + new Vector2(leftLimit, -upLimit);
            var dViewPortRightBottom = viewportRightBottom + new Vector2(-rightLimit, downLimit);

            float deltay = 0f;
            float deltax = 0f;

            //������ݵĸ߶ȴ���ViewPort�ĸ߶ȣ�����λ��
            if (content.MRectSize().y > viewport.MRectSize().y)
            {
                //�����ť��view���棬ContentӦ�������ƶ�
                if (btnLeftTop.y > dViewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - dViewPortLeftTop.y + upLimit);
                }
                //�����ť��view���棬ContentӦ�������ƶ�
                else if (btnRightBottom.y < dViewPortRightBottom.y)
                {
                    deltay = dViewPortRightBottom.y - btnRightBottom.y + downLimit;
                }
            }

            if (content.MRectSize().x > viewport.MRectSize().x)
            {
                //�����ť����ߣ�ContentӦ�������ƶ�
                if (btnLeftTop.x < dViewPortLeftTop.x)
                {
                    deltax = dViewPortLeftTop.x - btnLeftTop.x + leftLimit;
                }
                //�����ť���ұߣ�ContentӦ�������ƶ�
                else if (btnRightBottom.x > dViewPortRightBottom.x)
                {
                    deltax = -(btnRightBottom.x - dViewPortRightBottom.x + rightLimit);
                }
            }

            aimPos = content.MAnchoredPosition() + new Vector2(deltax, deltay);

            //�ƶ����Content�����Ͻ���������½�����
            Vector2 AimLeftTop = contentLeftTop + new Vector2(deltax, deltay);
            Vector2 AimRightBottom = contentRightBottom + new Vector2(deltax, deltay);

            if (content.MRectSize().y > viewport.MRectSize().y)
            {
                if (AimLeftTop.y < viewportLeftTop.y)
                {
                    aimPos.y += viewportLeftTop.y - AimLeftTop.y;
                }
                else if (AimRightBottom.y > viewportRightBottom.y)
                {
                    aimPos.y -= AimRightBottom.y - viewportRightBottom.y;
                }
            }
            if (content.MRectSize().x > viewport.MRectSize().x)
            {
                if (AimLeftTop.x > viewportLeftTop.x)
                {
                    aimPos.x -= AimLeftTop.x - viewportLeftTop.x;
                }
                else if (AimRightBottom.x < viewportRightBottom.x)
                {
                    aimPos.x += viewportRightBottom.x - AimRightBottom.x;
                }
            }
            content.DOKill();
            content.DOAnchorPos(aimPos, 0.15f).SetEase(Ease.OutCirc);
        }

    }
}