using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DVelocityFromData : MonoBehaviour
{
    //[Header("Data")]

    //[SerializeField] private List<DataBool> dataBools = new List<DataBool>();
    //[SerializeField] private List<DataInt> dataInts = new List<DataInt>();
    //[SerializeField] private List<DataFloat> dataFloats = new List<DataFloat>();
    //[SerializeField] private List<DataVector2> dataVector2s = new List<DataVector2>();


    [Header("Neccesary Data")]
    //public string MoveDirection;
    public string MoveNode;
    public string JumpNode;
    public Vector2 Velocity;

    public Vector2 previousMoveDir = Vector2.zero;
    private RaycastHit2D slopeDetect;
    [Header("References")]
    [SerializeField] private Data data;
    //[SerializeField] private DataInputController dataInputs;
    [SerializeField] private Rigidbody2D RB2D;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<Data>();
        RB2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Velocity = RB2D.velocity;
    }

    public void VerticalForceImpulse(float value)
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, 0);
        //Vector2 temp = Vector2.zero;

        //RB2D.velocity.Normalize();
        RB2D.AddForce(new Vector2(0, value), ForceMode2D.Impulse);
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
    public void VerticalMovement(float value)
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, value);
    }

    public void VerticalVelocity(string VMoveNode, float value)
    {
        RB2D.velocity = new Vector2(RB2D.velocity.x, data.GetFloat(VMoveNode).Value * value);
    }

    public void HorizontalMovement(float value)
    {
        RB2D.velocity = new Vector2(value, RB2D.velocity.y);
        //RB2D.velocity = new Vector2(dataDirection.Value * value, RB2D.velocity.y);
    }

    public void HorizontalVelocity(string MoveSpeed,/* string MoveNode, */string SlopeNode, /*string SlopeCheck, */string SlopeCheck)
    {
        float PrevAngle = Vector2.Angle(previousMoveDir, Vector2.up);
        float NewAngle = Vector2.Angle(data.GetVector2(SlopeNode).Value, Vector2.up);
        //Debug.Log(NewAngle);
        //Debug.Log(PrevAngle);

        if (data.GetFloat(JumpNode).Value <= 0 && data.GetBool(SlopeCheck).Value)
        {
            RB2D.velocity = new Vector2(data.GetInt(MoveSpeed).Value * data.GetVector2(SlopeNode).Value.x * -data.GetFloat(MoveNode).Value,
                                        data.GetInt(MoveSpeed).Value * data.GetVector2(SlopeNode).Value.y * -data.GetFloat(MoveNode).Value);
        }
        else
        {
            RB2D.velocity = new Vector2(data.GetInt(MoveSpeed).Value * data.GetFloat(MoveNode).Value, RB2D.velocity.y);
        }
        if (data.GetFloat(MoveNode).Value == -1 && NewAngle > PrevAngle || data.GetFloat(MoveNode).Value == 1 && NewAngle < PrevAngle)
        {
            if(RB2D.velocity.y > 0 && data.GetFloat(JumpNode).Value <= 0)
            {
                RB2D.velocity = new Vector2(RB2D.velocity.x, RB2D.velocity.y * -1);
            }
        }


        Debug.DrawLine(RB2D.position, RB2D.position + RB2D.velocity.normalized, Color.green);

        previousMoveDir = data.GetVector2(SlopeNode).Value;
    }


    public void ResetVelocityX()
    {

        RB2D.velocity = new Vector2(0.0f, RB2D.velocity.y);

    }

    public void ResetVelocityX(string SlopeCheck)
    {
        if (data.GetBool(SlopeCheck) != null && data.GetBool(SlopeCheck).Value)
        {
            ResetVelocity();
        }
        else
        {
            RB2D.velocity = new Vector2(0.0f, RB2D.velocity.y);
        }
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

    public void SetGravityScale(int newGravity)
    {
        RB2D.gravityScale = newGravity;
    }
}
