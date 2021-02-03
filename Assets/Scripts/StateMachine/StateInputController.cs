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
    [SerializeField] private List<DataInputAction> Actions = new List<DataInputAction>();
    public DataInputKey GetKey(string name) { return keys.Find(key => key.Name == name); }
    public DataInputAction GetAction(string name) { return Actions.Find(action => action.Name == name); }
    public List<DataInputKey> Keys { get => keys; }

    [Header("References")]
    [SerializeField] private Data data;
    [SerializeField] private State UseState;


    void Awake()
    {
        //for (int i = keys.Count - 1; i >= 0; i--)
        //    keys[i].Start(data);
        for (int i = Actions.Count - 1; i >= 0; i--)
            Actions[i].Start(data);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(inputMaster.PlayerControls.Get().actions);
        //for (int i = keys.Count - 1; i >= 0; i--)
        //{
        //    keys[i].Update();
        //}

        for (int i = Actions.Count - 1; i >= 0; i--)
        {
            Actions[i].Update();
        }
    }

    public void CheckStateActive()
    {
        if (UseState.isActive == true)
        {
            TotalInputLockToggle(true);
        }
    }

    public void InputActionBoolConditionLock(string actionName, string Condition_)
    {
        //with a string to the Key's name and the string to the Condition it is tied to, set the Key's active equal to the Bool condition
        var Condition = data.GetBool(Condition_);
        GetAction(actionName).SetActive(Condition.Value);
        
    }

    public void InputKeyLocked(string keyName)
    {
        //GetKey(keyName).SetActive(false);
    }

    public void InputKeyUnlocked(string keyName)
    {
        //GetKey(keyName).SetActive(true);
    }

    public void InputKeyToggle(string keyName)
    {
       // GetKey(keyName).toggleActive();
    }

    public void TotalInputLockToggle(bool value)
    {
        //Loop through all the keys and set InputLock based on the given value
        for (int i = Actions.Count - 1; i >= 0; i--)
        {
            Actions[i].SetActive(value);
        }
    }

    IEnumerator NextInputCountDown(float time)
    {
        float inputTimer = time;

        while (inputTimer >= 0)
        {
            inputTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
