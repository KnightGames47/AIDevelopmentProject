using BTree;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;

public class BehaviorTreeEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    BehaviorTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("BehaviorTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        //for double click handling

        if(Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorWindows/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorWindows/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviorTreeView>();
        inspectorView = root.Q<InspectorView>();

        treeView.OnNodeSelected = OnNodeSelectionChanged;

        //Needed for deleting...
        treeView.focusable = true;
        inspectorView.focusable = true;

        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playmodeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
    }


    private void OnDisable()
    {
        EditorApplication.playmodeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged()
    {
        throw new NotImplementedException();
    }


    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;
        if(!tree)
        {
            if(Selection.activeGameObject)
            {
                BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                if(runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                treeView.PopulateView(tree);
            }
        }
        else
        {
            if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}
