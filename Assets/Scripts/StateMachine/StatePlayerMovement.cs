using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePlayerMovement : State
{
    //[SerializeField] private List<DataBool> dataBools = new List<DataBool>();

    //Important variables for ground check
    public LayerMask GroundLayer;
    public GameObject rayPosLeft;
    public GameObject rayPosRight;
    
    //stores a check point for when the player needs to respawn somewhere
    public GameObject CheckPoint;

    [Header("References")]
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private Rigidbody2DTrigger RB2DTrigger;


    public void GetData()
    {
        //for (int i = dataBools.Count - 1; i >= 0; i--)
        //{
        //    dataBools[i] = data.GetBool(dataBools[i].Name);
        //}
        
        RigidBody = GetComponent<Rigidbody2D>();
        RB2DTrigger = GetComponent<Rigidbody2DTrigger>();
    }
 
    public void GroundCheck(string Condition_)
    {
        //Ground check that uses 2 rays for detection
        float rayDistance = 0.3f;
        
        RaycastHit2D leftHit = Physics2D.Raycast(rayPosLeft.transform.position, Vector2.down, rayDistance, GroundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayPosRight.transform.position, Vector2.down, rayDistance, GroundLayer);

        //Debug.DrawRay(rayPosLeft.transform.position, Vector2.down, Color.red);
        //Debug.DrawRay(rayPosRight.transform.position, Vector2.down, Color.red);

        if (leftHit.collider == null && rightHit.collider == null)
        {
            //if both rays return false then Grounded is false
            SetBoolCondition(Condition_, false);
        }
        else
        {
            //otherwise Grounded is true
            SetBoolCondition(Condition_, true);
            stateInputController.InputKeyBoolLock("Jump", Condition_);
        }

        
    }

    public void CrouchCheck(string condition_)
    {
        if (data.GetBool(condition_).Value)
        {
            ChangeGameObjectYScale(1);
        }
    }

    public void LimitJump(string Condition_)
    {
        stateInputController.InputKeyBoolLock("Jump", Condition_);
    }

    public void GrabLadder(string Condition_)
    {
        if (RB2DTrigger.ColliderTag == "Ladder")
        {
            ChangeRigidBodyType("Kinematic");
            SetBoolCondition(Condition_, true);   
        }
    }

    public void InteractWithObject()
    {
        //detect if the object is interactable
        if (RB2DTrigger.ColliderTag == "Interactable")
        {
            RB2DTrigger.Collider.gameObject.GetComponent<InteractableObject>().Interact();
        }
    }

    public void ChangeRigidBodyType(string newType)
    {
        if (newType == "Dynamic")
        {
            RigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
        else if (newType == "Kinematic")
        {
            RigidBody.bodyType = RigidbodyType2D.Kinematic;
        }
        else if (newType == "Static")
        {
            RigidBody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void SetLocationToCheckpoint()
    {
        transform.position = CheckPoint.transform.position;
    }

    public void SetLayer(string layerName)
    {       
        //Sets the gameObject's layer to the string given
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void ResetLayer(float time)
    {
        StartCoroutine(CountDown(time));   
    }

    public void GetCheckPoint()
    {
        if (RB2DTrigger.ColliderTag == "Checkpoint")
        {
            CheckPoint = RB2DTrigger.Collider.gameObject;
        }
    }

    IEnumerator CountDown(float time)
    {
        float duration = time; 
        
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        SetLayer("Default");
    }

    public void ChangeGameObjectYScale(float size)
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, size, gameObject.transform.localScale.z);
    }
}
