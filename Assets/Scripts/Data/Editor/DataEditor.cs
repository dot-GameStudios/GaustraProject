using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Data))]
public class DataEditor : Editor {
    
    private Data data;

    private ReorderableList bools;
    private ReorderableList ints;
    private ReorderableList floats;
    private ReorderableList vector2s;

    private void OnEnable() {
        data = (Data)target;

        if(bools == null) {
            bools = new ReorderableList(serializedObject,
                serializedObject.FindProperty("bools"),
                true, true, true, true);
        }

        bools.drawHeaderCallback += DrawBoolHeader;
        bools.drawElementCallback += DrawBoolElement;
        bools.onAddCallback += AddBoolElement;
        bools.onRemoveCallback += RemoveBoolElement;

        if(ints == null) {
            ints = new ReorderableList(data.Ints,
                typeof(DataInt),
                true, true, true, true);
        }

        ints.drawHeaderCallback += DrawIntHeader;
        ints.drawElementCallback += DrawIntElement;
        ints.onAddCallback += AddIntElement;
        ints.onRemoveCallback += RemoveIntElement;

        if (floats == null)
        {
            floats = new ReorderableList(data.Floats,
                typeof(DataFloat),
                true, true, true, true);
        }

        floats.drawHeaderCallback += DrawFloatHeader;
        floats.drawElementCallback += DrawFloatElement;
        floats.onAddCallback += AddFloatElement;
        floats.onRemoveCallback += RemoveFloatElement;

        if (vector2s == null)
        {
            vector2s = new ReorderableList(data.Vector2s, typeof(DataVector2),
                true, true, true, true);
        }

        vector2s.drawHeaderCallback += DrawVector2Header;
        vector2s.drawElementCallback += DrawVector2Element;
        vector2s.onAddCallback += AddVector2Element;
        vector2s.onRemoveCallback += RemoveVector2Element;
    }

    private void OnDisable() {
        bools.drawHeaderCallback -= DrawBoolHeader;
        bools.drawElementCallback -= DrawBoolElement;
        bools.onAddCallback -= AddBoolElement;
        bools.onRemoveCallback -= RemoveBoolElement;
    }

    private void DrawHeader(Rect rect, string label) {
        GUI.Label(rect, label);
    }

    private void DrawBoolHeader(Rect rect) {
        DrawHeader(rect, "Boolean Nodes");
    }

    private void DrawIntHeader(Rect rect) {
        DrawHeader(rect, "Integer Nodes");
    }

    private void DrawFloatHeader(Rect rect)
    {
        DrawHeader(rect, "Float Nodes");
    }

    private void DrawVector2Header(Rect rect)
    {
        DrawHeader(rect, "Vector2 Nodes");
    }

    private void DrawBoolElement(Rect rect, int index, bool active, bool focused) {
        SerializedProperty element = bools.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, element);
    }

    private void DrawIntElement(Rect rect, int index, bool active, bool focused) {
        DataInt node = data.Ints[index];
        EditorGUI.BeginChangeCheck();

        node.Name = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width / 3, rect.height - 1), node.Name);

        node.Value = EditorGUI.IntField(new Rect(rect.x + rect.width / 3 + 8, rect.y, rect.width / 1.5f - 16, rect.height - 1), node.Value);

        if(EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawFloatElement(Rect rect, int index, bool active, bool focused)
    {
        DataFloat node = data.Floats[index];
        EditorGUI.BeginChangeCheck();

        node.Name = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width/3, rect.height - 1), node.Name );

        node.Value = EditorGUI.FloatField(new Rect(rect.x + rect.width/3 + 8, rect.y, rect.width /1.5f - 16, rect.height - 1), node.Value);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawVector2Element(Rect rect, int index, bool active, bool focused)
    {
        DataVector2 node = data.Vector2s[index];
        EditorGUI.BeginChangeCheck();

        node.Name = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width / 3, rect.height - 1), node.Name);

        node.Value = EditorGUI.Vector2Field(new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height - 1), GUIContent.none ,node.Value);


        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void AddBoolElement(ReorderableList list) {
        data.Bool(new DataBool() { Name = "bool_" + list.count });
        EditorUtility.SetDirty(target);
    }

    private void AddIntElement(ReorderableList list) {
        data.Int(new DataInt() { Name = "int_" + list.count });
        EditorUtility.SetDirty(target);
    }

    private void AddFloatElement(ReorderableList list)
    {
        data.Float(new DataFloat() { Name = "float_" + list.count });
        EditorUtility.SetDirty(target);
    }

    private void AddVector2Element(ReorderableList list)
    {
        data.Vector2(new DataVector2() { Name = "vector2_" + list.count });
        EditorUtility.SetDirty(target);
    }

    private void RemoveBoolElement(ReorderableList list) {
        data.Bools.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    private void RemoveIntElement(ReorderableList list) {
        data.Ints.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    private void RemoveFloatElement(ReorderableList list)
    {
        data.Floats.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    private void RemoveVector2Element(ReorderableList list)
    {
        data.Vector2s.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        bools.DoLayoutList();
        ints.DoLayoutList();
        floats.DoLayoutList();
        vector2s.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
