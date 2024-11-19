using UnityEngine;
using BTree;

public class Node_Repeat : Node_Decorator
{
    //Possible additions:
    /*
     * How many times to loop
     * only while successful
     * only while failing
     */
    protected override NodeState OnEvaluate()
    {
        //Constantly repeating the child loop
        child.Evaluate();
        return NodeState.RUNNING;
    }

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }
}

