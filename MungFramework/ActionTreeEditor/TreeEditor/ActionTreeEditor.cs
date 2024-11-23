#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{
    /// <summary>
    /// 行为树编辑器
    /// </summary>
    public class ActionTreeEditor : EditorWindow
    {
        //行为树视图和节点属性视图
        private ActionTreeViewer nodeTreeViewer;
        //private InspectorView inspectorView;


        [MenuItem("Tools/MungFramework/节点编辑器")]
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

            var nodeTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ActionTreeEditorConfig.ActionTreeEditorPath + ".uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ActionTreeEditorConfig.ActionTreeEditorPath + ".uss");

            nodeTree.CloneTree(root);

            root.styleSheets.Add(styleSheet);

            // 将节点树视图添加到节点编辑器中
            nodeTreeViewer = root.Q<ActionTreeViewer>();
            //inspectorView = root.Q<InspectorView>();
            // 查找名为 FixedViewToggle 的 Toggle 控件
            var fixedViewToggle = root.Q<Toggle>("FixedViewToggle");
            if (fixedViewToggle != null)
            {
                nodeTreeViewer.FixedView(false);
                fixedViewToggle.SetValueWithoutNotify(false);
                fixedViewToggle.RegisterValueChangedCallback(evt =>nodeTreeViewer.FixedView(evt.newValue));
            }

            nodeTreeViewer.OnNodeSelected = OnNodeSelected;
            nodeTreeViewer.OnNodeUnSelected = OnNodeUnSelected;
        }

        /// <summary>
        /// 当节点被选中时，更新属性面板
        /// </summary>
        private void OnNodeSelected(ActionNodeView view)
        {
           // inspectorView.UpdateSelection(view);
            Selection.activeObject = view.Node;
            UnityEditor.AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 当节点被取消选中时，清空属性面板
        /// </summary>
        private void OnNodeUnSelected(ActionNodeView view)
        {
            //inspectorView.Clear();
            UnityEditor.AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 当选择的对象发生变化时，更新节点树视图
        /// </summary>
        private void OnSelectionChange()
        {
            if (Selection.activeObject is ActionNodeTree tree)
            {
                rootVisualElement.Q<Label>("Title").text = tree.name;

               // inspectorView.Clear();
                nodeTreeViewer.PopulateView(tree);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif

