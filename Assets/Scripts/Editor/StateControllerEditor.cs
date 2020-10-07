using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(StateController))]
public class StateControllerEditor : Editor
{
    private StateController controller;

    private State currentState;
    private ReorderableList states;
    private void OnEnable()
    {
        controller = (StateController)target;
        currentState = controller.CurrentState;

        if (states == null)
        {
            states = new ReorderableList(serializedObject,
                serializedObject.FindProperty("bools"),
                true, true, true, true);
        }

        //bools.drawHeaderCallback += DrawBoolHeader;
        //bools.drawElementCallback += DrawBoolElement;
        //bools.onAddCallback += AddBoolElement;
        //bools.onRemoveCallback += RemoveBoolElement;

       
    }

    private void OnDisable()
    {
        //bools.drawHeaderCallback -= DrawBoolHeader;
        //bools.drawElementCallback -= DrawBoolElement;
        //bools.onAddCallback -= AddBoolElement;
        //bools.onRemoveCallback -= RemoveBoolElement;
    }
}
