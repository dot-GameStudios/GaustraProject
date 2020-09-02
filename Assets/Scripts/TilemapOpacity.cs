using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapOpacity : MonoBehaviour
{
    public Tilemap Renderer;
    public float AChange;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeOpacity(float opacity, float AChange)
    {
        while (Renderer.color.a != opacity) 
        { 
            if(Renderer.color.a < opacity)
            {
                Renderer.color += new Color(0, 0, 0, AChange);
            }
            else if(Renderer.color.a > opacity)
            {
                Renderer.color -= new Color(0, 0, 0, AChange);
            }
            else
            {
                yield return null;
            }
            yield return null;
        }
    }

    public void lowerOpacity(float opacity)
    {
        StartCoroutine(ChangeOpacity(opacity, AChange));

    }

    public void raiseOpacity(float opacity)
    {
        StartCoroutine(ChangeOpacity(opacity, AChange));
    }
}
