using System.Collections;
using UnityEngine.Events;

namespace MungFramework.Model.MungActionQueue
{
    public interface IActionQueueManager
    {
        public ActionQueueModel ActionQueueModel
        {
            get;
        }
        public void AddAction_First(IEnumerator actionEnumerator, int queuePriority, int actionPriority);
        public void AddAction_First(UnityAction action, int queuePriority, int actionPriority);
        public void AddAction_First(ActionModelAbstract actionModel, int queuePriority, int actionPriority);

        public void AddAction_Last(IEnumerator actionEnumerator, int queuePriority, int actionPriority);
        public void AddAction_Last(UnityAction action, int queuePriority, int actionPriority);
        public void AddAction_Last(ActionModelAbstract actionModel, int queuePriority, int actionPriority);
    }
}
