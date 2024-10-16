using UnityEngine.Events;

namespace MungFramework.Ui
{
    public class UiEventModel : MungFramework.Model.Model
    {
        private UnityEvent<UiLayerAbstract> onLayerOpen = new();
        private UnityEvent<UiLayerAbstract> onLayerClose = new();

        private UnityEvent<UiButtonAbstract> onButtonSelect = new();
        private UnityEvent<UiButtonAbstract> onButtonUnSelect = new();

        private UnityEvent<UiButtonAbstract> onButtonOK = new();
        private UnityEvent<UiButtonAbstract> onButtonSpecialAction = new();


        public void AddListener_OnLayerOpen(UnityAction<UiLayerAbstract> action) => onLayerOpen.AddListener(action);
        public void RemoveListener_OnLayerOpen(UnityAction<UiLayerAbstract> action) => onLayerOpen.RemoveListener(action);
        public void Call_OnLayerOpen(UiLayerAbstract layer) => onLayerOpen.Invoke(layer);

        public void AddListener_OnLayerClose(UnityAction<UiLayerAbstract> action) => onLayerClose.AddListener(action);
        public void RemoveListener_OnLayerClose(UnityAction<UiLayerAbstract> action) => onLayerClose.RemoveListener(action);
        public void Call_OnLayerClose(UiLayerAbstract layer) => onLayerClose.Invoke(layer);

        public void AddListener_OnButtonSelect(UnityAction<UiButtonAbstract> action) => onButtonSelect.AddListener(action);
        public void RemoveListener_OnButtonSelect(UnityAction<UiButtonAbstract> action) => onButtonSelect.RemoveListener(action);
        public void Call_OnButtonSelect(UiButtonAbstract button) => onButtonSelect.Invoke(button);

        public void AddListener_OnButtonUnSelect(UnityAction<UiButtonAbstract> action) => onButtonUnSelect.AddListener(action);
        public void RemoveListener_OnButtonUnSelect(UnityAction<UiButtonAbstract> action) => onButtonUnSelect.RemoveListener(action);
        public void Call_OnButtonUnSelect(UiButtonAbstract button) => onButtonUnSelect.Invoke(button);

        public void AddListener_OnButtonOK(UnityAction<UiButtonAbstract> action) => onButtonOK.AddListener(action);
        public void RemoveListener_OnButtonOK(UnityAction<UiButtonAbstract> action) => onButtonOK.RemoveListener(action);
        public void Call_OnButtonOK(UiButtonAbstract button) => onButtonOK.Invoke(button);

        public void AddListener_OnButtonSpecialAction(UnityAction<UiButtonAbstract> action) => onButtonSpecialAction.AddListener(action);
        public void RemoveListener_OnButtonSpecialAction(UnityAction<UiButtonAbstract> action) => onButtonSpecialAction.RemoveListener(action);
        public void Call_OnButtonSpecialAction(UiButtonAbstract button) => onButtonSpecialAction.Invoke(button);
    }
}
