using UnityEngine;
using BTree;

public class Node_DebugAction : Node_Action
{
    public string message;

    protected override NodeState OnEvaluate()
    {
        Debug.Log($"OnEvaluate {message}");
        return NodeState.SUCCESS;
    }

    protected override void OnStart()
    {
        Debug.Log($"OnStart {message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop {message}");
    }
}
