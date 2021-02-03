using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class DataInputAction
{
    public string Name;
    public bool Active;
    private enum ActionType { SINGLE, SEQUENCE, COMBO };
    [SerializeField] private ActionType TypeOfAction;

    Thread inputThread;

    [SerializeField] List<DataInputKey> Keys = new List<DataInputKey>();

    private bool ActionPerformed;
    private bool BeforeActionPerformed;
    private bool AfterActionPerformed;
    [SerializeField]private int NumberOfKeysPressed;

    private int keyIndexer = 0;
    [SerializeField]private float inputTimer;
    public float minimumInputWindow;

    [Header("Events")]
    [SerializeField] private UnityEvent2 beforePerformed;
    [SerializeField] private UnityEvent2 onPerformed;
    [SerializeField] private UnityEvent2 afterPerformed;

    // Start is called before the first frame update
    public void Start(Data data)
    {
        for (int i = Keys.Count - 1; i >= 0; i--)
        {
            Keys[i].Start(data);
        }
        ActionPerformed = false;
        BeforeActionPerformed = false;
        AfterActionPerformed = false;
        NumberOfKeysPressed = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Active)
        {

            switch (TypeOfAction)
            {
                case ActionType.SINGLE:
                    Keys[0].Update();

                    if (Keys[0].GetKeyPress("onKeyDown"))
                    {
                        BeforeActionPerformed = true;
                    }
                    if (Keys[0].GetKeyPress("onKey"))
                    {
                        ActionPerformed = true;
                    }
                    if (Keys[0].GetKeyPress("onKeyUp"))
                    {
                        AfterActionPerformed = true;
                    }
                    break;

                case ActionType.SEQUENCE:
                    Keys[keyIndexer].Update();

                    if (Keys[keyIndexer].GetKeyPress("onKeyDown"))
                    {
                        NumberOfKeysPressed++;
                        NextInputTimer(minimumInputWindow);

                        if (keyIndexer < Keys.Count - 1)
                        {
                            keyIndexer++;
                        }
                    }

                    if (inputTimer > 0)
                    {
                        NextInputCountDown();
                    }
                    else
                    {
                        NumberOfKeysPressed = 0;
                        keyIndexer = 0;
                    }

                    if (NumberOfKeysPressed >= Keys.Count)
                    {
                        BeforeActionPerformed = true;
                    }

                    if (BeforeActionPerformed && inputTimer <= 0)
                    {
                        ActionPerformed = true;
                        NumberOfKeysPressed = 0;
                    }

                    if (ActionPerformed && NumberOfKeysPressed == 0)
                    {
                        AfterActionPerformed = true;
                    }
                    break;

                case ActionType.COMBO:

                    Keys[keyIndexer].Update();
                    
                    if (Keys[keyIndexer].GetKeyPress("onKeyDown"))
                    {
                        NumberOfKeysPressed++;
                        NextInputTimer(minimumInputWindow);

                        if (keyIndexer < Keys.Count - 1)
                        {
                            keyIndexer++;
                        }
                    }
                
                    if(inputTimer > 0)
                    {
                        NextInputCountDown();
                    }
                    else
                    {
                        NumberOfKeysPressed = 0;
                        keyIndexer = 0;
                    }

                    if (NumberOfKeysPressed >= Keys.Count)
                    {
                        BeforeActionPerformed = true;
                    }

                    if(BeforeActionPerformed && inputTimer <= 0)
                    {
                        ActionPerformed = true;
                        NumberOfKeysPressed = 0;
                    }

                    if(ActionPerformed && NumberOfKeysPressed == 0)
                    {
                        AfterActionPerformed = true;
                    }
                    break;
            }

        }
        
        if (BeforeActionPerformed)
        {
            beforePerformed.Invoke();
            BeforeActionPerformed = false;
        }
        if (ActionPerformed)
        {
            onPerformed.Invoke();
            ActionPerformed = false;
            NumberOfKeysPressed = 0;
            keyIndexer = 0;
        }
        if (AfterActionPerformed)
        {
            afterPerformed.Invoke();
            AfterActionPerformed = false;
        }
    }

    public void SetActive(bool value)
    {
        Active = value;
    }

    public void ToggleActive()
    {
        Active = !Active;
    }

    IEnumerator WaitForInput(float time_)
    {
        yield return new WaitForSeconds(time_);
    }

    IEnumerator CountDown(float time)
    {
        float duration = time;

        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
    }

    public void NextInputCountDown()
    {
        inputTimer -= Time.deltaTime;
    }

    public void NextInputTimer(float time)
    {
        inputTimer = time;
    }
}
