using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MungFramework.ActionTreeEditor
{
    /// <summary>
    /// �ڵ���ͼ
    /// </summary>
    public class ActionNodeView : Node
    {
        public ActionNode _Node;

        public Port InputPort;

        public Port NextPort, AtTimePort;

        public Action<ActionNodeView> OnNodeSelected;

        public Action<ActionNodeView> OnNodeUnSelected;

        private void SetStyle()
        {
            title = _Node.NodeTitle;
            titleContainer.style.backgroundColor = _Node.��ɫ;
        }
        public ActionNodeView(ActionNode node)
        {
            _Node = node;

            SetStyle();

            viewDataKey = _Node.guid;

            style.left = _Node.position.x;
            style.top = _Node.position.y;

            //������Ǹ��ڵ㣬��������˿�
            if (_Node._NodeType != ActionNode.NodeType.Root)
            {
                CreateInputPort();
            }
            CreateOutputPort();
        }
        public void CreateInputPort()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            if (InputPort != null)
            {
                InputPort.portName = "";
                inputContainer.Add(InputPort);
            }
        }

        public void CreateOutputPort()
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
            _Node.position = newPos.position;
        }
    }

}
