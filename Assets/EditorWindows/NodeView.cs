using UnityEngine;
using BTree;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public BTree.Node node;
    public Port input;
    public Port output;

    public NodeView(BTree.Node node) : base("Assets/EditorWindows/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();

        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    private void SetupClasses()
    {
        if (node is Node_Action)
            AddToClassList("action");
        else if (node is Node_Composite)
            AddToClassList("composite");
        else if (node is Node_Decorator)
            AddToClassList("decorator");
        else if (node is Node_RootNode)
            AddToClassList("root");
    }

    private void CreateInputPorts()
    {
        if (node is Node_Action)
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if (node is Node_Composite)
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
        else if (node is Node_Decorator)
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        else if (node is Node_RootNode) { }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is Node_Action) { }
        else if (node is Node_Composite)
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        else if (node is Node_Decorator)
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        else if (node is Node_RootNode)
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "Behavior Tree (Set Postion)");
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        Node_Composite composite = node as Node_Composite;

        if(composite)
        {
            composite.children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(BTree.Node left, BTree.Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }

    public void UpdateStates()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case BTree.Node.NodeState.RUNNING:
                    if (node.started)
                        AddToClassList("running");
                    break;
                case BTree.Node.NodeState.FAILURE:
                    AddToClassList("failure");
                    break;
                case BTree.Node.NodeState.SUCCESS:
                    AddToClassList("success");
                    break;
            }
        }
    }
}
