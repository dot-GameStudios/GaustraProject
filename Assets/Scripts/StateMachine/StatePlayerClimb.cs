using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerClimb : State
{
    [Header("References")]
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private Rigidbody2DTrigger RB2DTrigger;   


    [Header("Collision data")]
    //Important variables for ground check
    public LayerMask GroundLayer;
    public LayerMask PlatformLayer;

    public Transform Feet;

    [Header("Ladder Climb data")]
    [SerializeField] private Vector2 centerOfLadder;
    public bool CanLeaveLadder;
    public float GrabTimer;
    public void GetData()
    {
        GrabTimer = 0.1f;
        CanLeaveLadder = false;
        RigidBody = GetComponent<Rigidbody2D>();
        RB2DTrigger = GetComponent<Rigidbody2DTrigger>();
        centerOfLadder = RB2DTrigger.Collider.bounds.center;   
    }

    public void SnapToLadder()
    {
        transform.position = new Vector3(centerOfLadder.x, transform.position.y, transform.position.z);
    }

    public void GrabTimerCountDown()
    {

        if (GrabTimer >= 0)
        {
            GrabTimer -= Time.deltaTime;
            
        }
        if(GrabTimer <= 0)
        {
            CanLeaveLadder = true;
        }
        
    }

    public void LimitJump(string Condition_)
    {
        stateInputController.InputKeyBoolConditionLock("Jump", Condition_);
    }

    public void ClimbDownGroundCheck(string Condition_)
    {
        //Ground check that uses 1 ray for detection
        float rayDistance = 0.3f;

        if (RayCastGroundCheck(Feet.position, Vector2.down, rayDistance, GroundLayer))
        {
            //if the ray return true then isClimbing is false
            leaveLadder(Condition_);
        }
    }

    public void ClimbUpGroundCheck(string Condition_) {
        //Ground check that uses 1 ray for detection
        float rayDistance = 0.3f;

        if (RayCastGroundCheck(Feet.position, Vector2.down, rayDistance, PlatformLayer))
        {
            //if the ray return true then isClimbing is false
            leaveLadder(Condition_);          
        }
    }

    public bool RayCastGroundCheck(Vector3 Position_, Vector2 Direction_, float Distance_, LayerMask CollisionLayer_)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(Position_, Direction_, Distance_, CollisionLayer_);
        Debug.DrawRay(Position_, Direction_, Color.red);

        if (rayHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ZeroVelocity()
    {
        SetVelocity(Vector2.zero);
    }

    public void SetVelocity(Vector2 value)
    {
        RigidBody.velocity = value;
    }

    public void leaveLadder(string Condition_)
    {
        if (CanLeaveLadder)
        {
            SetBoolNode(Condition_, false);
        }
        CanLeaveLadder = false;
    }

    public void ChangeRigidBodyType(string newType)
    {
        Debug.Log(newType);
        if (newType.Contains("Dynamic"))
        {
            RigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
        else if (newType.Contains("Kinematic"))
        {
            RigidBody.bodyType = RigidbodyType2D.Kinematic;
        }
        else if (newType.Contains("Static"))
        {
            RigidBody.bodyType = RigidbodyType2D.Static;
        }
    }
}
