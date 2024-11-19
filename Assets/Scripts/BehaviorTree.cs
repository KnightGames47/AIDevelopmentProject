using UnityEngine;

namespace BTree
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;
        public Node.NodeState treeState = Node.NodeState.RUNNING;

        public Node.NodeState Evaluate() 
        { 
            if(rootNode.state == Node.NodeState.RUNNING)
                treeState = rootNode.Evaluate(); 

            return treeState;
        }
    }
}
