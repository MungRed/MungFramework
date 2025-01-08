using System;
using UnityEngine.Events;

namespace MungFramework.Model.MungActionQueue
{
    [Serializable]
    public class ActionModel_Sync : ActionModelAbstract_Sync
    {
        private UnityAction _action
        {
            get;
        }

        public ActionModel_Sync(UnityAction _action)
        {
            this._action = _action;
        }

        public override void DoActionSync()
        {
            _action?.Invoke();
        }
    }
}
