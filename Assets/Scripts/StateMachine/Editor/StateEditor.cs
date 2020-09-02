using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(State))]
public class StateEditor : Editor
{
    private State state;

    private ReorderableList transitions;

    private void OnEnable()
    {
        state = (State)target;
        
        if (transitions == null)
        {
            transitions = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Transitions"),
                true, true, true, true);
        }

        transitions.drawHeaderCallback += DrawTransitionHeader;
        transitions.drawElementCallback += DrawTransitionElement;
        transitions.onAddCallback += AddTransitionElement;
        transitions.onRemoveCallback += RemoveTransitionElement;

    }

    private void OnDisable()
    {
        transitions.drawHeaderCallback -= DrawTransitionHeader;
        transitions.drawElementCallback -= DrawTransitionElement;
        transitions.onAddCallback -= AddTransitionElement;
        transitions.onRemoveCallback -= RemoveTransitionElement;
    }

    private void DrawHeader(Rect rect, string label)
    {
        GUI.Label(rect, label);
    }

    private void DrawTransitionHeader(Rect rect)
    {
        DrawHeader(rect, "State Transitions");
    }

    private void DrawTransitionElement(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty element = transitions.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, element);
    }

    private void AddTransitionElement(ReorderableList list)
    {
        state.Transitions.Add(new State.Transition());
        //data.Bool(new DataBool() { Name = "bool_" + list.count });
        EditorUtility.SetDirty(target);
    }

    private void RemoveTransitionElement(ReorderableList list)
    {
        state.Transitions.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        transitions.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
