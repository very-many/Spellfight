using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class StaffDragAndDropWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Drag And Drop")]
    public static void ShowExample()
    {
        StaffDragAndDropWindow wnd = GetWindow<StaffDragAndDropWindow>();
        wnd.titleContent = new GUIContent("Drag And Drop");
    }

    

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Staff/DragAndDrop/StaffDragAndDrop.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Staff/DragAndDrop/StaffDragAndDrop.uss");

        DragAndDropManipulator manipulator = new(rootVisualElement.Q<VisualElement>("object"));
    }
}




