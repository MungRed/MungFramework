#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

        public ActionTreeViewer()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ActionTreeEditorConfig.ActionTreeViewerPath+ ".uss");
            styleSheets.Add(styleSheet);
        }

        //右键菜单
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //获取所有实现的ActionNode类
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                //只能创建与行为树同一命名空间的节点
                if (NodeTree.GetType().Namespace == type.Namespace)
                {
                    evt.menu.AppendAction($"节点/{type.Name}", (a) => CreateNode(type, a));
                }
            }
            evt.menu.AppendAction("回到中心", (a) => FrameOrigin());
        }

        //创建节点
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


        private void CreateNodeView(ActionNode node)
        {
            ActionNodeView nodeView = new ActionNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.OnNodeUnSelected = OnNodeUnSelected;
            AddElement(nodeView);
        }


        //更新视图
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


            if (nodeTree.ActionNodeList != null)
            {
                //创建节点视图
                foreach (var node in nodeTree.ActionNodeList)
                {
                    CreateNodeView(node);
                }

                //创建连线
                foreach (var node in nodeTree.ActionNodeList)
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


        //更新数据
        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    ActionNodeView nodeView = element as ActionNodeView;
                    if (nodeView != null)
                    {
                        NodeTree.DeleteNode(nodeView.Node);
                    }

                    Edge edge = element as Edge;
                    if (edge != null)
                    {
                        ActionNodeView startNodeView = edge.output.node as ActionNodeView;
                        ActionNodeView endNodeView = edge.input.node as ActionNodeView;
                        NodeTree.RemoveChild(startNodeView.Node, endNodeView.Node);
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

                    if (edge.output == startNodeView.NextPort)
                    {
                        NodeTree.SetNextChild(startNodeView.Node, endNodeView.Node);
                    }
                    else if (edge.output == startNodeView.AtTimePort)
                    {
                        NodeTree.SetAtTimeChild(startNodeView.Node, endNodeView.Node);
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
            return ports.ToList().Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
        }
    }

}
#endif
