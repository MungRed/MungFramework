using System;
using System.Collections;

namespace MungFramework.Model.MungActionQueue
{
    [Serializable]
    public class ActionModel_Enumerator : ActionModelAbstract_Enumerator
    {
        private IEnumerator _enumerator
        {
            get;
        }

        public ActionModel_Enumerator(IEnumerator _enumerator)
        {
            this._enumerator = _enumerator;
        }

        public override IEnumerator DoActionEnumerator()
        {
            yield return _enumerator;
        }
    }
}
