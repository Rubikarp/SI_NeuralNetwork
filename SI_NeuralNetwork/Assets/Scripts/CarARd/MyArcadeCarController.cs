using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyArcadeCarController : MonoBehaviour
{
    [Header("Wheels Pos")]
    public Transform suspPosAvRight = null;
    public Transform suspPosAvLeft = null;
    public Transform suspPosArrRight = null;
    public Transform suspPosArrLeft = null;
    
    [Header("Varaible")]
    private Rigidbody rb = null;
    public Transform centerOfMass = null;
    public LayerMask terrainLayer;

    public float suspPower = 1f;

    [Range(0f,1f)]
    public float suspHeight = 0.3f;

    private Vector3 suspForce = Vector3.zero;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.position;
    }

    private void FixedUpdate()
    {
        CheckSuspDist();
        rb.velocity += rb.transform.forward * Input.GetAxis("Vertical") * 50 * Time.fixedDeltaTime ;
    }

    private void CheckSuspDist()
    {
        ApplySpring(suspPosAvRight);
        ApplySpring(suspPosAvLeft);
        ApplySpring(suspPosArrRight);
        ApplySpring(suspPosArrLeft);
    }

    private void ApplySpring( Transform susp)
    {
        float compressionRatio = RayProximity(susp.position, -susp.up, suspHeight);

        suspForce = suspPower * rb.mass * compressionRatio * susp.up ;
        rb.AddForceAtPosition(suspForce, susp.position, ForceMode.Acceleration) ;

    }

    /// <summary>
    /// Retourne une float entre 0 et 1 représentant la proximité d'un élément.
    /// 0 signifie que rien n'est détecter et 1 signifie qu'on est au contact avec un élément
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_dir"></param>
    /// <param name="lenght"></param>
    /// <returns></returns>
    float RayProximity(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        //0 rien et 1 au contact
        float hitProximity = 0;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            hitProximity = (lenght - hit.distance) / lenght;
        }

        Debug.DrawRay(_pos, hit.distance < 0 ?_dir * hit.distance : _dir * lenght, Color.Lerp(Color.green, Color.red, hitProximity));

        return hitProximity;

    }
    
    float RayDist(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        float hitDist = lenght;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, terrainLayer))
        {
            hitDist = hit.distance;
        }

        Debug.DrawRay(_pos, _dir * hit.distance, Color.Lerp(Color.red, Color.green, (lenght-hitDist) / lenght));

        return 0;

    }
}
