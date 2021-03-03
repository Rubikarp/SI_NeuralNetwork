using UnityEngine;

public enum SuspensionPos { FrontRight, FrontLeft , RearRight, RearLeft }

public class Suspension : MonoBehaviour
{

    [Header("Suspension Pos")]
    public Transform FrontRight;
    public Transform FrontLeft, RearRight, RearLeft;
    
    [Header("Variable")]
    public Rigidbody rb = null;
    public LayerMask terrainLayer;
    private Vector3 suspForce = Vector3.zero;

    [Min(0)]
    public float suspStrength = 1f;

    [Range(0f, 1f)]
    public float suspHeight = 0.3f;

    private void FixedUpdate()
    {
        ApplySpringForce(FrontRight);
        ApplySpringForce(FrontLeft);
        ApplySpringForce(RearRight);
        ApplySpringForce(RearLeft);
    }

    public void ApplySpringForce(Transform susp)
    {
        float compressionRatio = RayProximity(susp.position, -susp.up, suspHeight);

        if (compressionRatio > 0)
        {
            suspForce = suspStrength * compressionRatio * susp.up;
            rb.AddForceAtPosition(suspForce, susp.position, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Retourne une float entre 0 et 1 représentant la proximité d'un élément.
    /// 0 signifie que rien n'est détecter et 1 signifie qu'on est au contact avec un élément
    /// </summary>
    public float RayProximity(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        //0 rien et 1 au contact
        float hitProximity = 0;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            hitProximity = (lenght - hit.distance) / lenght;
        }

        Debug.DrawRay(_pos, hit.distance < 0 ? _dir * hit.distance : _dir * lenght, Color.Lerp(Color.green, Color.red, hitProximity));

        return hitProximity;
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
        Vector3 frontRightImp = RayImpactPoint( FrontRight.position, -FrontRight.up, 10);
        Vector3 frontLeftImp = RayImpactPoint( FrontLeft.position, -FrontLeft.up, 10);
        Vector3 rearRightImp = RayImpactPoint( RearRight.position, -RearRight.up, 10);
        Vector3 rearLeftImp = RayImpactPoint( RearLeft.position, -RearLeft.up, 10);

        Vector3 left = frontLeftImp - rearRightImp;
        Vector3 right = frontRightImp - rearLeftImp;

        return Vector3.Cross(left, right).normalized;
    }

}
