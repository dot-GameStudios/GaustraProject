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

    //public class multiParameterEvent : UnityEvent<string, string> { }


    private bool active;
    public bool isActive { get => active; }
    [Header("References")]
    public Data data;
    public string ControllerName;
    [SerializeField]private StateInputController StateInputController;
    public StateInputController stateInputController { get => StateInputController; }
    public List<Transition> Transitions = new List<Transition>(); //holds the states that the current state can transition to

    [Header("Events")]
    public UnityEvent2 OnStateEnter;
    public UnityEvent2 OnUpdate;
    public UnityEvent2 OnStateExit;

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
        if (StateInputController)
        {
            StateInputController.TotalInputLockToggle(value);
        }
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        if (active)
        {
            OnUpdate.Invoke();
        }
    }

    public void SetBoolNode(string NodeName_, bool value_)
    {
        data.GetBool(NodeName_).Value = value_;
    }

    public void SetFloatNode(string NodeName_, float value_)
    {
        data.GetFloat(NodeName_).Value = value_;
    }

    public void SetVector2Node(string NodeName_, Vector2 value_)
    {
        data.GetVector2(NodeName_).Value = value_;
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