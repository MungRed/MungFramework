using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{
    public class ActionTreeEditor : EditorWindow
    {
        private ActionTreeViewer nodeTreeViewer;
        private InspectorView inspectorView;

        [MenuItem("MungFramework/节点编辑器")]
        public static void OpenWindow()
        {
            ActionTreeEditor wnd = GetWindow<ActionTreeEditor>();
            wnd.titleContent = new GUIContent("NodeEditor");
            wnd.OnSelectionChange();
        }

        /// <summary>
        /// 当双击打开ActionNodeTree时打开节点编辑器窗口
        /// </summary>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is ActionNodeTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }


        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var nodeTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ActionTreeEditorConfig.ActionTreeEditorPath+ ".uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ActionTreeEditorConfig.ActionTreeEditorPath + ".uss");

            nodeTree.CloneTree(root);

            root.styleSheets.Add(styleSheet);

            // 将节点树视图添加到节点编辑器中
            nodeTreeViewer = root.Q<ActionTreeViewer>();
            inspectorView = root.Q<InspectorView>();

            nodeTreeViewer.OnNodeSelected = OnNodeSelected;
            nodeTreeViewer.OnNodeUnSelected = OnNodeUnSelected;
        }

        // 当节点被选中时，更新属性面板
        public void OnNodeSelected(ActionNodeView view)
        {
            inspectorView.UpdateSelection(view);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        public void OnNodeUnSelected(ActionNodeView view)
        {
            inspectorView.Clear();
            UnityEditor.AssetDatabase.SaveAssets();
        }



        // 当选择的对象发生变化时，更新节点树视图
        private void OnSelectionChange()
        {
            ActionNodeTree tree = Selection.activeObject as ActionNodeTree;
            if (tree == null)
            {
                return;
            }

            rootVisualElement.Q<Label>("Title").text = tree.name;

            inspectorView.Clear();
            nodeTreeViewer.PopulateView(tree);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}

