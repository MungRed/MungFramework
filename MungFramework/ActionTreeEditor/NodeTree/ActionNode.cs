using MungFramework.ScriptableObjects;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace MungFramework.ActionTreeEditor
{
    public abstract class ActionNode : DataSO
    {
        [SerializeField]
        [ReadOnly]
        internal string guid;

        [SerializeField]
        [ReadOnly]
        internal Vector2 position;

        [SerializeField]
        [ReadOnly]
        internal ActionNode Next;

        [SerializeField]
        [ReadOnly]
        internal ActionNode AtTime;

        public Color 颜色 = new Color(0.227f, 0.227f, 0.227f, 1.000f);
        public string 备注 = "";

        //节点类型
        public enum NodeTypeEnum
        {
            Root, Node
        }

        public abstract NodeTypeEnum NodeType
        {
            get;
        }

        public abstract string NodeTitle
        {
            get;
        }

        public static string MenuName => "null";

        public T GetNextNode<T>() where T : ActionNode
        {
            if (Next is T res)
            {
                return res;
            }
            return null;
        }

        public T GetAtTimeNode<T>() where T : ActionNode
        {
            if (AtTime is T res)
            {
                return res;
            }
            return null;
        }

        /// <summary>
        /// 用孩子兄弟二叉树的逻辑获取下一个节点，逻辑为：    
        /// 1.如果Next节点为null，返回null
        /// 2.如果Next节点满足，返回Next
        /// 3.递归Next节点的AtTime节点，返回第一个满足的AtTime节点
        /// 4.如果没有满足的AtTime节点，返回null
        /// </summary>
        public T GetNextNodeWithMatch<T>(Func<T, bool> match) where T : ActionNode
        {
            T GetNextNodeInAtTime(T nowNode)
            {
                var atTimeNode = nowNode.GetAtTimeNode<T>();
                if (atTimeNode == null)
                {
                    return null;
                }
                if (match(atTimeNode))
                {
                    return atTimeNode;
                }
                else
                {
                    return GetNextNodeInAtTime(atTimeNode);
                }
            }

            var nextNode = GetNextNode<T>();
            if (nextNode == null)
            {
                return null;
            }
            if (match(nextNode))
            {
                return nextNode;
            }
            else
            {
                return GetNextNodeInAtTime(nextNode);
            }
        }

#if UNITY_EDITOR
        public void SetPosition(Vector3 position)
        {
            this.position = position;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        public void SetNextNode(ActionNode node)
        {
            Next = node;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        public void SetAtTimeNode(ActionNode node)
        {
            AtTime = node;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        //将资源名称设置为id
        [Button("将资源名称设置为id", ButtonSizes.Medium)]
        public override void SetAssetName()
        {
            name = Id;
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}



