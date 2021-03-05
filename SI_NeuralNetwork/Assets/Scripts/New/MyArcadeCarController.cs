using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MyArcadeCarController : MonoBehaviour
{
    [Header("Wheels Pos")]
    public Suspension susp = null;
    
    [Header("Varaible")]
    private Rigidbody rb = null;
    public Transform centerOfMass = null;
    [Space(15),Min(0)]
    public float speedFactor = 200;
    public float turnFactor = 200;

    public AnimationCurve slidingCurve;
    public float roadFriction = 10f;

    [Header("Information")]
    public float verticalInput = 0;
    public float horizontalInput = 0;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        if (susp.onGround)
        {
            MotorControl(verticalInput);

            TurnControl(horizontalInput * (float)Math.Tanh(rb.velocity.magnitude * 0.1f));
            
            RoadTraction(roadFriction);
        }
    }

    private void MotorControl(float input)
    {
        Vector3 Dir = Vector3.ProjectOnPlane(rb.transform.forward, susp.NormalFromSurfrom().normalized).normalized;

        Debug.DrawRay(rb.transform.position, susp.NormalFromSurfrom().normalized * 3f, Color.yellow);
        Debug.DrawRay(rb.transform.position, Dir.normalized * 3f, Color.blue);

        float speedForce = input * speedFactor;

        if (Mathf.Abs(input) > 0)
        {
            rb.AddForce(Dir * speedForce, ForceMode.Acceleration);
        }
    }
    private void TurnControl(float input)
    {
        //Vector3 normal = susp.NormalFromSurfrom().normalized;
        Vector3 normal = rb.transform.up;

        if (Mathf.Abs(input) > 0)
        {
            rb.AddTorque(normal * input * turnFactor, ForceMode.Acceleration);
        }
    }
    
    private void RoadTraction(float groundFriction)
    {
        float dir = Vector3.Dot(rb.velocity, rb.transform.right);

        rb.AddForce(rb.transform.right * -dir * rb.mass * groundFriction);
    }

    public void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
