using UnityEngine;
using BTree;

[RequireComponent(typeof(AiAgent))]
public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tree = tree.Clone();
        tree.Bind(GetComponent<AiAgent>());
    }

    // Update is called once per frame
    void Update()
    {
        tree.Evaluate();
    }
}
