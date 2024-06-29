using UnityEngine;


namespace MungFramework.ActionTreeEditor
{
    public abstract class ActionNode : ScriptableObject
    {
        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Vector2 position;

        public Color ��ɫ = new Color(0.227f, 0.227f, 0.227f, 1.000f);
        public string ��ע = "";

        //�ڵ�����
        public enum NodeType
        {
            Root,Node
        }
        public abstract NodeType _NodeType
        {
            get;
        }
        public abstract string NodeTitle
        {
            get;
        }

        [HideInInspector]
        public ActionNode Next;
        [HideInInspector]
        public ActionNode AtTime;
    }
}



