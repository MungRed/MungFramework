#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{

    /// <summary>
    /// 属性面板
    /// </summary>
    public class InspectorView : VisualElement
    {
        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
        {

        }

        private Editor editor;

        //当选择某个节点时,更新InspectorView
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

