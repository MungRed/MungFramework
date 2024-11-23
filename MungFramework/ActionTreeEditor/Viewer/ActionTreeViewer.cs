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
        /// �Ҽ��˵�
        /// </summary>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //��ȡ����ʵ�ֵ�ActionNode��
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                //�ų�������
                if (type.IsAbstract)
                {
                    continue;
                }

                //ֻ�ܴ�������Ϊ��ͬһ�����ռ�Ľڵ�
                if (NodeTree.GetType().Namespace == type.Namespace)
                {
                    string menuName = type.GetProperty("MenuName").GetValue(null) as string;

                    evt.menu.AppendAction($"�ڵ�/{menuName}", (a) => CreateNode(type, a));
                }
            }
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("�ص�����", (a) => ReturnCenter());
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
        /// �����ڵ�
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
        /// �����ڵ���ͼ
        /// </summary>
        private void CreateNodeView(ActionNode node)
        {
            ActionNodeView nodeView = new ActionNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.OnNodeUnSelected = OnNodeUnSelected;
            AddElement(nodeView);
        }


        /// <summary>
        /// ������Ϊ����ͼ
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
                //�����ڵ���ͼ
                foreach (var node in nodeTree.actionNodeList)
                {
                    CreateNodeView(node);
                }

                //��������
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
        /// ��������
        /// </summary>
        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            //���ĳ��Ԫ�ر��Ƴ�
            if (change.elementsToRemove != null)
            {
                //�������Ƴ���Ԫ��
                foreach (var element in change.elementsToRemove)
                {
                    //�����ĳ���ڵ㱻�Ƴ���
                    if (element is ActionNodeView nodeView)
                    {
                        //ɾ���ڵ�
                        NodeTree.DeleteNode(nodeView.Node);
                    }

                    //�����ĳ���߱��Ƴ���
                    if (element is Edge edge)
                    {
                        //�ߵĸ��ڵ�
                        ActionNodeView startNodeView = edge.output.node as ActionNodeView;
                        //�ߵ��ӽڵ�
                        ActionNodeView endNodeView = edge.input.node as ActionNodeView;

                        //����Ӧ���ӽڵ��ÿ�
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

            //���һ���߱�����
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
