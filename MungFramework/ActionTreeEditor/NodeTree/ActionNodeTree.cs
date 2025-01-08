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
        [Button("�������нڵ�")]
        public void LoadAllActionNode()
        {
            // ��ȡ��ǰ ScriptableObject ��·��
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);

            // ���ظ�·���µ������ʲ�
            Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);

            // ��յ�ǰ�� actionNodeList
            actionNodeList.Clear();

            // ɸѡ�����е� ActionNode ����ӵ� actionNodeList ��
            foreach (var asset in assets)
            {
                if (asset is ActionNode actionNode)
                {
                    actionNodeList.Add(actionNode);
                }
            }

            // ���Ϊ���޸�
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
        /// �����ڵ�
        /// </summary>
        public ActionNode CreateNode(System.Type type)
        {
            ActionNode node = CreateInstance(type) as ActionNode;

            node.name = type.Name;
            node.guid = System.Guid.NewGuid().ToString();

            actionNodeList.Add(node);

            //����
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
            UnityEditor.AssetDatabase.SaveAssets();

            return node;
        }

        /// <summary>
        /// ɾ���ڵ�
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
        /// ����Next�ӽڵ�
        /// </summary>
        public void SetNextChild(ActionNode father, ActionNode child)
        {
            father.SetNextNode(child);
        }

        /// <summary>
        /// ����AtTime�ӽڵ�
        /// </summary>
        public void SetAtTimeChild(ActionNode father, ActionNode child)
        {
            father.SetAtTimeNode(child);
        }
#endif
    }

}






