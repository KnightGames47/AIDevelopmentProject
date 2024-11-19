using UnityEngine;
using BTree;
using Unity.PlasticSCM.Editor.WebApi;

public class Node_Sequencer : Node_Composite
{
    int current;

    protected override NodeState OnEvaluate()
    {
        var child = children[current];
        switch (child.Evaluate())
        {
            case NodeState.RUNNING:
                return NodeState.RUNNING;
            case NodeState.FAILURE:
                return NodeState.FAILURE;
            case NodeState.SUCCESS:
                current++;
                break;
        }

        return current == children.Count ? NodeState.SUCCESS : NodeState.RUNNING;
    }

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }
}
