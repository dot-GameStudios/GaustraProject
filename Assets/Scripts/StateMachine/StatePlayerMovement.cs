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
    public GameObject playerFeet;

    private Vector2 slopeNormalPerp;
    private float slopeDownAngle;

    public string MovementInputNode;
    private DataFloat MovementInput;
    public PhysicsMaterial2D FrictionLess;
    public PhysicsMaterial2D SlopeFriction;

    //stores a check point for when the player needs to respawn somewhere
    public GameObject CheckPoint;

    [Header("References")]
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private Rigidbody2DTrigger RB2DTrigger;


    public void GetData()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        RB2DTrigger = GetComponent<Rigidbody2DTrigger>();
        MovementInput = data.GetFloat(MovementInputNode);
    }

    public void GroundCheck(float rayDistance, string Condition_)
    {
        //Ground check that uses 2 rays for detection     
        RaycastHit2D feetHit = Physics2D.Raycast(playerFeet.transform.position, Vector2.down, rayDistance, GroundLayer);

        RaycastHit2D leftHit = Physics2D.Raycast(rayPosLeft.transform.position, Vector2.left, rayDistance, GroundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayPosRight.transform.position, Vector2.right, rayDistance, GroundLayer);

        slopeNormalPerp = Vector2.Perpendicular(feetHit.normal).normalized;
        slopeDownAngle = Vector2.Angle(feetHit.normal, Vector2.up);

        Debug.DrawLine(playerFeet.transform.position, playerFeet.transform.position + Vector3.down * rayDistance, Color.red);

        Debug.DrawLine(rayPosRight.transform.position, rayPosRight.transform.position + Vector3.down * rayDistance, Color.red);
        Debug.DrawLine(rayPosLeft.transform.position, rayPosLeft.transform.position + Vector3.down * rayDistance, Color.red);

        Debug.DrawLine(feetHit.point, feetHit.point + slopeNormalPerp, Color.yellow);

        if (feetHit || rightHit || leftHit)
        {
            if (slopeDownAngle != 0)
            {
                SetBoolNode("IsOnSlope", true);
                if (MovementInput.Value == 0)
                {
                    SetPhysicsMaterial2D(SlopeFriction);
                }
            }
            else
            {
                SetBoolNode("IsOnSlope", false);
            }
            SetVector2Node("SlopeDirection", slopeNormalPerp);
            //otherwise Grounded is true
            SetBoolNode(Condition_, true);
            stateInputController.InputActionBoolConditionLock("Jump", Condition_);

        }
        else
        {
            //if both rays return false then Grounded is false
            SetBoolNode(Condition_, false);
            SetBoolNode("IsOnSlope", false);
        }


    }

    public void SetPhysicsMaterial2D(PhysicsMaterial2D newMaterial)
    {
        RigidBody.sharedMaterial = newMaterial;
    }

    public void SetPhysicsMaterial2D(PhysicsMaterial2D newMaterial, string BoolNode)
    {
        if (data.GetBool(BoolNode).Value)
        {
            RigidBody.sharedMaterial = newMaterial;
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
        stateInputController.InputActionBoolConditionLock("Jump", Condition_);
    }

    public void Dodge(float speed)
    {
        float DodgeSpeed = speed;
        Vector2 DodgeDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            DodgeDirection += Vector2.left;
            DodgeDirection.Normalize();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            DodgeDirection += Vector2.right;
            DodgeDirection.Normalize();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            DodgeDirection += Vector2.up;
            DodgeDirection.Normalize();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            DodgeDirection += Vector2.down;
            DodgeDirection.Normalize();
        }

        Debug.DrawLine(transform.position, transform.position + new Vector3(DodgeDirection.x, DodgeDirection.y, transform.position.z), Color.green);
        RigidBody.velocity = DodgeDirection * DodgeSpeed;
        SetFloatNode("ActionlessTimer", 0.5f);
        SetActionless(true);
    }

    public void SetActionless(bool value_)
    {
        SetBoolNode("Actionless", value_);
    }

    public void InputLockToggle(string keyName)
    {
        //stateInputController.InputKeyBoolLock(key, stateInputController.GetKey(key).toggleActive());
        stateInputController.InputKeyToggle(keyName);
    }

    public void GrabLadder(string Condition_)
    {
        if (RB2DTrigger.ColliderTag == "Ladder")
        {
            ChangeRigidBodyType("Kinematic");
            SetBoolNode(Condition_, true);
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
