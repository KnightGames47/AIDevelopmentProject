using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public abstract class Node_Composite : Node
    {
        [HideInInspector] public List<Node> children = new List<Node>();

        public override Node Clone()
        {
            Node_Composite node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}
