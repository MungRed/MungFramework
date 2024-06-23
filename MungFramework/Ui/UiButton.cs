using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using MungFramework.ComponentExtend;

namespace MungFramework.Ui
{
    public class UiButton : MonoBehaviour
    {
        public enum UiButtonActionType
        {
            Up, Down, Left, Right, OK, Select, UnSelect
        }

        [SerializeField]
        protected SerializedDictionary<UiButtonActionType, UnityEvent> UiButtonActions = new();

        [HorizontalGroup]
        public bool CouldUp = true, CouldDown = true, CouldLeft = true, CouldRight = true;

        //是否选中
        public bool isSelected = false;

        //选中特效
        public GameObject SelectObj;

        public UiScrollView @UiScrollView => GetComponentInParent<UiScrollView>();


        #region 事件
        public void AddAction(UiButtonActionType type, UnityAction action)
        {
            if (!UiButtonActions.ContainsKey(type))
            {
                UiButtonActions.Add(type, new());
            }
            UiButtonActions[type].AddListener(action);
        }
        public void RemoveAction(UiButtonActionType type, UnityAction action)
        {
            if (UiButtonActions.ContainsKey(type))
            {
                UiButtonActions[type].RemoveListener(action);
            }
        }
        public void DoAction(UiButtonActionType type)
        {
            if (UiButtonActions.ContainsKey(type))
            {
                UiButtonActions[type].Invoke();
            }
        }

        public virtual void Select()
        {
            isSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (SelectObj != null)
            {
                SelectObj.SetActive(true);
            }
        }

        //选中但不播放音效
        public virtual void SelectWithOutAudio()
        {
            isSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (SelectObj != null)
            {
                SelectObj.SetActive(true);
            }
        }

        public virtual void UnSelect()
        {
            isSelected = false;
            DoAction(UiButtonActionType.UnSelect);
            if (SelectObj != null)
            {
                SelectObj.SetActive(false);
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
            if (UiScrollView != null)
            {
                UiScrollView.UpdatePosition(this);
            }
        }

        [ShowInInspector]
        public Vector2 Position => GetComponent<RectTransform>().MAnchoredPosition();
        [ShowInInspector]
        public Vector2 Size => GetComponent<RectTransform>().MRectSize();
        [ShowInInspector]
        public Vector2 Pivot => GetComponent<RectTransform>().MPivot();
        [ShowInInspector]
        public Vector2 LeftTop => GetComponent<RectTransform>().MLeftTop();
        [ShowInInspector]
        public Vector2 RightBottom => GetComponent<RectTransform>().MRightBottom();

    }

}