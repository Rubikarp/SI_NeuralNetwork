using System;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    [Header("Suspension Pos")]
    public Transform FrontRight;
    public Transform FrontLeft, RearRight, RearLeft;
    
    [Header("Info")]
    public Rigidbody rb = null;
    public LayerMask terrainLayer;
    public bool onGround = false;

    [Header("Suspension > WheelHeight")]
    public float suspHeight = 0.5f;
    public float wheelsHeight = 0.5f;

    public float springMultiplier = 10f;

    private void FixedUpdate()
    {
        onGround = IsOnGround();

        ApplyWheelForce(FrontRight);
        ApplyWheelForce(FrontLeft);
        ApplyWheelForce(RearRight);
        ApplyWheelForce(RearLeft);
    }

    private bool IsOnGround()
    {
        if (RayTouch(FrontRight.position, -FrontRight.up, wheelsHeight + suspHeight))
        {
            return true;
        }
        else if (RayTouch(FrontLeft.position, -FrontLeft.up, wheelsHeight + suspHeight))
        {
            return true;
        }
        else if (RayTouch(RearRight.position, -RearRight.up, wheelsHeight + suspHeight))
        {
            return true;
        }
        else if (RayTouch(RearLeft.position, -RearLeft.up, wheelsHeight + suspHeight))
        {
            return true;
        }
        return false;
    }

    public void ApplyWheelForce(Transform susp)
    {
        float compressionRatio = springCompression(susp.position + (-susp.up * wheelsHeight), -susp.up, suspHeight);

        compressionRatio = Mathf.Clamp01(compressionRatio);


        Vector3 defaultPos = susp.position + (-susp.up * (suspHeight * 0.5f));
        susp.GetChild(0).position = defaultPos + (1-compressionRatio) * (-susp.up * suspHeight * 0.8f);

    }
    public bool RayTouch(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float springCompression(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        float compression = 0;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            compression = (lenght - hit.distance) / lenght;

            Debug.DrawRay(_pos, _dir * lenght, Color.Lerp(Color.green, Color.red, compression));
        }
        else
        {
            Debug.DrawRay(_pos, _dir * lenght, Color.green);
        }


        return compression;
    }

    public Vector3 RayImpactPoint(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        //0 rien et 1 au contact
        Vector3 impactPoint = _pos + _dir * lenght;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            impactPoint = hit.point;
        }

        return impactPoint;
    }
    public Vector3 NormalFromSurfrom()
    {
        Vector3 frontRightImp = RayImpactPoint( FrontRight.position, Vector3.down, 10);
        Vector3 frontLeftImp = RayImpactPoint( FrontLeft.position, Vector3.down, 10);
        Vector3 rearRightImp = RayImpactPoint( RearRight.position, Vector3.down, 10);
        Vector3 rearLeftImp = RayImpactPoint( RearLeft.position, Vector3.down, 10);

        Vector3 left = frontLeftImp - rearRightImp;
        Vector3 right = frontRightImp - rearLeftImp;

        return Vector3.Cross(left, right).normalized;
    }

}
