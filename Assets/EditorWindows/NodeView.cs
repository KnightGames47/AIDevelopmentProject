using UnityEngine;
using BTree;
using System;
using UnityEditor.Experimental.GraphView;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public BTree.Node node;
    public Port input;
    public Port output;

    public NodeView(BTree.Node node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void CreateInputPorts()
    {
        if (node is Node_Action)
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if (node is Node_Composite)
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        else if (node is Node_Decorator)
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if (node is Node_RootNode) { }

        if (input != null)
        {
            input.portName = "in";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is Node_Action) { }
        else if (node is Node_Composite)
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        else if (node is Node_Decorator)
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        else if (node is Node_RootNode)
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

        if (output != null)
        {
            output.portName = "out";
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }
}
