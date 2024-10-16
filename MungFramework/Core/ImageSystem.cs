using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Core
{
    public static class ImageSystem
    {
        public static  string ImageSavePath => Database.DatabasePath + "/image";
        public static string ImageFormat => "jpg";


        #region 屏幕截图
        public static IEnumerator ScreenShot(UnityAction<Texture2D> textureResult,int width = 400)
        {
            yield return new WaitForEndOfFrame();

            var texture = ScreenCapture.CaptureScreenshotAsTexture();

            float quality = width/(float)texture.width;

            // 缩放图片到指定宽
            int newWidth = (int)(texture.width*quality);
            int newHeight = (int)(texture.height*quality);

            Texture2D resizedImage = ResizeTexture(texture, newWidth, newHeight);
            textureResult.Invoke(resizedImage);
        }
        public static IEnumerator ScreenShotAndSave(UnityAction<Texture2D> textureResult, int width, string imageName)
        {
            yield return new WaitForEndOfFrame();

            var texture = ScreenCapture.CaptureScreenshotAsTexture();

            float quality =  width/(float)texture.width ;

            // 缩放图片到指定宽
            int newWidth = (int)(texture.width * quality);
            int newHeight = (int)(texture.height * quality);

            Texture2D resizedImage = ResizeTexture(texture, newWidth, newHeight);
            yield return SaveImageAsync(imageName, resizedImage);
            textureResult.Invoke(resizedImage);
        }
        private static Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
        {
            Texture2D result = new Texture2D(newWidth, newHeight, source.format, false);
            float scaleX = 1.0f / newWidth;
            float scaleY = 1.0f / newHeight;

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    float u = x * scaleX;
                    float v = y * scaleY;
                    result.SetPixel(x, y, source.GetPixelBilinear(u, v));
                }
            }
            result.Apply();
            return result;
        }

        public static Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        #endregion


        #region 图片存储
        /// <summary>
        /// 储存一张图片
        /// </summary>
        public static void SaveImage(string imageName, Texture2D image)
        {
            if (image == null)
            {
                return;
            }
            if (!Directory.Exists(ImageSavePath))
            {
                Directory.CreateDirectory(ImageSavePath);
            }
            byte[] bytes = image.EncodeToJPG();
            FileSystem.WriteAllBytes(ImageSavePath, imageName, ImageFormat, bytes);
        }
        /// <summary>
        /// 异步储存一张图片
        /// </summary>
        public static IEnumerator SaveImageAsync(string imageName, Texture2D image)
        {
            if (image == null)
            {
                yield break;
            }
            if (!Directory.Exists(ImageSavePath))
            {
                Directory.CreateDirectory(ImageSavePath);
            }
            byte[] bytes = image.EncodeToJPG();
            yield return FileSystem.WriteAllBytesAsync(ImageSavePath, imageName, ImageFormat, bytes);
        }

        /// <summary>
        /// 以Sprite形式获取图片
        /// </summary>
        public static Sprite GetImage(string imageName)
        {
            if (!FileSystem.HaveDirectory(ImageSavePath) || !FileSystem.HaveFile(ImageSavePath, imageName, ImageFormat))
            {
                return null;
            }

            byte[] bytes = FileSystem.ReadAllBytes(ImageSavePath, imageName, ImageFormat);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// 异步以Sprite形式获取图片
        /// </summary>
        public static IEnumerator GetImageAsync(string imageName, UnityAction<Sprite> result)
        {
            if (!FileSystem.HaveDirectory(ImageSavePath) || !FileSystem.HaveFile(ImageSavePath, imageName, ImageFormat))
            {
                result.Invoke(null);
                yield break;
            }
            byte[] bytes = null;
            yield return FileSystem.ReadAllBytesAsync(ImageSavePath, imageName, ImageFormat, (b) => { bytes = b; });
            if (bytes == null)
            {
                result.Invoke(null);
                yield break;
            }
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            result(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
        }
        #endregion
    }
}
