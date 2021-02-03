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
    //[SerializeField] private bool active;

    [Header("Data")]
    [SerializeField] private DataFloat dataNode;

    //[Header("Events")]
    [SerializeField] private bool onPositiveKeyDown;
    [SerializeField] private bool onPositiveKey;
    [SerializeField] private bool onPositiveKeyUp;
    [SerializeField] private bool onNegativeKeyDown;
    [SerializeField] private bool onNegativeKey;
    [SerializeField] private bool onNegativeKeyUp;

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
        //if (active == true)
        //{
        bool currentlyPressedPositive = Input.GetKey(positive);
        bool currentlyPressedNegative = Input.GetKey(negative);

        onPositiveKeyDown = false;
        onPositiveKey = false;
        onPositiveKeyUp = false;
        onNegativeKeyDown = false;
        onNegativeKey = false;
        onNegativeKeyUp = false;

        if (currentlyPressedPositive && !pressedPositive)
            onPositiveKeyDown = true;
        else if (currentlyPressedPositive && pressedPositive)
            onPositiveKey = true;
        else if (!currentlyPressedPositive && pressedPositive)
            onPositiveKeyUp = true;
        else if (currentlyPressedNegative && !pressedNegative)
            onNegativeKeyDown = true;
        else if (currentlyPressedNegative && pressedNegative)
            onNegativeKey = true;
        else if (!currentlyPressedNegative && pressedNegative)
            onNegativeKeyUp = true;

        dataNode.Value = 0;
        if (currentlyPressedPositive)
            dataNode += 1;
        if (currentlyPressedNegative)
            dataNode -= 1;

        pressedPositive = currentlyPressedPositive;
        pressedNegative = currentlyPressedNegative;
        //}
    }

    public int GetIfPressed()
    {
        return (int)Mathf.Abs(dataNode.Value);
    }

    public bool GetKeyPress(string keyType)
    {
        if (String.Compare(keyType, "onPositiveKeyDown", true) == 0)
        {
            return onPositiveKeyDown;
        }
        else if (String.Compare(keyType, "onPositiveKey", true) == 0)
        {
            return onPositiveKey;
        }
        else if (String.Compare(keyType, "onPositiveKeyUp", true) == 0)
        {
            return onPositiveKeyUp;
        }
        else if (String.Compare(keyType, "onNegativeKeyDown", true) == 0)
        {
            return onNegativeKeyDown;
        }
        else if (String.Compare(keyType, "onNegativeKey", true) == 0)
        {
            return onPositiveKey;
        }
        else if (String.Compare(keyType, "onNegativeKeyUp", true) == 0)
        {
            return onPositiveKeyUp;
        }
        else if (String.Compare(keyType, "onKeyDown") == 0)
        {
            if (onNegativeKeyDown || onPositiveKeyDown)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (String.Compare(keyType, "onKeyUp") == 0)
        {
            if(onNegativeKeyUp || onPositiveKeyUp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (String.Compare(keyType, "onKey") == 0)
        {
            if (onNegativeKey || onPositiveKey)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
