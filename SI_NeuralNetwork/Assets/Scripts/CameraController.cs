using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target = null;

    //public Transform cam = null;
    
    public Vector3 camLocalPos = Vector3.zero;
    public Vector3 localTargetLookAtPos = Vector3.zero;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.1f;

    private Vector3 wantedPos;

    void Update()
    {
        wantedPos = target.TransformPoint(camLocalPos);
        wantedPos.y = target.position.y + 2;

        transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);

        Quaternion look = Quaternion.LookRotation(target.TransformPoint(localTargetLookAtPos) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, lookLerpSpeed);
    }
}
