﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int popSize = 100;
    public float trainDur = 30;
    public float mutation = 5;

    public CameraController cameraController;
    public Agent agentPrefab;
    public Transform agentsParent;

    Agent agent;
    List<Agent> agents = new List<Agent>();

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        FocusFirst();

        yield return new WaitForSeconds(trainDur);

        StartCoroutine(Loop());

    }

    IEnumerator Loop()
    {
        NewGeneration();
        FocusFirst();

        yield return new WaitForSeconds(trainDur);

        StartCoroutine(Loop());

    }

    private void NewGeneration()
    {
        agents.Sort();
        AddOrRemoveAgent();
        MutateAgents();
        ResetAgents();
        SetColors();
    }

    private void SetColors()
    {
        agents[0].SetFirstColor();

        for (int i = 1; i < agents.Count/2; i++)
        {
            agents[i].SetDefaultColor();
        }

    }

    private void AddOrRemoveAgent()
    {
        if(agents.Count != popSize)
        {
            int dif = popSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();                
                }
            }
            else
            {
                for (int i = dif; i < 0; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }
    private void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentsParent);
        agent.neuralNet = new NeuralNetwork(agent.neuralNet.layers);

        agents.Add(agent);
    }
    private void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].gameObject);
        agents.RemoveAt(agents.Count - 1);
    }

    private void FocusFirst()
    {
        cameraController.target = agents[0].transform;
    }

    private void MutateAgents()
    {
        for (int i = agents.Count / 2; i < agents.Count; i++)
        {
            agents[i].neuralNet.CopyNetwork(agents[i % agents.Count / 2].neuralNet);
            agents[i].neuralNet.Mutate(0.1f, 0.5f);
            agents[i].SetMutantColor();
        }
    }
    private void ResetAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].Reset();
        }
    }


    public void EndTrain()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    public void RebootAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].neuralNet = new NeuralNetwork(agent.neuralNet.layers);
        }
        EndTrain();
    }
}
