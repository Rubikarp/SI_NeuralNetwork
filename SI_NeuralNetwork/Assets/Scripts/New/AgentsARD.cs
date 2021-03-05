using System;
using UnityEngine;

public class AgentsARD : MonoBehaviour, IComparable<AgentsARD>
{
    [Header("Info")]
    public float fitness = 0;
    public float[] input;
    private float[] output;
    public Transform nextCheckpoint = null;

    [Header("Component")]
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] MyArcadeCarController car = null;
    [SerializeField] private MeshRenderer carApparence;
    [SerializeField] private MeshRenderer mapfeedbackRenderer;
    [SerializeField] private LayerMask layerCollidable;

    [Header("Caracteristique")]
    public NeuralNetworkARD brain;
    private RaycastHit hit;

    #region Variables Tampon
    [SerializeField] private float distanceMade = 0;
    [SerializeField] private float nextCheckPtDist = 0;
    #endregion

    private void Awake()
    {
        input = new float[brain.layers[0]];
    }

    public void Reset()
    {
        input = new float[brain.layers[0]];

        car.Reset();

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
        //Sensor Forward angled (Right puis Left)
        input[0] = RayHitDistRatio(car.susp.FrontRight.position, rb.transform.forward + 0.5f * rb.transform.right, 50);
        input[1] = RayHitDistRatio(car.susp.FrontLeft.position, rb.transform.forward + 0.5f * -rb.transform.right, 50);

        //Velocity
        input[2] = (float)Math.Tanh(rb.velocity.magnitude * 0.1f);

        //Appuie sur l'accélérateur
        input[3] = 1 - Mathf.Abs(car.verticalInput);

        //next trackOrientation Orientation
        input[4] = Vector3.Dot(nextCheckpoint.position-rb.position, rb.transform.forward);


    }
    public void AgentOutput()
    {
        output = brain.ReactToIput(input);

        car.horizontalInput = output[0];
        car.verticalInput = output[1];
    }
    private void FitnesScoreCalc()
    {
        float tempDist = distanceMade + (nextCheckPtDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDist)
        {
            fitness = tempDist;
        }
    }

    public void CheckpointReached(Transform checkpoint)
    {
        distanceMade += nextCheckPtDist;
        nextCheckpoint = checkpoint;
        nextCheckPtDist = Vector3.Magnitude(transform.position - nextCheckpoint.position);
    }
    public void SetColor(Color _color)
    {
        carApparence.material.color = _color;
        mapfeedbackRenderer.material.color = _color;
    }
    public int CompareTo(AgentsARD other)
    {
        return (int)(other.fitness - fitness);
    }

    //INPUT methode
    public float RayHitDistRatio(Vector3 _pos, Vector3 _dir, float lenght)
    {
        RaycastHit hit;
        //0 rien et 1 au contact
        float hitProximity = 0;

        if (Physics.Raycast(_pos, _dir, out hit, lenght, layerCollidable))
        {
            hitProximity = (lenght - hit.distance) / lenght;
        }

        Debug.DrawRay(_pos, hit.distance < 0 ? _dir * hit.distance : _dir * lenght, Color.Lerp(Color.green, Color.red, hitProximity));

        return hitProximity;
    }
    public float RoadOrientation(Rigidbody _rb, float lenght)
    {
        RaycastHit hit;

        Vector3 left = Vector3.zero;
        Vector3 right = Vector3.zero;
        Vector3 frontLeft = Vector3.zero;
        Vector3 frontRight = Vector3.zero;

        if (Physics.Raycast(_rb.position, -_rb.transform.right, out hit, lenght, layerCollidable))
        {
            left = hit.point;
        }
        if (Physics.Raycast(_rb.position, _rb.transform.right, out hit, lenght, layerCollidable))
        {
            right = hit.point;
        }
        if (Physics.Raycast(_rb.position, -_rb.transform.right + _rb.transform.forward * 0.05f, out hit, lenght, layerCollidable))
        {
            frontLeft = hit.point;
        }
        if (Physics.Raycast(_rb.position, -_rb.transform.right + _rb.transform.forward * 0.05f, out hit, lenght, layerCollidable))
        {
            frontRight = hit.point;
        }

        Vector3 leftRoad = frontLeft - left;
        Vector3 rightRoad = frontRight - right;


        return Vector3.Dot(leftRoad, rightRoad);
    }
}
