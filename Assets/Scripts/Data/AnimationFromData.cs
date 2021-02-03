using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFromData : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Data data;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<Data>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
