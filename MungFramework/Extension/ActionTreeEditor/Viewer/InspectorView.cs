#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{

    /// <summary>
    /// �������
    /// </summary>
    public class InspectorView : VisualElement
    {
        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
        {

        }

        private Editor editor;

        //��ѡ��ĳ���ڵ�ʱ,����InspectorView
        public void UpdateSelection(ActionNodeView nodeView)
        {
            Clear();

            if (editor)
            {
                Object.DestroyImmediate(editor);
            }

            editor = Editor.CreateEditor(nodeView.Node);

            IMGUIContainer container = new IMGUIContainer(
                () =>
                {
                    if (editor.target)
                    {
                        editor.OnInspectorGUI();
                    }
                });

            Add(container);
        }
    }

}
#endif

