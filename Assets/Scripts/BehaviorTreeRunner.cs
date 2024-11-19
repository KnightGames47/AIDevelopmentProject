using UnityEngine;
using BTree;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tree = tree.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        tree.Evaluate();
    }
}
