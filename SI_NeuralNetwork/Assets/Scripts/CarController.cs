using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSteerAngle = 30;
    public float motorForce = 250;

    public WheelCollider wheelFrontLeftCollider, wheelFrontRightCollider, wheelRearLeftCollider, wheelRearRightCollider;
    public Transform wheelFrontLeftTransform, wheelFrontRightTransform, wheelRearLeftTransform, wheelRearRightTransform;

    public Rigidbody rb;
    public Transform centerOfMass;

    public float steeringAngle = 0;
    public float verticalInput = 0;
    public float horizontalInput = 0;

    private float actHoriInput = 0;
    public float rotInputSpeed = 0.08f;


    private void Start()
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
        steeringAngle = Mathf.Abs(actHoriInput * maxSteerAngle) > 0.1f ? actHoriInput * maxSteerAngle : 0f ;

        wheelFrontLeftCollider.steerAngle = steeringAngle;
        wheelFrontRightCollider.steerAngle = steeringAngle;
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

    private void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
