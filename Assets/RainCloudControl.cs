using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloudControl : MonoBehaviour
{
    public Camera mainCamera;
    public float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y + yOffset);
    }
}
