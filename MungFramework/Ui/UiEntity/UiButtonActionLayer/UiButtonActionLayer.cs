using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    /// <summary>
    /// 功能：在点击某个按钮时弹出，选择要执行的某个操作
    /// </summary>
    public class UiButtonActionLayer : MungFramework.Ui.UiLayerAbstract
    {
        [SerializeField]
        private RectTransform buttonRoot;
        [SerializeField]
        private GameObject actionButtonPrefab;

        [SerializeField]
        private UiLayerAbstract rootLayer;
        [SerializeField]
        private UiButtonAbstract rootButton;
        [SerializeField]
        private List<UiButtonActionButton> actionButtonList = new();
        [SerializeField]
        private float k,b;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (rootButton != null)
            {
                transform.position = Vector3.Lerp(transform.position, rootButton.transform.position, MungFramework.StaticData.FixedDeltaTimeLerpValue_20f);
            }
        }

        public void OpenActionLayer(UiButtonAbstract rootButton,UiLayerAbstract rootLayer, List<(string name,UnityAction action)> actionList)
        {
            this.rootButton = rootButton;
            this.rootLayer = rootLayer;
            transform.position = rootButton.transform.position;

            if (actionList.Count > 0)
            {
                CreateButton(actionList);
                Open();
            }
            else
            {
                Debug.Log("当前没有可操作的");
            }
        }
        protected override void OK(bool useMouse)
        {
            base.OK(useMouse);
            Close();
        }
        public override void Close()
        {
            base.Close();
            ClearButton();
        }
        public override void Left()
        {
            Close();
            rootLayer?.Left();
        }
        public override void Right()
        {
            Close();
            rootLayer?.Right();
        }
        public override void LeftPage()
        {
            Close();
            rootLayer?.LeftPage();
        }
        public override void RightPage()
        {
            Close();
            rootLayer?.RightPage();
        } 

        private void CreateButton(List<(string name, UnityAction action)> actionList)
        {
            void createButtonHelp(string text, UnityAction okAction)
            {
                var actionButton = Instantiate(actionButtonPrefab, buttonRoot).GetComponent<UiButtonActionButton>();
                actionButton.InitButton(text, okAction);
                actionButtonList.Add(actionButton);
            }
            foreach (var action in actionList)
            {
                createButtonHelp(action.name, action.action);
            }
            if (actionButtonList.Count > 0)
            {
                actionButtonList.First().CouldUp = false;
                actionButtonList.Last().CouldDown = false;
            }
            buttonRoot.sizeDelta = new Vector2(buttonRoot.sizeDelta.x, actionList.Count * k + b);
        }
        private void ClearButton()
        {
            foreach (var button in actionButtonList)
            {
                Destroy(button.gameObject);
            }
            actionButtonList.Clear();
        }
    }
}
