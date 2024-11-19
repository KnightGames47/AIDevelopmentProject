using BTree;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;
        if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}
