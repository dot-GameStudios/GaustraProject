using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    public bool isHitting, active;
    public Vector2 position;
    public float angle, length;
    public RaycastHit2D hit2D;
    public Color drawColor;
    public LayerMask collisionLayer;

    public void Initialize()
    {
        isHitting = false;
        active = true;

        if(position == null)
        {
            position = transform.position;
        }
    }

    public void Start()
    {
        Initialize();
    }

    private Vector2 AddAngleToOrientation(float angle)
    {
        return new Vector2(
            transform.rotation.x * Mathf.Cos(angle * Mathf.Deg2Rad) - transform.rotation.y * Mathf.Sin(angle * Mathf.Deg2Rad),
            transform.rotation.x * Mathf.Sin(angle * Mathf.Deg2Rad) + transform.rotation.y * Mathf.Cos(angle * Mathf.Deg2Rad)
            );
    }

    public bool rayCast(Ray ray_)
    {
        if (ray_.active)
        {
            Vector2 angle = AddAngleToOrientation(ray_.angle).normalized;
            ray_.hit2D = Physics2D.Raycast(ray_.position, angle, ray_.length, ray_.collisionLayer);
            
            print(ray_.hit2D.collider);
            if (ray_.hit2D.collider != null)
            {
                ray_.isHitting = true;
            }
            else
            {
                ray_.isHitting = false;
            }
            return ray_.isHitting;
        }
        return false;
    }
}
