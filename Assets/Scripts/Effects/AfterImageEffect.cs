using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    public GameObject parent;
    public float timer;
    public float numOfAfterImages;
    public Color AfterImageColor;
    public Vector2 speedThreshold;

    private float tempTimer;
    private int imageIterator;
    private Rigidbody2D objectRigidbody;
    
    public GameObject afterImagePrefab;
    private List<GameObject> afterImages = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        tempTimer = timer;
        imageIterator = 0;
        objectRigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        tempTimer -= Time.deltaTime;

        if(tempTimer <= 0)
        {
            if (Mathf.Abs(objectRigidbody.velocity.x) >= Mathf.Abs(speedThreshold.x) || Mathf.Abs(objectRigidbody.velocity.y) >= Mathf.Abs(speedThreshold.y))
            {
                if (afterImages.Count < numOfAfterImages)
                {
                    GameObject temp = Instantiate(afterImagePrefab, parent.transform.position, parent.transform.rotation);
                    temp.GetComponent<SpriteRenderer>().sprite = parent.gameObject.GetComponent<SpriteRenderer>().sprite;
                    temp.GetComponent<AfterImage>().SetColour(AfterImageColor);
                    afterImages.Add(temp);
                    tempTimer = timer;
                }
            }
        }    

        for (int i = 0; i <= afterImages.Count - 1; i++) { 
            if(afterImages[i] == null)
            {
                afterImages.RemoveAt(i);
            }
        
        }
    }
}
