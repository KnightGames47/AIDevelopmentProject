using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public abstract class Node_Composite : Node
    {
        public List<Node> children = new List<Node>();
    }
}
