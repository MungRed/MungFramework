using DG.Tweening;
using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// 滚动视图抽象类
    /// 可以根据选中的RectTransform来更新Content的位置
    /// </summary>
    public abstract class UiScrollViewAbstract : UiEntityAbstract
    {
        [SerializeField]
        [Required]
        protected RectTransform viewport;
        [SerializeField]
        [Required]
        protected RectTransform content;

        //按钮距离边界的距离
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
            Vector2 viewPortLeftTop = viewport.MCanvasPosition_LeftTop(Canvas)+ new Vector2(leftLimit, -upLimit);//实际视图的左上角坐标
            Vector2 viewPortRightBottom = viewport.MCanvasPosition_RightBottom(Canvas) + new Vector2(-rightLimit, downLimit);//实际视图的右下角坐标

            Vector2 btnLeftTop = rectTransform.MCanvasPosition_LeftTop(Canvas);//按钮的左上角坐标
            Vector2 btnRightBottom = rectTransform.MCanvasPosition_RightBottom(Canvas);//按钮的右下角坐标

            //需要移动的距离
            float deltay = 0f;
            float deltax = 0f;

            //如果内容的高度大于ViewPort的高度，更新位置
            if (content.MCanvasSize().y > viewport.MCanvasSize().y)
            {
                //如果按钮在view上面，Content应该往下移动
                if (btnLeftTop.y > viewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - viewPortLeftTop.y);
                }
                //如果按钮在view下面，Content应该往上移动
                else if (btnRightBottom.y < viewPortRightBottom.y)
                {
                    deltay = viewPortRightBottom.y - btnRightBottom.y;
                }
            }

            if (content.MCanvasSize().x > viewport.MCanvasSize().x)
            {
                //如果按钮在左边，Content应该往右移动
                if (btnLeftTop.x < viewPortLeftTop.x)
                {
                    deltax = viewPortLeftTop.x - btnLeftTop.x;
                }
                //如果按钮在右边，Content应该往左移动
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