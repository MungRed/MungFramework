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

        [SerializeField]
        protected SerializedDictionary<UiButtonActionType, UnityEvent> UiButtonActionMap = new();

        [HorizontalGroup]
        public bool CouldUp = true, CouldDown = true, CouldLeft = true, CouldRight = true;

        //是否选中
        public bool IsSelected = false;

        //选中特效
        public GameObject SelectObject;

        public UiScrollViewAbstract UiScrollView => GetComponentInParent<UiScrollViewAbstract>();


        [SerializeField]
        protected AudioClip checkAudio;

        #region 事件
        public void AddAction(UiButtonActionType type, UnityAction action)
        {
            if (!UiButtonActionMap.ContainsKey(type))
            {
                UiButtonActionMap.Add(type, new());
            }
            UiButtonActionMap[type].AddListener(action);
        }
        public void RemoveAction(UiButtonActionType type, UnityAction action)
        {
            if (UiButtonActionMap.ContainsKey(type))
            {
                UiButtonActionMap[type].RemoveListener(action);
            }
        }
        public void DoAction(UiButtonActionType type)
        {
            if (UiButtonActionMap.ContainsKey(type))
            {
                UiButtonActionMap[type].Invoke();
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
            if (SelectObject != null)
            {
                SelectObject.SetActive(true);
            }
        }

        //选中但不播放音效
        public virtual void SelectWithoutAudio()
        {
            IsSelected = true;
            UpdateScrollView();
            DoAction(UiButtonActionType.Select);
            if (SelectObject != null)
            {
                SelectObject.SetActive(true);
            }
        }

        public virtual void UnSelect()
        {
            IsSelected = false;
            DoAction(UiButtonActionType.UnSelect);
            if (SelectObject != null)
            {
                SelectObject.SetActive(false);
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