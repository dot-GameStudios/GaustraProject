using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InputKeyBoolLock(string keyName, string Condition_)
    {
        var Condition = data.GetBool(Condition_);
        //Debug.Log(Condition.Value);
        GetKey(keyName).SetActive(Condition.Value);
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
