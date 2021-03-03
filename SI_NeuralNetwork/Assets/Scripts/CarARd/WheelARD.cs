using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelARD : MonoBehaviour
{
    public Rigidbody rb;

    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    public float minLenght;
    public float maxLenght;
    public float lastLenght;
    public float springLenght;

    public float springForce;
    public float damperForce;

    private void Start()
    {
        
    }

}
