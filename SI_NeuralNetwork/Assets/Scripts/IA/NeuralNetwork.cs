using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class NeuralNetwork
{
    //private enum Layer { Input, Hidden1, Hidden1, Output}

    public int[] layers;
    public float[][] neurons;
    public float[][][] axons;

    #region Variables Tampon
    //Valeur tampon pour des boucles for
    private int x;
    private int y;
    private int z;
    //Valeur tampon de la valeur d'un neurone
    private float value;
    #endregion

    public NeuralNetwork() { }
    public NeuralNetwork(int[] _layers)
    {
        layers = new int[_layers.Length];

        for ( x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();
    }
    private void InitNeurons()
    {
        neurons = new float[layers.Length][];

        for ( x = 0; x < layers.Length; x++)
        {
            neurons[x] = new float[layers[x]];
        }

        InitAxons();
    }
    private void InitAxons()
    {
        axons = new float[layers.Length-1][][];

        for (x = 0; x < layers.Length-1; x++)
        {
            axons[x] = new float[layers[x+1]][];

            for (y = 0; y < layers[x+1]; y++)
            {
                axons[x][y] = new float[layers[x]];

                for (z = 0; z < layers[x]; z++)
                {
                    axons[x][y][z] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }

    }
   


    public void CopyNetwork(NeuralNetwork _netCopy)
    {
        for (x = 0; x < _netCopy.axons.Length - 1; x++)
        {
            for (y = 0; y < _netCopy.axons[x].Length; y++)
            {
                for (z = 0; z < _netCopy.axons[x][y].Length; z++)
                {
                    axons[x][y][z] = _netCopy.axons[x][y][z];
                }
            }
        }
    }
    public void FeedForward(float[] inputs)
    {
        //Input Layer
        neurons[0] = inputs;

        //Hiden or Comput layer
        for (x = 1; x < neurons.Length; x++)
        {
            //Pour chaque neurons d'un couche
            for (y = 0; y < neurons[x].Length; y++)
            {
                //Reset la valeur enregistré
                value = 0;

                //Pour chaque neurone avant moi, j'additionne sa valeur multiplié par l'axons qui le lie à mon neurons actuel
                for (z = 0; z < neurons[x-1].Length; z++)
                {
                    value += neurons[x - 1][z] * axons[x-1][y][z];
                }

                //Tangent hyperbolic pour que les values ne partent pas en vrille
                neurons[x][y] = (float)Math.Tanh(value);
            }
        }
    }
    public void Mutate(float sensibility, float muteProba)
    {
        for (x = 0; x < axons.Length - 1; x++)
        {
            for (y = 0; y < axons[x].Length; y++)
            {
                for (z = 0; z < axons[x][y].Length; z++)
                {
                    axons[x][y][z] += UnityEngine.Random.Range(-sensibility, sensibility) * UnityEngine.Random.Range(0,2);
                }
            }
        }
    }

}
