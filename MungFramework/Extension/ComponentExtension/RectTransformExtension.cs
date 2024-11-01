using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Extension.ComponentExtension
{

    public static class RectTransformExtension
    {
        /// <summary>
        /// 轴心
        /// </summary>
        public static Vector2 MPivot(this RectTransform rectTransform) => rectTransform.pivot;

        #region CanvasPosition
        /// <summary>
        /// 相对于Canvas的坐标，也就是相对于1920*1080的坐标
        /// 无论屏幕分辨率多少，都是相对于1920*1080的坐标
        /// </summary>
        public static Vector2 MCanvasPosition(this RectTransform rectTransform, RectTransform canvas)
        {
            if (canvas == null)
            {
                return rectTransform.position;
            }
            var canvasScale = canvas.localScale;
            var position = rectTransform.position;
            return new Vector3(position.x / canvas.localScale.x, position.y / canvas.localScale.y, position.z / canvas.localScale.z);
        }

        /// <summary>
        /// 通过相对于Canvas的坐标来设置RectTransform的坐标
        /// </summary>
        public static void MCanvasPosition_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            if (canvas == null)
            {
                rectTransform.position = val;
                return;
            }
            var canvasScale = canvas.localScale;
            rectTransform.position = new Vector3(val.x * canvas.localScale.x, val.y * canvas.localScale.y, rectTransform.position.z);
        }

        /// <summary>
        /// 矩形的大小，不随屏幕分辨率改变而改变
        /// </summary>
        public static Vector2 MCanvasSize(this RectTransform rectTransform) => rectTransform.rect.size;

        public static float MCanvasPosition_Left(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MCanvasPosition(canvas);
            var size = rectTransform.MCanvasSize();
            var pivot = rectTransform.MPivot();

            return position.x - size.x * pivot.x;
        }
        public static void MCanvasPosition_Left_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldLeft = rectTransform.MCanvasPosition_Left(canvas);
            var delta = val - oldLeft;
            var position = rectTransform.MCanvasPosition(canvas);

            rectTransform.MCanvasPosition_Set(canvas, new Vector2(position.x + delta, position.y));
        }
        public static float MCanvasPosition_Right(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MCanvasPosition(canvas);
            var size = rectTransform.MCanvasSize();
            var pivot = rectTransform.MPivot();

            return position.x + size.x * (1 - pivot.x);
        }
        public static void MCanvasPosition_Right_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldRight = rectTransform.MCanvasPosition_Right(canvas);
            var delta = val - oldRight;
            var position = rectTransform.MCanvasPosition(canvas);

            rectTransform.MCanvasPosition_Set(canvas, new Vector2(position.x + delta, position.y));
        }
        public static float MCanvasPosition_Top(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MCanvasPosition(canvas);
            var size = rectTransform.MCanvasSize();
            var pivot = rectTransform.MPivot();

            return position.y + size.y * (1 - pivot.y);
        }
        public static void MCanvasPosition_Top_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldTop = rectTransform.MCanvasPosition_Top(canvas);
            var delta = val - oldTop;
            var position = rectTransform.MCanvasPosition(canvas);

            rectTransform.MCanvasPosition_Set(canvas, new Vector2(position.x, position.y + delta));
        }
        public static float MCanvasPosition_Bottom(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MCanvasPosition(canvas);
            var size = rectTransform.MCanvasSize();
            var pivot = rectTransform.MPivot();

            return position.y - size.y * pivot.y;
        }
        public static void MCanvasPosition_Bottom_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldBottom = rectTransform.MCanvasPosition_Bottom(canvas);
            var delta = val - oldBottom;
            var position = rectTransform.MCanvasPosition(canvas);

            rectTransform.MCanvasPosition_Set(canvas, new Vector2(position.x, position.y + delta));
        }

        public static Vector2 MCanvasPosition_LeftTop(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MCanvasPosition_Left(canvas), rectTransform.MCanvasPosition_Top(canvas));
        public static void MCanvasPosition_LeftTop_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MCanvasPosition_Left_Set(canvas, val.x);
            rectTransform.MCanvasPosition_Top_Set(canvas, val.y);
        }
        public static Vector2 MCanvasPosition_LeftButtom(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MCanvasPosition_Left(canvas), rectTransform.MCanvasPosition_Bottom(canvas));
        public static void MCanvasPosition_LeftButtom_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MCanvasPosition_Left_Set(canvas, val.x);
            rectTransform.MCanvasPosition_Bottom_Set(canvas, val.y);
        }
        public static Vector2 MCanvasPosition_RightTop(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MCanvasPosition_Right(canvas), rectTransform.MCanvasPosition_Top(canvas));
        public static void MCanvasPosition_RightTop_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MCanvasPosition_Right_Set(canvas, val.x);
            rectTransform.MCanvasPosition_Top_Set(canvas, val.y);
        }
        public static Vector2 MCanvasPosition_RightBottom(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MCanvasPosition_Right(canvas), rectTransform.MCanvasPosition_Bottom(canvas));
        public static void MCanvasPosition_RightBottom_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MCanvasPosition_Right_Set(canvas, val.x);
            rectTransform.MCanvasPosition_Bottom_Set(canvas, val.y);
        }

        public static Tweener DOCanvasPosition(this RectTransform rectTransform, RectTransform canvas, Vector2 endValue, float duration)
        {
            var aimPosition = new Vector3(endValue.x * canvas.localScale.x, endValue.y * canvas.localScale.y, rectTransform.position.z);
            return rectTransform.DOMove(aimPosition, duration);
        }
        #endregion

        #region ScreenPosition
        public static Vector2 MScreenPosition(this RectTransform rectTransform) => rectTransform.position;
        public static void MScreenPosition_Set(this RectTransform rectTransform, Vector2 val) => rectTransform.position = val;
        public static Vector2 MScreenSize(this RectTransform rectTransform, RectTransform canvas)
        {
            var size = rectTransform.MCanvasSize();
            if (canvas == null)
            {
                return size;
            }
            else
            {
                return new Vector2(size.x * canvas.localScale.x, size.y * canvas.localScale.y);
            }
        }
        public static float MScreenPosition_Left(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MScreenPosition();
            var size = rectTransform.MScreenSize(canvas);
            var pivot = rectTransform.MPivot();

            return position.x - size.x * pivot.x;
        }
        public static void MScreenPosition_Left_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldLeft = rectTransform.MScreenPosition_Left(canvas);
            var delta = val - oldLeft;
            var position = rectTransform.MScreenPosition();

            rectTransform.MScreenPosition_Set(new Vector2(position.x + delta, position.y));
        }
        public static float MScreenPosition_Right(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MScreenPosition();
            var size = rectTransform.MScreenSize(canvas);
            var pivot = rectTransform.MPivot();

            return position.x + size.x * (1 - pivot.x);
        }
        public static void MScreenPosition_Right_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldRight = rectTransform.MScreenPosition_Right(canvas);
            var delta = val - oldRight;
            var position = rectTransform.MScreenPosition();

            rectTransform.MScreenPosition_Set(new Vector2(position.x + delta, position.y));
        }
        public static float MScreenPosition_Top(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MScreenPosition();
            var size = rectTransform.MScreenSize(canvas);
            var pivot = rectTransform.MPivot();

            return position.y + size.y * (1 - pivot.y);
        }
        public static void MScreenPosition_Top_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldTop = rectTransform.MScreenPosition_Top(canvas);
            var delta = val - oldTop;
            var position = rectTransform.MScreenPosition();

            rectTransform.MScreenPosition_Set(new Vector2(position.x, position.y + delta));
        }
        public static float MScreenPosition_Bottom(this RectTransform rectTransform, RectTransform canvas)
        {
            var position = rectTransform.MScreenPosition();
            var size = rectTransform.MScreenSize(canvas);
            var pivot = rectTransform.MPivot();

            return position.y - size.y * pivot.y;
        }
        public static void MScreenPosition_Bottom_Set(this RectTransform rectTransform, RectTransform canvas, float val)
        {
            var oldBottom = rectTransform.MScreenPosition_Bottom(canvas);
            var delta = val - oldBottom;
            var position = rectTransform.MScreenPosition();

            rectTransform.MScreenPosition_Set(new Vector2(position.x, position.y + delta));
        }
        public static Vector2 MScreenPosition_LeftTop(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MScreenPosition_Left(canvas), rectTransform.MScreenPosition_Top(canvas));
        public static void MScreenPosition_LeftTop_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MScreenPosition_Left_Set(canvas, val.x);
            rectTransform.MScreenPosition_Top_Set(canvas, val.y);
        }
        public static Vector2 MScreenPosition_LeftButtom(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MScreenPosition_Left(canvas), rectTransform.MScreenPosition_Bottom(canvas));
        public static void MScreenPosition_LeftButtom_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MScreenPosition_Left_Set(canvas, val.x);
            rectTransform.MScreenPosition_Bottom_Set(canvas, val.y);
        }
        public static Vector2 MScreenPosition_RightTop(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MScreenPosition_Right(canvas), rectTransform.MScreenPosition_Top(canvas));
        public static void MScreenPosition_RightTop_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MScreenPosition_Right_Set(canvas, val.x);
            rectTransform.MScreenPosition_Top_Set(canvas, val.y);
        }
        public static Vector2 MScreenPosition_RightButtom(this RectTransform rectTransform, RectTransform canvas) => new Vector2(rectTransform.MScreenPosition_Right(canvas), rectTransform.MScreenPosition_Bottom(canvas));
        public static void MScreenPosition_RightButtom_Set(this RectTransform rectTransform, RectTransform canvas, Vector2 val)
        {
            rectTransform.MScreenPosition_Right_Set(canvas, val.x);
            rectTransform.MScreenPosition_Bottom_Set(canvas, val.y);
        }
        public static Tweener DOScreenPosition(this RectTransform rectTransform, Vector2 endValue, float duration)
        {
            return rectTransform.DOMove(endValue, duration);
        }
        #endregion

        public static void LerpRectTransform(this RectTransform rectTransform, RectTransform target, float t)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, target.position, t);
            rectTransform.sizeDelta = Vector3.Lerp(rectTransform.sizeDelta, target.sizeDelta, t);
        }

        public static List<GameObject> GetChildExcludeSelf(this RectTransform trans)
        {
            if (trans == null)
            {
                return new();
            }
            return trans.GetComponentsInChildren<RectTransform>().Where(x => x != trans).Select(x => x.gameObject).ToList();
        }
    }
}

