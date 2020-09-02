using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    private PolygonCollider2D Collider;
    private Rigidbody2DTrigger RB2DTrigger;
    public GameObject VirtualCam;

    public void Start()
    {
        Collider = GetComponent<PolygonCollider2D>();
        RB2DTrigger = GetComponent<Rigidbody2DTrigger>();
        
    }

    public void CameraRoomBoundary()
    {
        //Debug.Log(RB2DTrigger.Collider);
        if (RB2DTrigger.Collider.CompareTag("Player"))
        {
            SetCameraConfiner2D();
        }
    }

    public void CameraChangeBoundary(bool active)
    {
        if (RB2DTrigger.Collider.CompareTag("Player"))
        {
            SetVirtualCamActive(active);
        }
    }

    private void SetCameraConfiner2D()
    {
        CinemachineConfiner Confiner = FindObjectOfType<CinemachineConfiner>();
        Confiner.m_BoundingShape2D = Collider;
    }

    public void SetVirtualCamActive(bool active)
    {
       VirtualCam.gameObject.SetActive(active);
    }
}
