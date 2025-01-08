using System.Collections;

namespace MungFramework.Model.MungActionQueue
{
    public abstract class ActionModelAbstract_Enumerator : ActionModelAbstract
    {
        public abstract IEnumerator DoActionEnumerator();
    }
}