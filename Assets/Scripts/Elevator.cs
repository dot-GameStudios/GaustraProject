using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    [Header("Elevator Data")]
    public float ElevatorMovementSpeed;
    public Transform StoppedAt;
    public List<Transform> StopPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        if(StoppedAt == null)
        {
            StoppedAt = StopPoints[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateElevator()
    {
        for(int i = 0; i < StopPoints.Count -1; i++)
        {
            if(StopPoints[i] == StoppedAt)
            {
                StoppedAt = StopPoints[i + 1];
                StopAllCoroutines();
                StartCoroutine(FloorChange(StoppedAt.position));
            }
        }
    }

    IEnumerator FloorChange(Vector3 newPosition)
    {
        //While the distance between Camera's current Position and the new position is greater than 0.05. Lerp between the two positions over time
        while (Vector3.Distance(transform.position, newPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, ElevatorMovementSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
