using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MungFramework.Tool
{
    public class ImageColorTool : MonoBehaviour
    {
        private Color targetColor;

        [ShowInInspector]
        public Color TargetColor
        {
            get => targetColor;
            set
            {
                targetColor = value;
                foreach (var image in GetComponentsInChildren<Image>())
                {
                    image.color = targetColor;
                }
            }
        }
    }
}
