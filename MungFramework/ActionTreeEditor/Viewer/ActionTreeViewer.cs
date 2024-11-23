#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{
    /// <summary>
    /// 节点树视图
    /// </summary>
    public class ActionTreeViewer : GraphView
    {
        [Obsolete]
        public new class UxmlFactory : UxmlFactory<ActionTreeViewer, UxmlTraits>
        {
        }

        public ActionNodeTree NodeTree;

        public Action<ActionNodeView> OnNodeSelected;
        public Action<ActionNodeView> OnNodeUnSelected;

        private ContentZoomer contentZoomer;
        private ContentDragger contentDragger;
        private SelectionDragger selectionDragger;
        private RectangleSelector rectangleSelector;

        public ActionTreeViewer()
        {
            Insert(0, new GridBackground());

            contentZoomer = new();
            this.AddManipulator(contentZoomer);
            contentDragger = new();
            this.AddManipulator(contentDragger);
            selectionDragger = new();
            this.AddManipulator(selectionDragger);
            rectangleSelector = new();
            this.AddManipulator(rectangleSelector);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ActionTreeEditorConfig.ActionTreeViewerPath + ".uss");
            styleSheets.Add(styleSheet);
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //获取所有实现的ActionNode类
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                //排除抽象类
                if (type.IsAbstract)
                {
                    continue;
                }

                //只能创建与行为树同一命名空间的节点
                if (NodeTree.GetType().Namespace == type.Namespace)
                {
                    string menuName = type.GetProperty("MenuName").GetValue(null) as string;

                    evt.menu.AppendAction($"节点/{menuName}", (a) => CreateNode(type, a));
                }
            }
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("回到中心", (a) => ReturnCenter());
        }
        private void ReturnCenter()
        {
            if (NodeTree?.GetRoot<ActionNode>())
            {
                FrameAll();
            }
            else
            {
                FrameOrigin();
            }
        }
        public void FixedView(bool value)
        {
            if (value)
            {
                if (contentDragger != null)
                {
                    this.RemoveManipulator(contentDragger);
                }
                if (contentZoomer != null)
                {
                    this.RemoveManipulator(contentZoomer);
                }
                if (selectionDragger != null)
                {
                    this.RemoveManipulator(selectionDragger);
                }
            }
            else
            {
                if (contentDragger != null)
                {
                    this.RemoveManipulator(contentDragger);
                }
                if (contentZoomer != null)
                {
                    this.RemoveManipulator(contentZoomer);
                }
                if (selectionDragger != null)
                {
                    this.RemoveManipulator(selectionDragger);
                }
                if (rectangleSelector != null)
                {
                    this.RemoveManipulator(rectangleSelector);
                }

                contentZoomer = new();
                this.AddManipulator(contentZoomer);
                contentDragger = new();
                this.AddManipulator(contentDragger);
                selectionDragger = new();
                this.AddManipulator(selectionDragger);
                rectangleSelector = new();
                this.AddManipulator(rectangleSelector);
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        private void CreateNode(Type type, DropdownMenuAction a)
        {
            var node = NodeTree.CreateNode(type);

            if (node.NodeType != ActionNode.NodeTypeEnum.Root)
            {
                var mousePosition = a.eventInfo.mousePosition;
                var localPosition = contentViewContainer.WorldToLocal(mousePosition);
                node.position = localPosition;
            }

            CreateNodeView(node);
        }

        /// <summary>
        /// 创建节点视图
        /// </summary>
        private void CreateNodeView(ActionNode node)
        {
            ActionNodeView nodeView = new ActionNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.OnNodeUnSelected = OnNodeUnSelected;
            AddElement(nodeView);
        }


        /// <summary>
        /// 更新行为树视图
        /// </summary>
        public void PopulateView(ActionNodeTree nodeTree)
        {
            if (nodeTree == null)
            {
                return;
            }

            NodeTree = nodeTree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (nodeTree.actionNodeList != null)
            {
                //创建节点视图
                foreach (var node in nodeTree.actionNodeList)
                {
                    CreateNodeView(node);
                }

                //创建连线
                foreach (var node in nodeTree.actionNodeList)
                {
                    var nextChild = nodeTree.GetNextChild(node);
                    if (nextChild != null)
                    {
                        ActionNodeView startNodeView = GetNodeView(node);
                        ActionNodeView endNodeView = GetNodeView(nextChild);

                        Edge edge = startNodeView.NextPort.ConnectTo(endNodeView.InputPort);
                        AddElement(edge);
                    }

                    var atTimeChild = nodeTree.GetAtTimeChild(node);
                    if (atTimeChild != null)
                    {
                        ActionNodeView startNodeView = GetNodeView(node);
                        ActionNodeView endNodeView = GetNodeView(atTimeChild);

                        Edge edge = startNodeView.AtTimePort.ConnectTo(endNodeView.InputPort);
                        AddElement(edge);
                    }
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            //如果某个元素被移除
            if (change.elementsToRemove != null)
            {
                //遍历被移除的元素
                foreach (var element in change.elementsToRemove)
                {
                    //如果是某个节点被移除了
                    if (element is ActionNodeView nodeView)
                    {
                        //删除节点
                        NodeTree.DeleteNode(nodeView.Node);
                    }

                    //如果是某条边被移除了
                    if (element is Edge edge)
                    {
                        //边的父节点
                        ActionNodeView startNodeView = edge.output.node as ActionNodeView;
                        //边的子节点
                        ActionNodeView endNodeView = edge.input.node as ActionNodeView;

                        //将相应的子节点置空
                        if (edge.output == startNodeView.AtTimePort)
                        {
                            NodeTree.SetAtTimeChild(startNodeView.Node, null);
                        }
                        else if (edge.output == startNodeView.NextPort)
                        {
                            NodeTree.SetNextChild(startNodeView.Node, null);
                        }
                    }
                }
            }

            //如果一条边被创建
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    ActionNodeView startNodeView = edge.output.node as ActionNodeView;

                    ActionNodeView endNodeView = edge.input.node as ActionNodeView;

                    if (edge.output == startNodeView.AtTimePort)
                    {
                        NodeTree.SetAtTimeChild(startNodeView.Node, endNodeView.Node);
                    }
                    else if (edge.output == startNodeView.NextPort)
                    {
                        NodeTree.SetNextChild(startNodeView.Node, endNodeView.Node);
                    }
                }
            }
            return change;
        }

        private ActionNodeView GetNodeView(ActionNode node)
        {
            return GetNodeByGuid(node.guid) as ActionNodeView;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
        }
    }
}
#endif
