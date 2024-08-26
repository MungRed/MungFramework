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
    /// �ڵ�����ͼ
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

        //�Ҽ��˵�
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //��ȡ����ʵ�ֵ�ActionNode��
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                //ֻ�ܴ�������Ϊ��ͬһ�����ռ�Ľڵ�
                if (NodeTree.GetType().Namespace == type.Namespace)
                {
                    evt.menu.AppendAction($"�ڵ�/{type.Name}", (a) => CreateNode(type, a));
                }
            }
            evt.menu.AppendAction("�ص�����", (a) => FrameOrigin());
        }

        //�����ڵ�
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


        //������ͼ
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
                //�����ڵ���ͼ
                foreach (var node in nodeTree.ActionNodeList)
                {
                    CreateNodeView(node);
                }

                //��������
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


        //��������
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

            //���һ���߱�����
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
