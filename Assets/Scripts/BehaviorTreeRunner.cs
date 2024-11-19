using UnityEngine;
using BTree;

public class BehaviorTreeRunner : MonoBehaviour
{
    BehaviorTree tree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();

        var log1 = ScriptableObject.CreateInstance<Node_DebugAction>();
        log1.message = "Hello1";

        var wait1 = ScriptableObject.CreateInstance<Node_Wait>();

        var log2 = ScriptableObject.CreateInstance<Node_DebugAction>();
        log2.message = "Hello2";

        var wait2 = ScriptableObject.CreateInstance<Node_Wait>();

        var log3 = ScriptableObject.CreateInstance<Node_DebugAction>();
        log3.message = "Hello3";

        var wait3 = ScriptableObject.CreateInstance<Node_Wait>();



        var sequence = ScriptableObject.CreateInstance<Node_Sequencer>();
        sequence.children.Add(log1);
        sequence.children.Add(wait1);
        sequence.children.Add(log2);
        sequence.children.Add(wait2);
        sequence.children.Add(log3);
        sequence.children.Add(wait3);

        var loop = ScriptableObject.CreateInstance<Node_Repeat>();
        loop.child = sequence;

        tree.rootNode = loop;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Evaluate();
    }
}
