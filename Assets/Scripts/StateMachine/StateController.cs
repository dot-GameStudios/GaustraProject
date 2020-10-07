using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private State[] states; //holds all the states in the object
    public State[] AllStates { get => states; }
    [Header("State Input Controllers")]
    [SerializeField]private StateInputController[] stateInputControllers; //holds all the state input controllers
    public StateInputController[] AllInputControllers { get => stateInputControllers; } //a getter for the StateInputControllers array

    [Header("Current State")]
    [SerializeField] private State currentState; //holds the currently active state
    public State CurrentState { get => currentState; } //a getter for the private value of the same name

    // Start is called before the first frame update
    void Start()
    {
        //Make sure the States array has data inside it
        if (states.Length <= 0)
        {
            Debug.LogWarning("You need a state for the state controller");
        }
        else
        {
            //Set the Current State to be the State in index 0
            currentState = states[0];
            //Activate the current states
            currentState.SetActive(true);

            stateInputControllers = GetComponents<StateInputController>();

            //Make sure the State Input Controllers array has data inside it
            if (stateInputControllers.Length <= 0)
            {
                Debug.LogWarning("You need to add state input controllers!");
            }
            else {

                //Loop through all known states.
                for (int i = states.Length - 1; i >= 0; i--)
                {
                    int j = stateInputControllers.Length - 1;

                    while (j >= 0 && states[i].ControllerName != null)
                    {
                        if (states[i].ControllerName == stateInputControllers[j].ControllerName)
                        {
                            //If Yes, set the State's Input Controller to be the one the matches with it.
                            states[i].SetInputController(stateInputControllers[j]);
                        }
                        j--;
                        
                    }
                    /* while (j <= 0 || states[i].ControllerName != null);

                    ////Loop through all known State Input Controllers.
                    //for (int j = stateInputControllers.Length - 1; j >= 0; j--)
                    //{
                    //    //Check if the current State has the name of the current State Input Controller.
                    //    if (states[i].ControllerName == stateInputControllers[j].ControllerName)
                    //    {
                    //        //If Yes, set the State's Input Controller to be the one the matches with it.
                    //        states[i].SetInputController(stateInputControllers[j]);
                    //    }
                    //}*/
                }
            }
            //Activate all inputs in the Current State's input controller
            currentState.ActivateInputController(true);
            currentState.Enter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //On every frame, evaluate the transitions in the current state
        EvaluateStateTransition(currentState);
    }

    public void EvaluateStateTransition(State state_)
    {
        //Loop through all the transitions
        for (int i = state_.Transitions.Count - 1; i >= 0; i--)
        {
            //Loop through all Conditions within the current Transition Index
            for(int j = state_.Transitions[i].stateConditions.Count - 1; j >=0; j--)
            {
                //Evaluate Transition's conditions
                if (state_.Transitions[i].stateConditions[j].Evaluate())
                {
                    //If returns true, Transition to the next State
                    TransitionTo(state_.Transitions[i].TransitonToState);
                }
            }
        }
    }

    void TransitionTo(State state_) {
        //Set the Current State to not be active, call the Exit function, and disable all inputs in its corresponding inputcontroller
        currentState.SetActive(false);
        currentState.ActivateInputController(false);
        currentState.Exit();

        state_.SetActive(true);
        currentState = state_;
        currentState.ActivateInputController(true);
        currentState.Enter();
    }
}
