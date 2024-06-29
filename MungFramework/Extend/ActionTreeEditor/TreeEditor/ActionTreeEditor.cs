using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Callbacks;

namespace MungFramework.ActionTreeEditor
{
    public class ActionTreeEditor : EditorWindow
    {
        ActionTreeViewer nodeTreeViewer;
        InspectorView inspectorView;

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

            var nodeTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/MungFramework/Extend/ActionTreeEditor/TreeEditor/ActionTreeEditor.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/MungFramework/Extend/ActionTreeEditor/TreeEditor/ActionTreeEditor.uss");

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
        }
        public void OnNodeUnSelected(ActionNodeView view)
        {
            inspectorView.Clear();
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
        }
    }
}

