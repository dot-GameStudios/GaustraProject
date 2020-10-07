using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerActionless : State
{

    [Header("References")]
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private Rigidbody2DTrigger RB2DTrigger;

    public void GetData()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        RB2DTrigger = GetComponent<Rigidbody2DTrigger>();
    }

    public void StartCountdown(string Condition_) { 
        
        StartCoroutine(ActionlessCountDown(data.GetFloat(Condition_).Value));
    }

    IEnumerator ActionlessCountDown(float time)
    {
        float duration = time;

        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        SetBoolNode("Actionless", false);    
    }
}
