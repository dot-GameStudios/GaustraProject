using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DVelocityFromData : MonoBehaviour
{
    private LayerMask GroundLayer;

    [Header("Data")]
    //[SerializeField] private DataFloat dataFloat;
    //[SerializeField] private DataFloat dataDirection;
    //[SerializeField] private DataBool Condition;

    [SerializeField] private List<DataBool> dataBools = new List<DataBool>();
    [SerializeField] private List<DataInt> dataInts = new List<DataInt>();
    [SerializeField] private List<DataFloat> dataFloats = new List<DataFloat>();
    [SerializeField] private List<DataVector2> dataVector2s = new List<DataVector2>();
    
    [Header("References")]
    [SerializeField] private Data data;
    [SerializeField] private DataInputController dataInputs;
    [SerializeField] private Rigidbody2D RB2D;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<Data>();
        RB2D = GetComponent<Rigidbody2D>();

        //Condition = data.Bool(Condition);
        //dataFloat = data.Float(dataFloat);
        //dataDirection = data.Float(dataDirection);

        for (int i = dataBools.Count - 1; i >= 0; i--)
        {
            dataBools[i] = data.GetBool(dataBools[i].Name);
        }

        for (int i = dataInts.Count - 1; i >= 0; i--)
        {
            dataInts[i] = data.GetInt(dataInts[i].Name);
        }

        for (int i = dataFloats.Count - 1; i >= 0; i--)
        {
            dataFloats[i] = data.GetFloat(dataFloats[i].Name);
        }

        for (int i = dataVector2s.Count - 1; i >= 0; i--)
        {
            dataVector2s[i] = data.GetVector2(dataVector2s[i].Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VerticalForceImpulse()
    {
        //Debug.Log(dataFloat.Name);
        //RB2D.AddForce(new Vector2(RB2D.velocity.x, (float)dataFloats[0]), ForceMode2D.Impulse);
        //RB2D.AddForce(new Vector2(RB2D.velocity.x, dataFloat.Value), ForceMode2D.Impulse);
        
    }

    public void VerticalForceImpulse(float value)
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, 0);
        //Vector2 temp = Vector2.zero;

        RB2D.AddForce(new Vector2(RB2D.velocity.x, value), ForceMode2D.Impulse);
        //RB2D.velocity.Normalize();
    }

    public void VerticalJumpCutOff(float lowJumpMultiplier)
    {
        if (RB2D.velocity.y > 0)
        {
            RB2D.velocity = new Vector2(RB2D.velocity.x, RB2D.velocity.y * lowJumpMultiplier);
        }
    }

    public void VerticalGravityAccel(float fallMultiplier)
    {
        //Fall speed that accelerates based on parameter
        if (RB2D.velocity.y < 0)
        {
            RB2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;        
        }
    }

    public void HorizontalMovement(string NodeName)
    {
        dataInputs.GetKey(NodeName);
        //RB2D.velocity = new Vector2( (float)dataFloats[0] * (float)dataFloats[1], RB2D.velocity.y);
        //RB2D.velocity = new Vector2(dataDirection.Value * dataFloat.Value, RB2D.velocity.y);
    }

    public void HorizontalMovement(float value)
    {
        RB2D.velocity = new Vector2( value, RB2D.velocity.y);
        //RB2D.velocity = new Vector2(dataDirection.Value * value, RB2D.velocity.y);
    }

    public void VertalMovement(float value)
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x,value);
        //RB2D.velocity = new Vector2(dataDirection.Value * value, RB2D.velocity.y);
    }

    public void ResetVelocityX()
    {
        RB2D.velocity = new Vector2(0.0f, RB2D.velocity.y);
    }

    public void ResetVelocityY()
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, 0.0f);
    }

    public void ResetVelocity()
    {
        RB2D.velocity = Vector2.zero;
    }

    public void SetLinearDrag(int drag)
    {
        RB2D.drag = drag;
    }
}
