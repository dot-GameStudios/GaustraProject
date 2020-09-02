using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rigidbody2DTrigger : MonoBehaviour
{ 
    [Header("Events")]
    [SerializeField] private UnityEvent CollEnter2D;
    [SerializeField] private UnityEvent CollStay2D;
    [SerializeField] private UnityEvent CollExit2D;

    [SerializeField] private UnityEvent TriggEnter2D;
    [SerializeField] private UnityEvent TriggStay2D;
    [SerializeField] private UnityEvent TriggExit2D;

    [Header("Objects")]
    [SerializeField] private Collider2D targetCollider;
    private Collision2D targetCollision;
    [SerializeField] private string targetColliderTag;
    [SerializeField] private string targetCollisionTag;

    //Use these Getters and Setters for Collision with non-Trigger Colliders
    public string CollisionTag { get { return targetCollisionTag; } set { targetCollisionTag = value; } }
    public Collision2D Collision { get { return targetCollision; } set { targetCollision = value; } }

    //Use these Getters and Setters for Collision with Trigger Colliders
    public string ColliderTag { get { return targetColliderTag; } set { targetColliderTag = value; } }
    public Collider2D Collider { get { return targetCollider; } set { targetCollider = value; } }
   

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Collision = coll;
        CollisionTag = coll.gameObject.tag;
        CollEnter2D.Invoke();
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        Collision = coll;
        CollisionTag = coll.gameObject.tag;
        CollStay2D.Invoke();
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        Collision = coll;
        CollisionTag = coll.gameObject.tag;
        CollExit2D.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Collider = coll;
        ColliderTag = coll.gameObject.tag;
        TriggEnter2D.Invoke();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        Collider = coll;
        ColliderTag = coll.gameObject.tag;
        TriggStay2D.Invoke();
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        Collider = coll;
        ColliderTag = coll.gameObject.tag;
        TriggExit2D.Invoke();
    }
}
