using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Model.MungActionQueue
{
    [Serializable]
    public class ActionQueueModel : MungFramework.Model.Model
    {
        [Serializable]
        private class ActionModelNode
        {
            [ShowInInspector]
            public int ActionPriority
            {
                get;
            }

            [ShowInInspector]
            public ActionModelAbstract ActionModel
            {
                get;
            }

            public ActionModelNode(int priority, ActionModelAbstract actionModel)
            {
                ActionPriority = priority;
                ActionModel = actionModel;
            }
        }

        [Serializable]
        private class ActionModelQueue
        {
            [ShowInInspector]
            public int QueuePriority
            {
                get;
            }

            [ShowInInspector]
            private LinkedList<ActionModelNode> actionQueue = new();

            public ActionModelAbstract NowActionModel;


            public ActionModelQueue(int priority)
            {
                QueuePriority = priority;
                NowActionModel = null;
            }

            public void AddAction_First(IEnumerator actionEnumerator, int priority, MonoBehaviour mono)
            {
                AddAction_First(new ActionModel_Enumerator(actionEnumerator), priority, mono);
            }
            public void AddAction_First(UnityAction action, int priority, MonoBehaviour mono)
            {
                AddAction_First(new ActionModel_Sync(action), priority, mono);
            }
            public void AddAction_First(ActionModelAbstract actionModel, int priority, MonoBehaviour mono)
            {
                AddNode_First(new ActionModelNode(priority, actionModel));
            }
            public void AddAction_Last(IEnumerator actionEnumerator, int priority, MonoBehaviour mono)
            {
                AddAction_Last(new ActionModel_Enumerator(actionEnumerator), priority, mono);
            }
            public void AddAction_Last(UnityAction action, int priority, MonoBehaviour mono)
            {
                AddAction_Last(new ActionModel_Sync(action), priority, mono);
            }
            public void AddAction_Last(ActionModelAbstract actionModel, int priority, MonoBehaviour mono)
            {
                AddNode_Last(new ActionModelNode(priority, actionModel));
            }

            public ActionModelAbstract GetAndPopFirst()
            {
                if (actionQueue.Count == 0)
                {
                    return null;
                }
                else
                {
                    ActionModelAbstract actionModel = actionQueue.First.Value.ActionModel;
                    actionQueue.RemoveFirst();
                    return actionModel;
                }
            }

            /// <summary>
            /// 有相同优先级插入到前面
            /// </summary>
            private void AddNode_First(ActionModelNode node)
            {
                var nodeCurrent = actionQueue.First;
                while (nodeCurrent != null)
                {
                    if (nodeCurrent.Value.ActionPriority >= node.ActionPriority)
                    {
                        actionQueue.AddBefore(nodeCurrent, node);
                        return;
                    }
                    nodeCurrent = nodeCurrent.Next;
                }
                actionQueue.AddLast(node);
            }

            /// <summary>
            /// 有相同优先级插入到后面
            /// </summary>
            private void AddNode_Last(ActionModelNode node)
            {
                var nodeCurrent = actionQueue.Last;
                while (nodeCurrent != null)
                {
                    if (nodeCurrent.Value.ActionPriority <= node.ActionPriority)
                    {
                        actionQueue.AddAfter(nodeCurrent, node);
                        return;
                    }
                    nodeCurrent = nodeCurrent.Previous;
                }
                actionQueue.AddFirst(node);
            }
        }


        [ShowInInspector]
        private LinkedList<ActionModelQueue> actionModelQueueList = new();

        /// <summary>
        /// 获得某个优先级的队列，如果没有则创建
        /// </summary>
        private ActionModelQueue GetActionModelQueue(int queuePriority)
        {
            foreach (var actionModelQueue in actionModelQueueList)
            {
                if (actionModelQueue.QueuePriority == queuePriority)
                {
                    return actionModelQueue;
                }
            }

            ActionModelQueue newQueue = new(queuePriority);
            var nodeCurrent = actionModelQueueList.First;
            while (nodeCurrent != null)
            {
                if (nodeCurrent.Value.QueuePriority > queuePriority)
                {
                    actionModelQueueList.AddBefore(nodeCurrent, newQueue);
                    return newQueue;
                }
                nodeCurrent = nodeCurrent.Next;
            }
            actionModelQueueList.AddLast(newQueue);
            return newQueue;
        }

        private void CheckAction(MonoBehaviour mono)
        {
            foreach (var actionModelQueue in actionModelQueueList)
            {
                if (actionModelQueue.NowActionModel != null)
                {
                    return;
                }

                var actionModel = actionModelQueue.GetAndPopFirst();
                if (actionModel != null)
                {
                    if (actionModel is ActionModelAbstract_Sync syncActionModel)
                    {
                        DoActionSync(actionModelQueue, syncActionModel, mono);
                    }
                    else if (actionModel is ActionModelAbstract_Enumerator enumeratorActionAbstract)
                    {
                        mono.StartCoroutine(DoActionEnumerator(actionModelQueue, enumeratorActionAbstract, mono));
                    }
                    return;
                }
            }
        }

        public void Clear()
        {
            actionModelQueueList.Clear();
        }

        private void DoActionSync(ActionModelQueue actionModelQueue, ActionModelAbstract_Sync syncActionModel, MonoBehaviour mono)
        {
            actionModelQueue.NowActionModel = syncActionModel;
            syncActionModel.DoActionSync();
            actionModelQueue.NowActionModel = null;
            CheckAction(mono);
        }

        private IEnumerator DoActionEnumerator(ActionModelQueue actionModelQueue, ActionModelAbstract_Enumerator enumeratorActionAbstract, MonoBehaviour mono)
        {
            actionModelQueue.NowActionModel = enumeratorActionAbstract;
            yield return enumeratorActionAbstract.DoActionEnumerator();
            actionModelQueue.NowActionModel = null;
            CheckAction(mono);
        }

        #region QueueControl        
        public void AddAction_First(IEnumerator actionEnumerator, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_First(actionEnumerator, actionPriority, mono);
            CheckAction(mono);
        }
        public void AddAction_First(UnityAction action, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_First(action, actionPriority, mono);
            CheckAction(mono);
        }
        public void AddAction_First(ActionModelAbstract actionModel, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_First(actionModel, actionPriority, mono);
            CheckAction(mono);
        }


        public void AddAction_Last(IEnumerator actionEnumerator, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_Last(actionEnumerator, actionPriority, mono);
            CheckAction(mono);
        }
        public void AddAction_Last(UnityAction action, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_Last(action, actionPriority, mono);
            CheckAction(mono);
        }
        public void AddAction_Last(ActionModelAbstract actionModel, int queuePriority, int actionPriority, MonoBehaviour mono)
        {
            GetActionModelQueue(queuePriority).AddAction_Last(actionModel, actionPriority, mono);
            CheckAction(mono);
        }
        #endregion
    }
}
