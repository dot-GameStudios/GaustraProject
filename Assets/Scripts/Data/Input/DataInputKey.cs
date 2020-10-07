using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DataInputKey : DataInput
{
    [SerializeField] private string name;
    public string Name { get { return name; } set { name = value; } }
    [SerializeField] private KeyCode positive;
    [SerializeField] private KeyCode negative;
    [SerializeField] private bool active;
    private enum ActivateType { OnKey, OnKeyDown, OnKeyUp};
    [SerializeField] private ActivateType ActivatesOn = ActivateType.OnKey;

    [Header("Data")]
    [SerializeField] private DataFloat dataNode;

    [Header("Events")]
    [SerializeField] private UnityEvent2 onPositiveKeyDown;
    [SerializeField] private UnityEvent2 onPositiveKey;
    [SerializeField] private UnityEvent2 onPositiveKeyUp;
    [SerializeField] private UnityEvent2 onNegativeKeyDown;
    [SerializeField] private UnityEvent2 onNegativeKey;
    [SerializeField] private UnityEvent2 onNegativeKeyUp;

    private bool pressedPositive;
    private bool pressedNegative;

    public override void Start(Data data)
    {
        if (data.Has(dataNode))
            dataNode = data.GetFloat(dataNode.Name);
        else
            data.Add(dataNode);
    }

    public override void Update()
    {
        if (active == true)
        {
            bool currentlyPressedPositive = Input.GetKey(positive);
            bool currentlyPressedNegative = Input.GetKey(negative);

            if (currentlyPressedPositive && !pressedPositive)
                onPositiveKeyDown.Invoke();
            else if (currentlyPressedPositive && pressedPositive)
                onPositiveKey.Invoke();
            else if (!currentlyPressedPositive && pressedPositive)
                onPositiveKeyUp.Invoke();
            else if (currentlyPressedNegative && !pressedNegative)
                onNegativeKeyDown.Invoke();
            else if (currentlyPressedNegative && pressedNegative)
                onNegativeKey.Invoke();
            else if (!currentlyPressedNegative && pressedNegative)
                onNegativeKeyUp.Invoke();

            dataNode.Value = 0;
            if (currentlyPressedPositive)
                dataNode += 1;
            if (currentlyPressedNegative)
                dataNode -= 1;

            pressedPositive = currentlyPressedPositive;
            pressedNegative = currentlyPressedNegative;
        }
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public void toggleActive()
    {
        active = !active;
    }
}
