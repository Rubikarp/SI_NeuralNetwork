using System;
using UnityEngine;


public class Agent : MonoBehaviour, IComparable<Agent>
{
    public CarController carController = null;
    public Rigidbody rb = null;
    
    public NeuralNetwork neuralNet;
    public float fitness = 0;
    public float distanceMade = 0;
    
    public float[] input;

    public float nextCheckPtDist = 0;
    public Transform nextCheckpoint = null;
    public LayerMask layerM;

    public Material leadingMat;
    public Material defaultMat;
    public Material mutantMat;
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
        input[0] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward, 4f);
        input[1] = RaySensor(transform.position + Vector3.up * 0.2f, transform.right, 4f);
        input[2] = RaySensor(transform.position + Vector3.up * 0.2f, -transform.right, 4f);
        input[3] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward + transform.right, 4f);
        input[4] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward - transform.right, 4f);

        input[5] = (float)Math.Tanh(rb.velocity.magnitude * 0.05f);
        input[6] = (float)Math.Tanh(rb.angularVelocity.magnitude * 0.2f);
        input[7] = 1;
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

    float tempDist;
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
        nextCheckPtDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void SetFirstColor()
    {
        GetComponent<MeshRenderer>().material = leadingMat;
        mapfeedbackRenderer.material = leadingMat;
    }
    public void SetDefaultColor()
    {
        GetComponent<MeshRenderer>().material = defaultMat;
        mapfeedbackRenderer.material = defaultMat;
    }
    public void SetMutantColor()
    {
        GetComponent<MeshRenderer>().material = mutantMat;
        mapfeedbackRenderer.material = mutantMat;
    }

    public int CompareTo(Agent other)
    {
        if(fitness < other.fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
