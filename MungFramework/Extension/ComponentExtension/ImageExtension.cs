using UnityEngine;
using UnityEngine.UI;

namespace MungFramework.Extension.ImageExtension
{
    public static class ImageExtension
    {
        public static bool SetSpriteAndColor(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
            image.color = new Color(1, 1, 1, sprite == null ? 0 : 1);
            return sprite != null;
        }
    }
}
