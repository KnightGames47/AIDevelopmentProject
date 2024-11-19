using UnityEngine;
using BTree;

public class Node_Wait : Node_Action
{
    public float duraction = 1;
    float startTime;
    protected override NodeState OnEvaluate()
    {
        if (Time.time - startTime > duraction) return NodeState.SUCCESS;

        return NodeState.RUNNING;
    }

    protected override void OnStart()
    {
        startTime = Time.time;  
    }

    protected override void OnStop()
    {
        
    }
}
