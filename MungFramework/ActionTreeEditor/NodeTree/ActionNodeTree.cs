using MungFramework.ScriptableObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.ActionTreeEditor
{
    public abstract class ActionNodeTree : DataSO
    {
        [SerializeField]
        [ReadOnly]
        internal List<ActionNode> actionNodeList = new();

        public T GetRoot<T>() where T : ActionNode
        {
            var root = actionNodeList.Find(x => x.NodeType == ActionNode.NodeTypeEnum.Root);
            if (root == null)
            {
                return null;
            }
            if (root is T res)
            {
                return res;
            }
            return null;
        }
        public T GetNodeWithId<T>(string id) where T : ActionNode
        {
            var node = actionNodeList.Find(x => x.Id == id);
            if (node == null)
            {
                return null;
            }
            if (node is T res)
            {
                return res;
            }
            return null;
        }



#if UNITY_EDITOR
        [Button("载入所有节点")]
        public void LoadAllActionNode()
        {
            // 获取当前 ScriptableObject 的路径
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);

            // 加载该路径下的所有资产
            Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);

            // 清空当前的 actionNodeList
            actionNodeList.Clear();

            // 筛选出所有的 ActionNode 并添加到 actionNodeList 中
            foreach (var asset in assets)
            {
                if (asset is ActionNode actionNode)
                {
                    actionNodeList.Add(actionNode);
                }
            }

            // 标记为已修改
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public ActionNode GetNextChild(ActionNode node)
        {
            return node.Next;
        }

        public ActionNode GetAtTimeChild(ActionNode node)
        {
            return node.AtTime;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public ActionNode CreateNode(System.Type type)
        {
            ActionNode node = CreateInstance(type) as ActionNode;

            node.name = type.Name;
            node.guid = System.Guid.NewGuid().ToString();

            actionNodeList.Add(node);

            //保存
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public ActionNode DeleteNode(ActionNode node)
        {
            actionNodeList.Remove(node);

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }

        /// <summary>
        /// 设置Next子节点
        /// </summary>
        public void SetNextChild(ActionNode father, ActionNode child)
        {
            father.SetNextNode(child);
        }

        /// <summary>
        /// 设置AtTime子节点
        /// </summary>
        public void SetAtTimeChild(ActionNode father, ActionNode child)
        {
            father.SetAtTimeNode(child);
        }
#endif
    }

}






