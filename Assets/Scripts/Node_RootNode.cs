using UnityEngine;
using BTree;

public class Node_RootNode : Node
{
    [HideInInspector] public Node child;
    protected override NodeState OnEvaluate()
    {
        return child.Evaluate();
    }

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    public override Node Clone()
    {
        Node_RootNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
