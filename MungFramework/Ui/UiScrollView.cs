using DG.Tweening;
using MungFramework.ComponentExtend;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    public abstract class UiScrollView : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform ViewPort;
        [SerializeField]
        protected RectTransform Content;

        //按钮距离边界的距离
        [SerializeField]
        protected float UpLimit,
            DownLimit,
            LeftLimit,
            RightLimit;


        [ShowInInspector]
        protected Vector2 ViewPortPos => ViewPort == null ? Vector2.zero : ViewPort.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 ViewPortLeftTop => ViewPort == null ? Vector2.zero : ViewPort.MLeftTop();
        [ShowInInspector]
        protected Vector2 ViewPortRightBottom => ViewPort == null ? Vector2.zero : ViewPort.MRightBottom();

        [ShowInInspector]
        protected Vector2 ContentPos => Content == null ? Vector2.zero : Content.MAnchoredPosition();
        [ShowInInspector]
        protected Vector2 ContentLeftTop => Content == null ? Vector2.zero : Content.MLeftTop();
        [ShowInInspector]
        protected Vector2 ContentRightBottom => Content == null ? Vector2.zero : Content.MRightBottom();



        //更新Content的位置
        public virtual void UpdatePosition(UiButton button)
        {
            Vector2 AimPos = Vector2.zero;


            var btnLeftTop = button.LeftTop + ContentPos;//按钮的左上角坐标
            var btnRightBottom = button.RightBottom + ContentPos;//按钮的右下角坐标

            var dViewPortLeftTop = ViewPortLeftTop + new Vector2(LeftLimit, -UpLimit);
            var dViewPortRightBottom = ViewPortRightBottom + new Vector2(-RightLimit, DownLimit);

            float deltay = 0f;
            float deltax = 0f;
            //如果内容的高度大于ViewPort的高度，更新位置
            if (Content.MRectSize().y > ViewPort.MRectSize().y)
            {
                //如果按钮在view上面，Content应该往下移动
                if (btnLeftTop.y > dViewPortLeftTop.y)
                {
                    deltay = -(btnLeftTop.y - dViewPortLeftTop.y + UpLimit);
                }
                //如果按钮在view下面，Content应该往上移动
                else if (btnRightBottom.y < dViewPortRightBottom.y)
                {
                    deltay = dViewPortRightBottom.y - btnRightBottom.y + DownLimit;
                }
            }
            if (Content.MRectSize().x > ViewPort.MRectSize().x)
            {
                //如果按钮在左边，Content应该往右移动
                if (btnLeftTop.x < dViewPortLeftTop.x)
                {
                    deltax = dViewPortLeftTop.x - btnLeftTop.x + LeftLimit;
                }
                //如果按钮在右边，Content应该往左移动
                else if (btnRightBottom.x > dViewPortRightBottom.x)
                {
                    deltax = -(btnRightBottom.x - dViewPortRightBottom.x + RightLimit);
                }
            }
            AimPos = Content.MAnchoredPosition() + new Vector2(deltax, deltay);
            //移动后的Content的左上角坐标和右下角坐标
            Vector2 AimLeftTop = ContentLeftTop + new Vector2(deltax, deltay);
            Vector2 AimRightBottom = ContentRightBottom + new Vector2(deltax, deltay);

            if (Content.MRectSize().y > ViewPort.MRectSize().y)
            {
                if (AimLeftTop.y < ViewPortLeftTop.y)
                {
                    AimPos.y += ViewPortLeftTop.y - AimLeftTop.y;
                }
                else if (AimRightBottom.y > ViewPortRightBottom.y)
                {
                    AimPos.y -= AimRightBottom.y - ViewPortRightBottom.y;
                }
            }
            if (Content.MRectSize().x > ViewPort.MRectSize().x)
            {
                if (AimLeftTop.x > ViewPortLeftTop.x)
                {
                    AimPos.x -= AimLeftTop.x - ViewPortLeftTop.x;
                }
                else if (AimRightBottom.x < ViewPortRightBottom.x)
                {
                    AimPos.x += ViewPortRightBottom.x - AimRightBottom.x;
                }
            }

            Content.DOKill();
            Content.DOAnchorPos(AimPos, 0.15f).SetEase(Ease.OutCirc);
        }

    }
}