using System;
using UnityEngine;

public class AgentsARD : MonoBehaviour
{
    [Header("Info")]
    public float fitness = 0;
    private float[] input;
    private float[] output;
    [SerializeField] private Transform nextCheckpoint = null;

    [Header("Component")]
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] CarControllerARD carController = null;
    [SerializeField] private MeshRenderer mapfeedbackRenderer;
    [SerializeField] private LayerMask layerCollidable;

    [Header("Caracteristique")]
    public NeuralNetworkARD brain;
    private RaycastHit hit;

    #region Variables Tampon
    [SerializeField] private float distanceMade = 0;
    [SerializeField] private float nextCheckPtDist = 0;
    #endregion

    public void Reset()
    {
        input = new float[brain.layers[0]];
        carController.Reset();

        fitness = 0;
        distanceMade = 0;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        nextCheckpoint = CheckPointManager.instance.checkPointsArray[0];
        nextCheckPtDist = Vector3.Magnitude(transform.position - nextCheckpoint.position);
    }

    private void FixedUpdate()
    {
        //Input
        RecupInput();

        //OutPut
        AgentOutput();

        //Fitness calc
        FitnesScoreCalc();
    }

    private void RecupInput()
    {
        
    }

    public void AgentOutput()
    {
        output = brain.ReactToIput(input);

        carController.horizontalInput = output[0];
        carController.verticalInput = output[1];
    }

    private void FitnesScoreCalc()
    {
        float tempDist = distanceMade + (nextCheckPtDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDist)
        {
            fitness = tempDist;
        }
    }

}
