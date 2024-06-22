using UnityEngine;

namespace MungFramework.ComponentExtend
{
    public static class RectTransformExtend
    {
        public static Vector2 MAnchoredPosition(this RectTransform rectTransform)=> rectTransform.anchoredPosition;

        public static Vector2 MRectSize(this RectTransform rectTransform)=> rectTransform.rect.size;

        public static Vector2 MPivot(this RectTransform rectTransform)=> rectTransform.pivot;

        public static Vector2 MLeftTop(this RectTransform rectTransform)=> new Vector2(rectTransform.MLeft(), rectTransform.MTop());

        public static Vector2 MRightBottom(this RectTransform rectTransform)=> new Vector2(rectTransform.MRight(), rectTransform.MBottom());

        public static Vector2 MLeftBottom(this RectTransform rectTransform)=> new Vector2(rectTransform.MLeft(), rectTransform.MBottom());

        public static Vector2 MRightTop(this RectTransform rectTransform)=> new Vector2(rectTransform.MRight(), rectTransform.MTop());

        public static float MLeft(this RectTransform rectTransform)
        {
            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.x -= size.x * pivot.x;
            return pos.x;
        }
        public static float MRight(this RectTransform rectTransform)
        {

            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.x += size.x * (1 - pivot.x);
            return pos.x;
        }
        public static float MTop(this RectTransform rectTransform)
        {

            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.y += size.y * (1 - pivot.y);
            return pos.y;
        }
        public static float MBottom(this RectTransform rectTransform)
        {
            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.y -= size.y * pivot.y;
            return pos.y;
        }
    }
}

