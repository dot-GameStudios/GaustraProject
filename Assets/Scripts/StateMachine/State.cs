using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class State : MonoBehaviour
{
    [Serializable]
    public struct Transition
    {
        public State TransitonToState; //what state to transition to
        public List<Condition> stateConditions; //holds state conditions

        public void Initialize()
        {
            for (int i = stateConditions.Count - 1; i >= 0; i--)
            {
                stateConditions[i].Initialize();
            }
        }
    }

    public class multiParameterEvent : UnityEvent<string, string> { }


    private bool active;
    public bool isActive { get => active; }
    [Header("References")]
    public Data data;
    public string ControllerName;
    private StateInputController StateInputController;
    public StateInputController stateInputController { get => StateInputController; }
    public List<Transition> Transitions = new List<Transition>(); //holds the states that the current state can transition to

    [Header("Events")]
    public UnityEvent OnStateEnter;
    public UnityEvent OnUpdate;
    public UnityEvent OnStateExit;

    // Start is called before the first frame update
    public void Start()
    {
        for (int i = 0; i < Transitions.Count; i++)
        {
            Transitions[i].Initialize();
        }

    }

    public void Enter()
    {
        OnStateEnter.Invoke();
    }

    public void SetInputController(StateInputController Controller)
    {
        StateInputController = Controller;
    }

    public void ActivateInputController(bool value)
    {
        StateInputController.TotalInputLockToggle(value);
    }
    // Update is called once per frame
    public void Update()
    {
        if (active)
        {
            OnUpdate.Invoke();
        }
    }

    public void SetBoolCondition(string Condition_, bool value)
    {
        data.GetBool(Condition_).Value = value;
    }

    public void Exit()
    {
        OnStateExit.Invoke();
    }

    public void SetActive(bool active_)
    {
        this.active = active_;
    }

}