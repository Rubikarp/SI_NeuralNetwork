using System;
using UnityEngine;

public struct AxonsNet
{
    public float[][][] axons;
    public AxonsNet(int[] layers)
    {
        axons = new float[layers.Length - 1][][];

        for (int x = 0; x < layers.Length - 1; x++)
        {
            axons[x] = new float[layers[x + 1]][];

            for (int y = 0; y < layers[x + 1]; y++)
            {
                axons[x][y] = new float[layers[x]];

                for (int z = 0; z < layers[x]; z++)
                {
                    axons[x][y][z] = 1;
                }
            }
        }

    }

}

public class NeuralNetworkARD
{
    [Header("NeuralNetwork")]
    public int[] layers;
    public float[][] neurons;
    public AxonsNet axonsNet;

    [Header("MutationVariable"), Range(0f, 100f)]
    float mutPourcent;
    [Min(0f)]
    float poidValueVaria = 1f, poidIntensityVaria = 1f, poidSignChange = 1f, poidReboot = 1f;


    #region Variables Tampon
    //Valeur tampon pour des boucles for
    private int x;
    private int y;
    private int z;
    //Valeur tampon de la valeur d'un neurone
    private float value;
    private float rdmNumber;
    #endregion

    #region Constructor
    public NeuralNetworkARD() { }
    public NeuralNetworkARD(int[] _layers)
    {
        InitNeuralNetwork(_layers);
    }

    private void InitNeuralNetwork(int[] _layers)
    {
        //Layers Initaliasation
        layers = new int[_layers.Length];
        for (x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        //Neurons Initaliasation
        InitNeurons();

        //Axons Initaliasation
        axonsNet = new AxonsNet(_layers);
    }
    private void InitNeurons()
    {
        neurons = new float[layers.Length][];

        for (x = 0; x < layers.Length; x++)
        {
            neurons[x] = new float[layers[x]];
        }
    }
    #endregion

    public void CopyNetwork(NeuralNetworkARD _neuralNetCopy)
    {
        axonsNet = _neuralNetCopy.axonsNet;
    }
    public float[] ReactToIput(float[] inputs)
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
                for (z = 0; z < neurons[x - 1].Length; z++)
                {
                    value += neurons[x - 1][z] * axonsNet.axons[x - 1][y][z];
                }

                //Tangent hyperbolic pour que les values ne partent pas en vrille
                neurons[x][y] = (float)Math.Tanh(value);
            }
        }

        //Renvoie le Layer Output
        return neurons[layers.Length - 1];
    }
    
    
    public void Mutate()
    {
        if(UnityEngine.Random.Range(0f,100f) < mutPourcent)
        {
            for (x = 0; x < axonsNet.axons.Length - 1; x++)
            {
                for (y = 0; y < axonsNet.axons[x].Length; y++)
                {
                    for (z = 0; z < axonsNet.axons[x][y].Length; z++)
                    {
                        rdmNumber = UnityEngine.Random.Range(0f, poidValueVaria + poidIntensityVaria + poidSignChange + poidReboot);

                        //Evolution de la sensibilité
                        if (rdmNumber < poidValueVaria)
                        {
                            axonsNet.axons[x][y][z] += UnityEngine.Random.Range(-0.1f, 0.1f);
                        }
                        //Evolution de la intensité
                        else if (rdmNumber < poidValueVaria + poidIntensityVaria)
                        {
                            axonsNet.axons[x][y][z] *= UnityEngine.Random.Range(0f, 1f);
                        }
                        //Inverser le sens
                        else if (rdmNumber < poidValueVaria + poidIntensityVaria + poidSignChange)
                        {
                            axonsNet.axons[x][y][z] *= -1f;
                        }
                        //Reboot
                        else
                        {
                            axonsNet.axons[x][y][z] *= UnityEngine.Random.Range(-1f, 1f);
                        }
                    }
                }
            }
        }
    }

}
