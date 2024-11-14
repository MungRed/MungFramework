using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Ui
{
    public class UiButtonActionButton : MungFramework.Ui.UiButtonAbstract
    {
        [SerializeField]
        private TMP_Text unSelectText;
        [SerializeField]
        private TMP_Text selectText;

        public void InitButton(string text, UnityAction okAction)
        {
            unSelectText.text = selectText.text = text;
            AddListener_Action(ON_BUTTON_OK, okAction);
        }
        public override void OnSelect(bool playAudio = true)
        {
            base.OnSelect(playAudio);
            unSelectText.gameObject.SetActive(false);
        }
        public override void OnUnSelect()
        {
            base.OnUnSelect();
            unSelectText.gameObject.SetActive(true);
        }
    }
}
