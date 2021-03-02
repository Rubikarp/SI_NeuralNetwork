using System;
using UnityEngine;


public class Agent : MonoBehaviour, IComparable<Agent>
{
    public CarController carController = null;
    public Rigidbody rb = null;
    
    public NeuralNetwork neuralNet;
    public float fitness = 0;
    public float distanceMade = 0;
    private float tempDist;

    public float[] input;

    public float nextCheckPtDist = 0;
    public Transform nextCheckpoint = null;
    public LayerMask layerM;

    public MeshRenderer mapfeedbackRenderer;

    RaycastHit hit;
    private float rayRange = 5;

    public void Reset()
    {
        fitness = 0;
        distanceMade = 0;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        input = new float[neuralNet.layers[0]];

        carController.Reset();

        nextCheckpoint = CheckPointManager.instance.checkPointsArray[0];
        nextCheckPtDist = Vector3.Magnitude(transform.position - nextCheckpoint.position);
    }

    private void FixedUpdate()
    {
        //Input
        InputUpdate();

        //OutPut
        OutputUpdate();

        //Fitness calc
        FitnessUpdate();
    }

    public void InputUpdate()
    {

        //Velocity
        input[0] = (float)Math.Tanh(rb.velocity.magnitude * 0.1f);
        //Road Orientation
        //input[1] = NearTrackOrientation(transform);
        //AngularVelocity
        //input[2] = (float)Math.Tanh(rb.angularVelocity.magnitude * 0.05f);

        //45degré distance
        input[1] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward + transform.right, 8f);
        input[2] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward - transform.right, 8f);

        //forward wall Proximity
        input[3] = FacingNextCheckpoint(transform);
        //forward wall Proximity
        input[4] = NextDoorOrientation(transform);

        input[5] = 1f;

    }

    float NearTrackOrientation(Transform car)
    {
        Vector3 rightPos = RayHitPos(car.position, car.right + Vector3.up * 0.2f);
        Vector3 rightPosOffset = RayHitPos(car.position, car.right + car.forward * 0.25f + Vector3.up * 0.2f);

        Vector3 leftPos = RayHitPos(car.position, -car.right + Vector3.up * 0.2f);
        Vector3 leftPosOffset = RayHitPos(car.position, -car.right + car.forward * 0.25f + Vector3.up * 0.2f);

        Vector3 rightWallDir = rightPosOffset - rightPos;
        Vector3 leftWallDir = leftPosOffset - leftPos;

        return Mathf.Abs(Vector3.Dot(rightWallDir.normalized, leftWallDir.normalized));
    }
    float NextDoorOrientation(Transform car)
    {
        Vector3 nextcheckPtDir = nextCheckpoint.position - transform.position;

        return Vector3.Dot(nextcheckPtDir.normalized, car.right);
    }
    float FacingNextCheckpoint(Transform car)
    {
        Vector3 nextcheckPtDir = nextCheckpoint.position - transform.position;

        return Vector3.Dot(nextcheckPtDir.normalized, car.forward);
    }

    Vector3 RayHitPos(Vector3 _pos, Vector3 _dir)
    {
        if (Physics.Raycast(_pos, _dir, out hit, 100f * rayRange, layerM))
        {
            Debug.DrawRay(_pos, _dir * hit.distance, Color.yellow);

            return hit.point;
        }
        else
        {
            Debug.DrawRay(_pos, _dir * hit.distance, Color.red);

            return Vector3.zero;
        }
    }

    float RaySensor(Vector3 _pos, Vector3 _dir, float lenght)
    {
        if(Physics.Raycast(_pos, _dir, out hit, lenght * rayRange, layerM))
        {
            Debug.DrawRay(_pos, _dir * hit.distance, Color.Lerp(Color.red, Color.green, (lenght * rayRange - hit.distance) / (rayRange * lenght)));
            
            return (lenght * rayRange - hit.distance)/(rayRange*lenght);
        }
        else
        {
            Debug.DrawRay(_pos, _dir * hit.distance,Color.red);
            
            return 0;
        }
    }

    public void OutputUpdate()
    {
        neuralNet.FeedForward(input);

        carController.horizontalInput = neuralNet.neurons[neuralNet.layers.Length-1][0];
        carController.verticalInput = neuralNet.neurons[neuralNet.layers.Length-1][1];

    }

    void FitnessUpdate()
    {
        tempDist = distanceMade + (nextCheckPtDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDist)
        {
            fitness = tempDist;
        }
    }

    public void CheckpointReached(Transform checkpoint)
    {
        distanceMade += nextCheckPtDist;
        nextCheckpoint = checkpoint;
        nextCheckPtDist = Vector3.Magnitude(transform.position - nextCheckpoint.position) ;
    }

    public void SetColor(Color _color)
    {
        GetComponent<MeshRenderer>().material.color = _color;
        mapfeedbackRenderer.material.color = _color;
    }

    public int CompareTo(Agent other)
    {
        return (int)(other.fitness - fitness);
    }
}
