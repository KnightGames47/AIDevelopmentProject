using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BTree
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;
        public Node.NodeState treeState = Node.NodeState.RUNNING;
        public List<Node> nodes = new List<Node>();

        public Node.NodeState Evaluate() 
        { 
            if(rootNode.state == Node.NodeState.RUNNING)
                treeState = rootNode.Evaluate(); 

            return treeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            Node_RootNode rootNode = parent as Node_RootNode;
            if (rootNode) rootNode.child = child;

            Node_Decorator decorator = parent as Node_Decorator;
            if(decorator) decorator.child = child;

            Node_Composite composite = parent as Node_Composite;
            if (composite) composite.children.Add(child);
        }

        public void RemoveChild(Node parent, Node child)
        {
            Node_RootNode rootNode = parent as Node_RootNode;
            if (rootNode) rootNode.child = null;

            Node_Decorator decorator = parent as Node_Decorator;
            if (decorator) decorator.child = null;

            Node_Composite composite = parent as Node_Composite;
            if (composite) composite.children.Remove(child);
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            Node_RootNode rootNode = parent as Node_RootNode;
            if (rootNode && rootNode.child != null) children.Add(rootNode.child);

            Node_Decorator decorator = parent as Node_Decorator;
            if (decorator && decorator.child != null) children.Add(decorator.child);

            Node_Composite composite = parent as Node_Composite;
            if (composite) return composite.children;

            return children;
        }

        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            return tree;
        }
    }
}
