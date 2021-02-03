using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float deathTimer;
    public Color AfterImageColour;

    public SpriteRenderer Renderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deathTimer -= Time.deltaTime;

        if(deathTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetColour(Color newColour)
    {
        Renderer.color = newColour;
    }
}
