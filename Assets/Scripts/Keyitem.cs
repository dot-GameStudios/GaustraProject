using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyitem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Data data;
    [SerializeField] private Data PlayerData;
    [SerializeField] private Rigidbody2DTrigger RB2DTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AcquireItem(string BoolName)
    {
        if (PlayerData.GetBool(BoolName) != null && RB2DTrigger.ColliderTag == "Player"){
            SetDataBool(BoolName, true);
            Destroy(gameObject);
        }
    }

    public void SetDataBool(string BoolName, bool value)
    {
        PlayerData.GetBool(BoolName).Value = value;
    }
}
