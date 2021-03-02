using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Parameter")]
    public float motorForce = 250;
    [Range(5, 45)]
    public float maxSteerAngle = 30;
    [Range(0,0.1f)]
    public float rotInputSpeed = 0.08f;

    [Header("Component")]
    public Rigidbody rb;
    public Transform centerOfMass;
    [Space(15)]
    public WheelCollider wheelFrontLeftCollider;
    public WheelCollider wheelFrontRightCollider;
    public WheelCollider wheelRearLeftCollider, wheelRearRightCollider;
    [Space(15)]
    public Transform wheelFrontLeftTransform;
    public Transform wheelFrontRightTransform;
    public Transform wheelRearLeftTransform, wheelRearRightTransform;

    [Header("Information")]
    public float steeringAngle = 0;
    public float verticalInput = 0;
    public float horizontalInput = 0;
    private float actHoriInput = 0;


    private void Awake()
    {
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    void Steer()
    {
        actHoriInput = Mathf.Lerp(actHoriInput, horizontalInput, rotInputSpeed);
        steeringAngle = Mathf.Abs(actHoriInput * maxSteerAngle) > 0.1f ? actHoriInput * maxSteerAngle : 0f;

        wheelFrontLeftCollider.steerAngle = steeringAngle;
        wheelFrontRightCollider.steerAngle = steeringAngle;

        //Frition * steering angle
    }

    void Accelerate()
    {
        wheelFrontLeftCollider.motorTorque = verticalInput * motorForce;
        wheelFrontRightCollider.motorTorque = verticalInput * motorForce;
    }
    void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelFrontLeftCollider, wheelFrontLeftTransform);
        UpdateWheelPose(wheelFrontRightCollider, wheelFrontRightTransform);
        UpdateWheelPose(wheelRearLeftCollider, wheelRearLeftTransform);
        UpdateWheelPose(wheelRearRightCollider, wheelRearRightTransform);
    }

    Vector3 pos;
    Quaternion quat;
    void UpdateWheelPose(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }

    public void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
