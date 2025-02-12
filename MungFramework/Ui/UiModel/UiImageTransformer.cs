using DG.Tweening;
using MungFramework.Extension.ImageExtension;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MungFramework.Ui
{
    /// <summary>
    /// 工具类，用于Ui图片的过渡转换
    /// </summary>
    [Serializable]
    public class UiImageTransformer : Model.Model
    {
        [SerializeField]
        private Image imageBack;
        [SerializeField]
        private Image imageFront;


        public void Open()
        {
            imageBack.gameObject.SetActive(true);
            imageFront.gameObject.SetActive(true);
            imageBack.SetSpriteAndColor(null);
            imageFront.SetSpriteAndColor(null);
        }

        public void Close()
        {
            imageBack.gameObject.SetActive(false);
            imageFront.gameObject.SetActive(false);
            imageBack.SetSpriteAndColor(null);
            imageFront.SetSpriteAndColor(null);
        }

        public void ClearImage()
        {
            imageBack.SetSpriteAndColor(null);
            imageFront.SetSpriteAndColor(null);
        }

        public void ChangeImage(Sprite sprite, float duration)
        {
            if (imageFront.sprite == null)
            {
                imageFront.sprite = sprite;
                imageFront.color = new Color(1, 1, 1, 0.5f);
                imageFront.DOKill();
                imageFront.DOColor(new Color(1, 1, 1, 1), duration);
            }
            else
            {
                if (imageFront.sprite != sprite)
                {
                    imageBack.sprite = imageFront.sprite;
                    imageBack.color = new Color(1, 1, 1, 1);
                    imageBack.DOKill();
                    imageBack.DOColor(new Color(1, 1, 1, 0), duration);

                    imageFront.sprite = sprite;
                    imageFront.color = new Color(0, 0, 0, 0.5f);
                    imageFront.DOKill();
                    imageFront.DOColor(new Color(1, 1, 1, 1), duration);
                }
            }
        }
    }
}
