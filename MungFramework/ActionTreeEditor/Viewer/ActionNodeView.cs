#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{
    /// <summary>
    /// 节点视图
    /// </summary>
    public class ActionNodeView : Node
    {
        public ActionNode Node;

        public Port InputPort;
        public Port NextPort, AtTimePort;

        public Action<ActionNodeView> OnNodeSelected;
        public Action<ActionNodeView> OnNodeUnSelected;

        public ActionNodeView(ActionNode node)
        {
            Node = node;

            SetStyle();
            viewDataKey = Node.guid;

            style.left = Node.position.x;
            style.top = Node.position.y;

            //如果不是根节点，创建输入端口
            if (Node.NodeType != ActionNode.NodeTypeEnum.Root)
            {
                CreateInputPort();
            }
            CreateOutputPort();
        }

        private void SetStyle()
        {
            title = Node.NodeTitle;
            titleContainer.style.backgroundColor = Node.颜色;
        }

        private void CreateInputPort()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (InputPort != null)
            {
                InputPort.portName = "";
                inputContainer.Add(InputPort);
            }
        }

        private void CreateOutputPort()
        {
            NextPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

            if (NextPort != null)
            {
                NextPort.portName = "Next";
                outputContainer.Add(NextPort);
            }

            AtTimePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            if (AtTimePort != null)
            {
                AtTimePort.portName = "AtTime";
                outputContainer.Add(AtTimePort);
            }
            outputContainer.style.flexDirection = FlexDirection.Column;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            SetStyle();
            OnNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            SetStyle();
            OnNodeUnSelected?.Invoke(this);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.SetPosition(newPos.position);
        }
    }
}
#endif
