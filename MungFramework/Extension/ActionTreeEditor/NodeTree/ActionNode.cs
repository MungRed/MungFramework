using UnityEngine;


namespace MungFramework.ActionTreeEditor
{
    public abstract class ActionNode : ScriptableObject
    {
        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Vector2 position;
        [HideInInspector]
        public ActionNode Next;
        [HideInInspector]
        public ActionNode AtTime;



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
        public ActionNode GetNextNode()
        {
            return Next;
        }
        public ActionNode GetAtTimeNode()
        {
            return AtTime;
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
#endif



    }
}



