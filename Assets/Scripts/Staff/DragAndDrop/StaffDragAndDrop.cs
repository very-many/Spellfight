using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StaffDragAndDropWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Staff Editor")]
    public static void ShowExample() => GetWindow<StaffDragAndDropWindow>("Staff Editor");

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // Load Styles
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Staff/DragAndDrop/StaffDragAndDrop.uss");
        if (styleSheet != null) root.styleSheets.Add(styleSheet);

        // Container for our 4 rows
        VisualElement mainContainer = new VisualElement() { name = "slots-container" };
        root.Add(mainContainer);

        // Create 4 Rows
        for (int i = 0; i < 4; i++)
        {
            VisualElement row = new VisualElement();
            row.AddToClassList("staff-row");

            // Row Label (The "Staff" Image or Icon)
            VisualElement staffIcon = new VisualElement();
            staffIcon.AddToClassList("staff-icon");
            staffIcon.Add(new Label(i < 3 ? $"Staff {i + 1}" : "Storage"));
            row.Add(staffIcon);

            // The actual drop zone for this row
            VisualElement slotContainer = new VisualElement() { name = "slots" };
            slotContainer.AddToClassList("slot-container");

            // Create some empty slots for each row
            for (int s = 0; s < 5; s++)
            {
                VisualElement slot = new VisualElement();
                slot.AddToClassList("slot");
                slotContainer.Add(slot);
            }

            row.Add(slotContainer);
            mainContainer.Add(row);
        }

        // Spawn Button for Spells
        Button spawnButton = new Button(() => SpawnSpell()) { text = "Add Spell to Storage" };
        root.Add(spawnButton);
    }

    void SpawnSpell()
    {
        // Find the 4th row (Storage)
        var rows = rootVisualElement.Query(className: "slot-container").ToList();
        VisualElement storage = rows[3];

        // Find the first empty slot in storage
        foreach (var slot in storage.Children())
        {
            if (slot.childCount == 0)
            {
                VisualElement spell = new VisualElement();
                spell.AddToClassList("object");

                // Give it a random color to represent different spells
                spell.style.backgroundColor = new StyleColor(Random.ColorHSV());

                // Attach the manipulator
                spell.AddManipulator(new DragAndDropManipulator(spell));

                slot.Add(spell);
                break;
            }
        }
    }
}




