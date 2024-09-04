using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using MungFramework.Extension.ComponentExtension;
using MungFramework.Logic.Sound;

namespace MungFramework.Ui
{
    public abstract class UiButtonAbstract : MonoBehaviour
    {
        public enum UiButtonActionType
        {
            Up, Down, Left, Right, OK, Select, UnSelect
        }

        protected RectTransform _rectTransform;
        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        protected UiScrollViewAbstract uiScrollView => GetComponentInParent<UiScrollViewAbstract>();


        [SerializeField]
        protected SerializedDictionary<UiButtonActionType, UnityEvent> uiButtonActionMap = new();

        [HorizontalGroup]
        public bool CouldUp = true, CouldDown = true, CouldLeft = true, CouldRight = true;

        [SerializeField]
        protected bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            protected set => isSelected = value;
        }

        //选中特效
        [SerializeField]
        protected GameObject selectObject;

        [SerializeField]
        protected AudioClip checkAudio;

        #region 事件
        public void AddAction(UiButtonActionType type, UnityAction action)
        {
            if (!uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap.Add(type, new());
            }
            uiButtonActionMap[type].AddListener(action);
        }
        public void RemoveAction(UiButtonActionType type, UnityAction action)
        {
            if (uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap[type].RemoveListener(action);
            }
        }
        public void DoAction(UiButtonActionType type)
        {
            if (uiButtonActionMap.ContainsKey(type))
            {
                uiButtonActionMap[type].Invoke();
            }
        }

        public virtual void Select()
        {
            IsSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (checkAudio != null)
            {
                SoundManagerAbstract.Instance.PlayAudio("effect", checkAudio,replace:true);
            }
            if (selectObject != null)
            {
                selectObject.SetActive(true);
            }
        }

        //选中但不播放音效
        public virtual void SelectWithoutAudio()
        {
            IsSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (selectObject != null)
            {
                selectObject.SetActive(true);
            }
        }

        public virtual void UnSelect()
        {
            IsSelected = false;
            DoAction(UiButtonActionType.UnSelect);
            if (selectObject != null)
            {
                selectObject.SetActive(false);
            }
        }

        public virtual void OK()
        {
            DoAction(UiButtonActionType.OK);
        }

        public virtual void Up()
        {
            DoAction(UiButtonActionType.Up);
        }
        public virtual void Down()
        {
            DoAction(UiButtonActionType.Down);
        }
        public virtual void Left()
        {
            DoAction(UiButtonActionType.Left);
        }
        public virtual void Right()
        {
            DoAction(UiButtonActionType.Right);
        }
        #endregion


        protected void UpdateScrollView()
        {
            if (uiScrollView != null)
            {
                uiScrollView.UpdatePosition(this);
            }
        }

        [ShowInInspector]
        public Vector2 AnchoredPosition => rectTransform.MAnchoredPosition();
        [ShowInInspector]
        public Vector2 Size => rectTransform.MRectSize();
        [ShowInInspector]
        public Vector2 Pivot => rectTransform.MPivot();
        [ShowInInspector]
        public Vector2 LeftTop => rectTransform.MLeftTop();
        [ShowInInspector]
        public Vector2 RightBottom => rectTransform.MRightBottom();


    }

}