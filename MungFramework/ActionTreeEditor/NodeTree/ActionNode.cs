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

        public Color ��ɫ = new Color(0.227f, 0.227f, 0.227f, 1.000f);
        public string ��ע = "";

        //�ڵ�����
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
        /// �ú����ֵܶ��������߼���ȡ��һ���ڵ㣬�߼�Ϊ��    
        /// 1.���Next�ڵ�Ϊnull������null
        /// 2.���Next�ڵ����㣬����Next
        /// 3.�ݹ�Next�ڵ��AtTime�ڵ㣬���ص�һ�������AtTime�ڵ�
        /// 4.���û�������AtTime�ڵ㣬����null
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
        //����Դ��������Ϊid
        [Button("����Դ��������Ϊid", ButtonSizes.Medium)]
        public override void SetAssetName()
        {
            name = Id;
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}



