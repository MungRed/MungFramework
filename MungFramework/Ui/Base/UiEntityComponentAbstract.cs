using MungFramework.Extension.RectTransformExtension;
using MungFramework.Logic.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// Ui实体的组件基类
    /// </summary>
    public abstract class UiEntityComponentAbstract<T> : MungFramework.Entity.EntityComponent<T> where T : UiEntityAbstract
    {
        #region Component
        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        private RectTransform canvas;
        public RectTransform Canvas
        {
            get
            {
                if (canvas == null)
                {
                    canvas = GetComponentInParent<Canvas>(true)?.GetComponent<RectTransform>();
                }
                return canvas;
            }
        }
        #endregion
        #region CanvasPosition
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public Vector2 CanvasPosition => RectTransform.MCanvasPosition(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public Vector2 CanvasSize => RectTransform.MCanvasSize();
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public float CanvasLeft => RectTransform.MCanvasPosition_Left(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public float CanvasRight => RectTransform.MCanvasPosition_Right(Canvas);

        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public float CanvasTop => RectTransform.MCanvasPosition_Top(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public float CanvasBottom => RectTransform.MCanvasPosition_Bottom(Canvas);
        #endregion
        #region ScreenPosition
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public bool MouseIn
        {
            get
            {
                var mousePosition = InputManagerAbstract.Instance.MousePosition;
                return mousePosition.x >= ScreenLeft && mousePosition.x <= ScreenRight && mousePosition.y >= ScreenBottom && mousePosition.y <= ScreenTop;
            }
        }
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public Vector2 ScreenPosition => RectTransform.MScreenPosition();
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public Vector2 ScreenSize => RectTransform.MScreenSize(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public float ScreenLeft => RectTransform.MScreenPosition_Left(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public float ScreenRight => RectTransform.MScreenPosition_Right(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public float ScreenTop => RectTransform.MScreenPosition_Top(Canvas);
        [ShowInInspector]
        [FoldoutGroup("Screen")]
        public float ScreenBottom => RectTransform.MScreenPosition_Bottom(Canvas);
        #endregion
    }
}
