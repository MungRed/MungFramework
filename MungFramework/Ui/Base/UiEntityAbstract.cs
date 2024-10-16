﻿using MungFramework.Extension.ComponentExtension;
using MungFramework.Logic.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Ui
{
    /// <summary>
    /// Ui实体的基类，可以缓存一些常用的组件，防止重复获取
    /// </summary>
    public abstract class UiEntityAbstract : MungFramework.Entity.Entity
    {
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
        /// <summary>
        /// 同一层级的Ui都会有同一个Canvas，因此如果某个RectTransform没有挂载UiEntityAbstract，
        /// 可以通过其他挂载了UiEntityAbstract的RectTransform来获取Canvas
        /// </summary>
        [ShowInInspector]
        [FoldoutGroup("Canvas")]
        public RectTransform Canvas
        {
            get
            {
                if (canvas == null)
                {
                    canvas = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
                }
                return canvas;
            }
        }

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
