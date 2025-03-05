using UnityEngine;
using UnityEngine.UI;

namespace MungFramework.Extension.ImageExtension
{
    public static class ImageExtension
    {
        /// <summary>
        /// 设置Image的sprite，如果sprite为空，将Image设置为透明
        /// </summary>
        public static bool SetSpriteAndColor(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.color = new Color(1, 1, 1, sprite == null ? 0 : 1);
            return sprite != null;
        }

        /// <summary>
        /// 设置Image的sprite，会根据sprite的pivot设置Image的pivot
        /// </summary>
        public static void SetSpriteWithPivot(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.rectTransform.pivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            image.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
