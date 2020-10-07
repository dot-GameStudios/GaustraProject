using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StateInputController : MonoBehaviour
{

    [Header("Controller Name")]
    public string ControllerName;

    [Header("Keys")]
    [SerializeField] private List<DataInputKey> keys = new List<DataInputKey>();
    public DataInputKey GetKey(string name) { return keys.Find(key => key.Name == name); }
    public List<DataInputKey> Keys { get => keys; }

    [Header("References")]
    [SerializeField] private Data data;
    [SerializeField] private State UseState;


    void Awake()
    {

        for (int i = keys.Count - 1; i >= 0; i--)
            keys[i].Start(data);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inputMaster.PlayerControls.Get().actions);
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            keys[i].Update();
        }

    }

    public void CheckStateActive()
    {
        if (UseState.isActive == true)
        {
            TotalInputLockToggle(true);
        }
    }

    public void InputKeyBoolConditionLock(string keyName, string Condition_)
    {
        //with a string to the Key's name and the string to the Condition it is tied to, set the Key's active equal to the Bool condition
        var Condition = data.GetBool(Condition_);
        GetKey(keyName).SetActive(Condition.Value);
        
    }

    public void InputKeyLocked(string keyName)
    {
        GetKey(keyName).SetActive(false);
    }

    public void InoutKeyUnlocked(string keyName)
    {
        GetKey(keyName).SetActive(true);
    }

    public void InputKeyToggle(string keyName)
    {
        GetKey(keyName).toggleActive();
    }

    public void TotalInputLockToggle(bool value)
    {
        //Loop through all the keys and set InputLock based on the given value
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            keys[i].SetActive(value);
        }
    }
}
