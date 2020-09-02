using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Events")]
    //public UnityEvent OnStateEnter;
    public UnityEvent OnUpdate;
    public UnityEvent OnInteract;
    //public UnityEvent OnStateExit;


    [Header("References")]
    public Data PlayerData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate.Invoke();
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }


    public void DataBoolToggle(string dataBool_)
    {
        PlayerData.GetBool(dataBool_).Value = !PlayerData.GetBool(dataBool_).Value;
    }
}
