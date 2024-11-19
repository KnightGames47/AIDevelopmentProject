using UnityEngine;

namespace BTree
{
    public abstract class Node_Decorator : Node
    {
        [HideInInspector] public Node child;

        public override Node Clone()
        {
            Node_Decorator node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}
