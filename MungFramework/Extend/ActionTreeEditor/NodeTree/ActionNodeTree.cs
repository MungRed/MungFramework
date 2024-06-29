using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Collections;

namespace MungFramework.ActionTreeEditor
{
    public abstract class ActionNodeTree : ScriptableObject
    {

        [Sirenix.OdinInspector.ReadOnly()]
        public List<ActionNode> ActionNodeList;


#if UNITY_EDITOR

        /// <summary>
        /// 创建节点
        /// </summary>
        public ActionNode CreateNode(System.Type type)
        {
            ActionNode node = CreateInstance(type) as ActionNode;
            node.name = type.Name;

            node.guid = System.Guid.NewGuid().ToString();

            ActionNodeList.Add(node);

            //保存
            UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }
        public ActionNode DeleteNode(ActionNode node)
        {
            ActionNodeList.Remove(node);


            UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }
        public void RemoveChild(ActionNode father, ActionNode child)
        {
            if (GetNextChild(father) == child)
            {
                father.Next = null;
            }

            if (GetAtTimeChild(father) == child)
            {
                father.AtTime = null;
            }
        }

        public void SetNextChild(ActionNode father, ActionNode child)
        {
            father.Next = child;
        }
        public void SetAtTimeChild(ActionNode father, ActionNode child)
        {

            father.AtTime = child;
        }

        public ActionNode GetNextChild(ActionNode node)
        {
            return node.Next;
        }
        public ActionNode GetAtTimeChild(ActionNode node)
        {
            return node.AtTime;
        }
#endif
    }

}






