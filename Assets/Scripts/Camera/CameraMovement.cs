using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //bool that sets the camera to follow the player
    [SerializeField] private bool followPlayer;
    //How fast the camera moves between boundaries
    public float screenTransitionSpeed;
    //A Box Collider which is the size of the camera. use this for boundary detection
    private BoxCollider2D cameraBox;
    public Ray testRay = new Ray();
    [Header("Boundary Data")]
    [SerializeField] private BoxCollider2D currBoundary;
    public BoxCollider2D currentBoundary { get { return currBoundary; } }
    
    public List<BoxCollider2D> Boundaries = new List<BoxCollider2D>();
    
    //Use for Clamping the Camera inside the boundaires
    private Vector3 cameraClampMax;
    private Vector3 cameraClampMin;

    //A transform for the player's position
    private Transform player;
   

    // Use this for initialization
    void Start () {

        followPlayer = true;
        cameraBox = GetComponent<BoxCollider2D>();
        AspectRatioBoxChange();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {
        //if FollowActive is true, make the camera's position equal to FollowPlayer
        if (followPlayer) { transform.position = FollowPlayer(); }
        
	}

    public void GetBoundaries(BoxCollider2D[] boundaries)
    {
        //Debug.Log(boundaries.Length);
        //Loop through the array of boundaires and set the List of Boundaries accordingly
        for (int i = 0; i < boundaries.Length; i++)
        {
            Boundaries.Add(boundaries[i]);
        }
        SetCameraBounds();
    }

    public void RemoveAllBoundaries() 
    {
        //clear the List off all Boundaries
        Boundaries.Clear();
    }

    void AspectRatioBoxChange()
    {
        //set camera Box Collider size based on aspect ratio of camera
        cameraBox.size = new Vector2(2 * Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * 2);
    }

    public void SetCameraBounds()
    {
        //set follow active to false so the camera doesn't follow the player
        followPlayer = false;

        //currBoundary = currentBoundary;
        float MaxBoundsX = 0;
        float MinBoundsX = Mathf.Infinity;
        float MaxBoundsY = 0;
        float MinBoundsY = Mathf.Infinity;

        //If the number of Boundaries per room is greater than 1
        if (Boundaries.Count > 1)
        {
            //Loop through all Boundaries
            for (int i = Boundaries.Count - 1; i >= 0; i--)
            {
                //If the Max X bound is greater than the one stored in MaxBoundsX, set it
                if (Boundaries[i].bounds.max.x > MaxBoundsX)
                {
                    MaxBoundsX = Boundaries[i].bounds.max.x;
                }
                //If the Min X bound is lesser than the one stored in MinBoundsX, set it
                if (Boundaries[i].bounds.min.x < MinBoundsX)
                {
                    MinBoundsX = Boundaries[i].bounds.min.x;
                }
                //If the current Boundary Box Collider in index i, set the MaxBoundsY and MinBoundsY to that of the boundary
                if (Boundaries[i].IsTouching(player.gameObject.GetComponent<BoxCollider2D>()))
                {
                    currBoundary = Boundaries[i];
                    MaxBoundsY = Boundaries[i].bounds.max.y;
                    MinBoundsY = Boundaries[i].bounds.min.y;
                    Debug.Log(MaxBoundsY);
                    Debug.Log(MinBoundsY);
                }
            }
        }
        else
        {
            //If there is only one Boundary per room, just set the bounds accordingly
            MaxBoundsX = Boundaries[0].bounds.max.x;
            MinBoundsX = Boundaries[0].bounds.min.x;
            MaxBoundsY = Boundaries[0].bounds.max.y;
            MinBoundsY = Boundaries[0].bounds.min.y;
        }

        //set Camera Clamp Max and Camera Clamp Min
        cameraClampMax = new Vector2(MaxBoundsX - cameraBox.size.x / 2, MaxBoundsY - cameraBox.size.y / 2);
        cameraClampMin = new Vector2(MinBoundsX + cameraBox.size.x / 2, MinBoundsY + cameraBox.size.y / 2);

        //find the position of the camera inside the new boundary
        Vector3 newPosition = FollowPlayer();

        //First stop any coroutines that are running, then call Coroutine
        StopAllCoroutines();
        StartCoroutine(BoundaryChange(newPosition));
    }

    public void SetBoundary(BoxCollider2D newBoundary) {
        //set follow active to false so the camera doesn't follow the player
        followPlayer = false;

        //set new Boundary to the boundary in parameter
        currBoundary = newBoundary;

        //set Camera Clamp Max and Camera Clamp Min
        cameraClampMax = new Vector2(currBoundary.bounds.max.x - cameraBox.size.x / 2, currBoundary.bounds.max.y - cameraBox.size.y / 2);
        cameraClampMin = new Vector2(currBoundary.bounds.min.x + cameraBox.size.x / 2, currBoundary.bounds.min.y + cameraBox.size.y / 2);

        //find the position of the camera inside the new boundary
        Vector3 newPosition = FollowPlayer();

        //Call Coroutine
        StartCoroutine(BoundaryChange(newPosition));
    }

    public void SetMultiBoxBoundary(BoxCollider2D[] boundaries, BoxCollider2D currentBoundary)
    {
       
        followPlayer = false;

        currBoundary = currentBoundary;
        float MaxBoundsX = 0;
        float MinBoundsX = Mathf.Infinity;
        
        for (int i = boundaries.Length - 1; i >=0; i--)
        {
            if(boundaries[i].bounds.max.x > MaxBoundsX)
            {
                MaxBoundsX = boundaries[i].bounds.max.x;
            }

            if(boundaries[i].bounds.min.x < MinBoundsX)
            {
                MinBoundsX = boundaries[i].bounds.min.x;
            }
            //MaxBoundsX += boundaries[i].bounds.max.x;
            //MinBoundsX += boundaries[i].bounds.min.x;
            //cameraClampMax += (boundaries[i].bounds.max.x - cameraBox.size.x / 2);
        }

        //set Camera Clamp Max and Camera Clamp Min
        cameraClampMax = new Vector2(MaxBoundsX - cameraBox.size.x / 2, currBoundary.bounds.max.y - cameraBox.size.y / 2);
        cameraClampMin = new Vector2(MinBoundsX + cameraBox.size.x / 2, currBoundary.bounds.min.y + cameraBox.size.y / 2);

        //find the position of the camera inside the new boundary
        Vector3 newPosition = FollowPlayer();

        //Call Coroutine
        StartCoroutine(BoundaryChange(newPosition));
    }
    IEnumerator BoundaryChange(Vector3 newPosition)
    {
        //While the distance between Camera's current Position and the new position is greater than 0.05. Lerp between the two positions over time
        while (Vector3.Distance(transform.position, newPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, screenTransitionSpeed * Time.deltaTime);
            yield return null;
        }
        
        followPlayer = true;
    }

    Vector3 FollowPlayer()
    {
        //if the camera is currently within a boundary
        if (currBoundary != null)
        {
            //return a Vector3
            return new Vector3(Mathf.Clamp(player.position.x, cameraClampMin.x, cameraClampMax.x), Mathf.Clamp(player.position.y, cameraClampMin.y, cameraClampMax.y), transform.position.z);
        }
        else
        {
            //return a Vector3 that is zero
            return Vector3.zero;
        }
    }

    
}
