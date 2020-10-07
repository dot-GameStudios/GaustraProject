using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Data data;
    [SerializeField] private Data PlayerData;
    [SerializeField] private Animator BlastDoorAnim;

    public BoxCollider2D upperCollider;
    public BoxCollider2D lowerCollider;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<Data>();
        BlastDoorAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BlastDoorAnim.SetBool("isOpen", data.GetBool("isOpen").Value);
    }

    public void CheckOpenCondition(string condition)
    {
        if(PlayerData.GetBool(condition).Value){
            data.GetBool("isOpen").Value = true;
            SetLayer(8);
            SetColliderBounds(upperCollider, 1.0f, 0.1f, 0.0f, 2.0f);
            SetColliderBounds(lowerCollider, 1.0f, 0.1f, 0.0f, -2.0f);
        }
    }

    public void SetLayer(int layerNumber)
    {
        gameObject.layer = layerNumber;
    }

    public void SetColliderBounds(BoxCollider2D coll, float x_, float y_, float xOffset_, float yOffset_)
    {
        coll.size = new Vector2(x_, y_);
        coll.offset = new Vector2(xOffset_, yOffset_);
    }
}
