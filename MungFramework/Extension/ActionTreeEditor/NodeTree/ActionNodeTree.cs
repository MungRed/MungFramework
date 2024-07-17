using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.ActionTreeEditor
{

    public abstract class ActionNodeTree : ScriptableObject
    {
        [ReadOnly]
        public List<ActionNode> ActionNodeList;

#if UNITY_EDITOR
        public ActionNode GetNextChild(ActionNode node)
        {
            return node.GetNextNode();
        }
        public ActionNode GetAtTimeChild(ActionNode node)
        {
            return node.GetAtTimeNode();
        }

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
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }
        public ActionNode DeleteNode(ActionNode node)
        {
            ActionNodeList.Remove(node);

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }


        public void RemoveChild(ActionNode father, ActionNode child)
        {
            if (GetNextChild(father) == child)
            {
                father.SetNextNode(null);
            }

            if (GetAtTimeChild(father) == child)
            {
                father.SetAtTimeNode(null);
            }
        }

        public void SetNextChild(ActionNode father, ActionNode child)
        {
            father.SetNextNode(child);
        }
        public void SetAtTimeChild(ActionNode father, ActionNode child)
        {
            father.SetAtTimeNode(child);
        }
#endif


    }

}






