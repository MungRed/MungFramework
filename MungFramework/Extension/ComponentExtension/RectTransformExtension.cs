using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Extension.ComponentExtension
{
    public static class RectTransformExtension
    {

        public static Vector2 MPosition(this RectTransform rectTransform)
        {
            var canvas = rectTransform.GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
            if (canvas == null)
            {
                return rectTransform.position;
            }

            var canvasScale = canvas.localScale;
            var position = rectTransform.position;
            return new Vector3(position.x / canvas.localScale.x, position.y / canvas.localScale.y, position.z / canvas.localScale.z);
        }

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
        public static void MLeft_Set(this RectTransform rectTransform, float val)
        {
            var oldLeft = rectTransform.MLeft();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + val - oldLeft, rectTransform.anchoredPosition.y);
        }
        public static float MRight(this RectTransform rectTransform)
        {

            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.x += size.x * (1 - pivot.x);
            return pos.x;
        }
        public static void MRight_Set(this RectTransform rectTrasnform, float val)
        {
            var oldRight = rectTrasnform.MRight();
            rectTrasnform.anchoredPosition = new Vector2(rectTrasnform.anchoredPosition.x + val - oldRight, rectTrasnform.anchoredPosition.y);
        }
        public static float MTop(this RectTransform rectTransform)
        {

            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.y += size.y * (1 - pivot.y);
            return pos.y;
        }
        public static void MTop_Set(this RectTransform rectTransform, float val)
        {
            var oldTop = rectTransform.MTop();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + val - oldTop);
        }
        public static float MBottom(this RectTransform rectTransform)
        {
            var pos = rectTransform.MAnchoredPosition();
            var size = rectTransform.MRectSize();
            var pivot = rectTransform.MPivot();

            pos.y -= size.y * pivot.y;
            return pos.y;
        }
        public static void MBottom_Set(this RectTransform rectTransform, float val)
        {
            var oldBottom = rectTransform.MBottom();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + val - oldBottom);
        }

        public static List<GameObject> GetChildExcludeSelf(this RectTransform trans)
        {
            return trans.GetComponentsInChildren<RectTransform>().Where(x => x != trans).Select(x => x.gameObject).ToList();
        }
    }
}

