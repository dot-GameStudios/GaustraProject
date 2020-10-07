using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[Serializable]
public class StateInputAction
{

    private InputMaster iMaster;

    public string name;
    public bool Hold;
    public bool isButtonSequence; //if True, the script will check if the actions are performed in order
    public bool isButtonCombo; //if True, the script will check if the actions are all activated at once

    public List<string> InputNames;
    public List<InputAction> Inputs;

    public UnityEvent BeforeActionPerformed;
    public UnityEvent ActionPerforms;
    public UnityEvent AfterActionPerformed;

    public void SetInputMaster(InputMaster newMaster)
    {
        iMaster = newMaster;
    }

    public void Initialize()
    {
        if (iMaster != null)
        {
            iMaster.PlayerControls.Enable();
        }
        for (int i = InputNames.Count - 1; i >= 0; i--)
        {
            if (iMaster.PlayerControls.Get().FindAction(InputNames[i]) != null)
            {
                Inputs.Add(iMaster.PlayerControls.Get().FindAction(InputNames[i]));

                Inputs[i].started += ctx => InvokeBeforeEvents();
                Inputs[i].performed += ctx => InvokeEvents();
                Inputs[i].canceled += ctx => InvokeAfterEvents();
            }
        }
    }

    public void Update()
    {
        //for(int i = 0; i <= Inputs.Count - 1; i++)
        //{
        //    Inputs[i].started += ctx => InvokeBeforeEvents();
        //    Inputs[i].performed += ctx => InvokeEvents();
        //    Inputs[i].canceled += ctx => InvokeAfterEvents();
        //}


    }

    public void InvokeEvents()
    {
        ActionPerforms.Invoke();
    }

    public void InvokeBeforeEvents()
    {
        BeforeActionPerformed.Invoke();
    }

    public void InvokeAfterEvents()
    {
        AfterActionPerformed.Invoke();
    }
}
