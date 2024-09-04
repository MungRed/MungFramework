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

        //按钮距离边界的距离
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



        //更新Content的位置
        public virtual void UpdatePosition(UiButtonAbstract button)
        {
            Vector2 aimPos = Vector2.zero;


            var btnLeftTop = button.LeftTop + contentPosition;//按钮的左上角坐标
            var btnRightBottom = button.RightBottom + contentPosition;//按钮的右下角坐标

            var dViewPortLeftTop = viewportLeftTop + new Vector2(leftLimit, -upLimit);
            var dViewPortRightBottom = viewportRightBottom + new Vector2(-rightLimit, downLimit);

            float deltay = 0f;
            float deltax = 0f;

            //如果内容的高度大于ViewPort的高度，更新位置
            if (content.MRectSize().y > viewport.MRectSize().y)
            {
                //如果按钮在view上面，Content应该往下移动
                if (btnLeftTop.y > dViewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - dViewPortLeftTop.y + upLimit);
                }
                //如果按钮在view下面，Content应该往上移动
                else if (btnRightBottom.y < dViewPortRightBottom.y)
                {
                    deltay = dViewPortRightBottom.y - btnRightBottom.y + downLimit;
                }
            }

            if (content.MRectSize().x > viewport.MRectSize().x)
            {
                //如果按钮在左边，Content应该往右移动
                if (btnLeftTop.x < dViewPortLeftTop.x)
                {
                    deltax = dViewPortLeftTop.x - btnLeftTop.x + leftLimit;
                }
                //如果按钮在右边，Content应该往左移动
                else if (btnRightBottom.x > dViewPortRightBottom.x)
                {
                    deltax = -(btnRightBottom.x - dViewPortRightBottom.x + rightLimit);
                }
            }

            aimPos = content.MAnchoredPosition() + new Vector2(deltax, deltay);

            //移动后的Content的左上角坐标和右下角坐标
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