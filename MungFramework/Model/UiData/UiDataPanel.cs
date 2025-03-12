using MungFramework.Extension.ImageExtension;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MungFramework.Model.UiData
{
    [Serializable]
    public class UiDataPanel<T_Enum> : MungFramework.Model.Model where T_Enum : Enum
    {
        [Serializable]
        public class UiDataPanelItem
        {
            [SerializeField]
            private T_Enum key;

            [SerializeField]
            [LabelText("文本框")]
            private TMP_Text text;
            [SerializeField]
            [LabelText("前缀")]
            private string prefix;
            [SerializeField]
            [LabelText("后缀")]
            private string suffix;

            [SerializeField]
            [LabelText("使用中心点")]
            private bool usePivot;
            [SerializeField]
            [LabelText("图片")]
            private Image image;

            public T_Enum Key => key;
            public TMP_Text Text => text;
            public string Prefix => prefix;
            public string Suffix => suffix;
            public bool UsePivot => usePivot;
            public Image Image => image;
        }

        [SerializeField]
        private List<UiDataPanelItem> uiDataPanelItemList;

        public void LoadUiData(IUiDataService<T_Enum> uiDataService)
        {
            if(uiDataService == null)
            {
                Clear();
                return;
            }
            foreach (var panelItem in uiDataPanelItemList)
            {
                if (panelItem.Text != null)
                {
                    panelItem.Text.text = panelItem.Prefix + uiDataService.GetTextData(panelItem.Key) + panelItem.Suffix;
                }

                if (panelItem.Image != null)
                {
                    var getSprite = uiDataService.GetSpriteData(panelItem.Key);
                    panelItem.Image.gameObject.SetActive(getSprite != null);
                    if (getSprite != null)
                    {
                        if (panelItem.UsePivot)
                        {
                            panelItem.Image.SetSpriteWithPivot(getSprite);
                        }
                        else
                        {
                            panelItem.Image.sprite = getSprite;
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (var panelItem in uiDataPanelItemList)
            {
                if (panelItem.Text != null)
                {
                    panelItem.Text.text = string.Empty;
                }
                if (panelItem.Image != null)
                {
                    panelItem.Image.gameObject.SetActive(false);
                }
            }
        }
    }

    [Serializable]
    public class UiDataPanel<T_Enum, T_Parameter> : MungFramework.Model.Model where T_Enum : Enum
    {
        [Serializable]
        public class UiDataPanelItem
        {
            [SerializeField]
            private T_Enum key;
            [SerializeField]
            [LabelText("文本框")]
            private TMP_Text text;
            [SerializeField]
            [LabelText("前缀")]
            private string prefix;
            [SerializeField]
            [LabelText("后缀")]
            private string suffix;
            [SerializeField]
            [LabelText("使用中心点")]
            private bool usePivot;
            [SerializeField]
            [LabelText("图片")]
            private Image image;

            public T_Enum Key => key;
            public TMP_Text Text => text;
            public string Prefix => prefix;
            public string Suffix => suffix;
            public bool UsePivot => usePivot;
            public Image Image => image;
        }

        [SerializeField]
        private List<UiDataPanelItem> uiDataPanelItemList;

        public void LoadUiData(IUiDataService<T_Enum, T_Parameter> uiDataService, T_Parameter parameter)
        {
            foreach (var panelItem in uiDataPanelItemList)
            {
                if (panelItem.Text != null)
                {
                    panelItem.Text.text = panelItem.Prefix + uiDataService.GetTextData(panelItem.Key, parameter) + panelItem.Suffix;
                    //panelItem.Text.text = uiDataService.GetTextData(panelItem.Key,parameter);
                }

                if (panelItem.Image != null)
                {
                    var getSprite = uiDataService.GetSpriteData(panelItem.Key, parameter);
                    panelItem.Image.gameObject.SetActive(getSprite != null);
                    if (getSprite != null)
                    {
                        if (panelItem.UsePivot)
                        {
                            panelItem.Image.SetSpriteWithPivot(getSprite);
                        }
                        else
                        {
                            panelItem.Image.sprite = getSprite;
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (var panelItem in uiDataPanelItemList)
            {
                if (panelItem.Text != null)
                {
                    panelItem.Text.text = string.Empty;
                }
                if (panelItem.Image != null)
                {
                    panelItem.Image.gameObject.SetActive(false);
                }
            }
        }
    }
}
