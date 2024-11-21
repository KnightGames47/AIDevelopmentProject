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
        public BlackBoard blackboard = new BlackBoard();

        public Node.NodeState Evaluate() 
        { 
            if(rootNode.state == Node.NodeState.RUNNING)
                treeState = rootNode.Evaluate(); 

            return treeState;
        }

#if UNITY_EDITOR
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behavior Tree (CreateNode)");
            nodes.Add(node);

            if(!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);

            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (CreateNode)");

            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behavior Tree (DeleteNode)");
            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            Node_RootNode rootNode = parent as Node_RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behavior Tree (AddChild)");
                rootNode.child = child;
                EditorUtility.SetDirty(rootNode);
            }

            Node_Decorator decorator = parent as Node_Decorator;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }

            Node_Composite composite = parent as Node_Composite;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (AddChild)");
                composite.children.Add(child);
                EditorUtility.SetDirty(composite);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            Node_RootNode rootNode = parent as Node_RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behavior Tree (RemoveChild)");
                rootNode.child = null;
                EditorUtility.SetDirty(rootNode);
            }

            Node_Decorator decorator = parent as Node_Decorator;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (RemoveChild)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
            }

            Node_Composite composite = parent as Node_Composite;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (RemoveChild)");
                composite.children.Remove(child);
                EditorUtility.SetDirty(composite);
            }
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
#endif

        public void Traverse(Node node, System.Action<Node> visitor)
        {
            if(node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visitor));
            }
        }

        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });

            return tree;
        }

        public void Bind(AiAgent agent)
        {
            Traverse(rootNode, node =>
            {
                node.agent = agent;
                node.blackboard = blackboard;
            });
        }
    }
}
