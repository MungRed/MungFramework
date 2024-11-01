using MungFramework.Extension.ComponentExtension;
using MungFramework.Logic.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// Ui实体的基类，可以缓存一些常用的组件，防止重复获取
    /// </summary>
    public abstract class UiEntityAbstract : MungFramework.Entity.Entity
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
                    canvas = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
                }
                return canvas;
            }
        }
        #endregion

        /// <summary>
        /// 事件列表
        /// 对于Button的事件，在button中触发后会调用button实体的无参和有参事件，同时向上调用layer和layergroup的无参和有参事件
        /// 对于layer的事件，在layer中触发后会调用layer实体的无参和有参事件，同时向上调用layergroup的无参和有参事件
        /// </summary>
        #region 事件列表
        public static readonly string ON_BUTTON_SELECT = "ON_BUTTON_SELECT";
        public static readonly string ON_BUTTON_UNSELECT = "ON_BUTTON_UNSELECT";
        public static readonly string ON_BUTTON_OK = "ON_BUTTON_OK";
        public static readonly string ON_BUTTON_CANCEL = "ON_BUTTON_CANCEL";
        public static readonly string ON_BUTTON_SPECIAL_ACTION = "ON_BUTTON_SPECIAL_ACTION";
        public static readonly string ON_BUTTON_MOUSE_ENTER = "ON_BUTTON_MOUSE_ENTER";
        public static readonly string ON_BUTTON_MOUSE_EXIT = "ON_BUTTON_MOUSE_EXIT";
        public static readonly string ON_BUTTON_UP = "ON_BUTTON_UP";
        public static readonly string ON_BUTTON_DOWN = "ON_BUTTON_DOWN";
        public static readonly string ON_BUTTON_LEFT = "ON_BUTTON_LEFT";
        public static readonly string ON_BUTTON_RIGHT = "ON_BUTTON_RIGHT";

        public static readonly string ON_LAYER_OPEN = "ON_LAYER_OPEN";
        public static readonly string ON_LAYER_CLOSE = "ON_LAYER_CLOSE";
        public static readonly string ON_LAYER_OK = "ON_LAYER_OK";
        public static readonly string ON_LAYER_CANCEL = "ON_LAYER_CANCEL";
        public static readonly string ON_LAYER_SPECIAL_ACTION = "ON_LAYER_SPECIAL_ACTION";
        public static readonly string ON_LAYER_LEFT_PAGE = "ON_LAYER_LEFT_PAGE";
        public static readonly string ON_LAYER_RIGHT_PAGE = "ON_LAYER_RIGHT_PAGE";
        public static readonly string ON_LAYER_UP_ROLL = "ON_LAYER_UP_ROLL";
        public static readonly string ON_LAYER_DOWN_ROLL = "ON_LAYER_DOWN_ROLL";
        public static readonly string ON_LAYER_UP = "ON_LAYER_UP";
        public static readonly string ON_LAYER_DOWN = "ON_LAYER_DOWN";
        public static readonly string ON_LAYER_LEFT = "ON_LAYER_LEFT";
        public static readonly string ON_LAYER_RIGHT = "ON_LAYER_RIGHT";

        public static readonly string ON_LAYERGROUP_CLOSE = "ON_LAYERGROUP_CLOSE";



        public void AddListener_UiButton(string actionName, UnityAction<UiButtonAbstract> listener) => AddListener_Action(actionName, listener);
        public void RemoveListener_UiButton(string actionName, UnityAction<UiButtonAbstract> listener) => RemoveListener_Action(actionName, listener);
        public void CallAction_UiButton(string actionName, UiButtonAbstract uiButton) => CallAction(actionName, uiButton);
        public void AddListener_UiLayer(string actionName, UnityAction<UiLayerAbstract> listener) => AddListener_Action(actionName, listener);
        public void RemoveListener_UiLayer(string actionName, UnityAction<UiLayerAbstract> listener) => RemoveListener_Action(actionName, listener);
        public void CallAction_UiLayer(string actionName, UiLayerAbstract uiLayer) => CallAction(actionName, uiLayer);
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
