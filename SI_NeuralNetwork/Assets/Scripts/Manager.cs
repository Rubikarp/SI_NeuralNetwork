﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int popSize = 100;
    public float trainDur = 30;
    public float mutation = 5;

    [Header("Evolve variable"),Range(0,100f)]
    public float muteProba = 10f;
    [Range(0, 0.1f)]
    public float sensiEvolve = 0.1f, sensiIntensity = 0.05f, sensiInverse = 0.03f, sensiReboot = 0.01f;


    public CameraController cameraController;
    public AgentsARD agentPrefab;
    public Transform agentsParent;

    AgentsARD agent;
    List<AgentsARD> agents = new List<AgentsARD>();

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        FocusFirst();
        InitNeuralNetView();
        yield return new WaitForSeconds(trainDur);

        StartCoroutine(Loop());

    }

    private void InitNeuralNetView()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    IEnumerator Loop()
    {
        NewGeneration();
        FocusFirst();
        InitNeuralNetView();

        yield return new WaitForSeconds(trainDur);

        StartCoroutine(Loop());

    }

    private void NewGeneration()
    {
        //Try
        agents.Sort();
        //CheckQueLaPopulation est complète
        AddOrRemoveAgent();
        //Evolve & replace agent
        MutateAgents();
        //Reset all agents
        ResetAgents();

        SetColors();
    }

    private void SetColors()
    {
        agents[0].SetColor(Color.yellow);

        float agentsHalf = agents.Count * 0.5f;
        for (int i = 1; i < agents.Count/2; i++)
        {
            agents[i].SetColor(Color.red);
        }
        for (int i = agents.Count / 2; i < agents.Count; i++)
        {
            agents[i].SetColor(Color.green);
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
        agent.brain = new NeuralNetworkARD(agent.brain.layers);

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
            agents[i].brain.CopyNetwork(agents[i % (agents.Count / 2)].brain);
            agents[i].brain.Mutate();
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
            agents[i].brain = new NeuralNetworkARD(agent.brain.layers);
        }
        EndTrain();
    }

    public void Save()
    {
        List<NeuralNetworkARD> _nets = new List<NeuralNetworkARD>();
        
            
        for (int i = 0; i < agents.Count; i++)
        {
            _nets.Add(agents[i].brain);
        }

         Data data = new Data(_nets);
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if (data != null)
        {
            if (agents.Count > data.nets.Count)
            {
                for (int i = 0; i < data.nets.Count; i++)
                {
                    agents[i].brain = data.nets[i];
                }
            }
            else
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    agents[i].brain = data.nets[i];
                }
            }
        }

        RebootAgents();
    }
}
