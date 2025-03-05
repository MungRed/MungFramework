using MungFramework.Extension.ImageExtension;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Tool
{
    public class ImageSetTool : MonoBehaviour
    {
        [Button]
        public void SetImage(Sprite sprite)
        {
            var image = GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.SetSpriteWithPivot(sprite);
            }
        }
    }
}
