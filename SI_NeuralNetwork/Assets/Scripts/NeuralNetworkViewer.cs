using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkViewer : MonoBehaviour
{
    public static NeuralNetworkViewer instance;

    public Gradient colorGradient;

    const float decalX = 100;
    const float decalY = 20;

    public Transform viewerGroup;

    public RectTransform neuronPrefab;
    public RectTransform axonPrefab;

    public RectTransform fitnessPrefab;
    private RectTransform fitness;

    public AgentsARD agent;

    private Image[][]   neurons;
    private Text[][]    neuronsValue;
    private Image[][][] axons;

    private RectTransform neuron;
    private RectTransform axon;
    private Text       fitnessText;

    private int   i;
    private int   x;
    private int   y;
    private int   z;
    private float posY;
    private float posZ;
    private float yAdd;
    private float zAdd;

    private void Awake()
    {
        instance = this;
    }

    public void Init(AgentsARD _agent)
    {
        agent = _agent;
        Init(agent.brain);
    }

    void Init(NeuralNetworkARD neuralNet)
    {
        for (i = viewerGroup.childCount - 1; i > -1; i--)
        {
            DestroyImmediate(viewerGroup.GetChild(i).gameObject);
        }
        
        neurons      = new Image[neuralNet.layers.Length][];
        neuronsValue = new Text[neuralNet.layers.Length][];

        for (x = 0; x < neuralNet.layers.Length; x++)
        {
            neurons[x]      = new Image[neuralNet.layers[x]];
            neuronsValue[x] = new Text[neuralNet.layers[x]];

            for (y = 0; y < neuralNet.layers[x]; y++)
            {
                if (neuralNet.layers[x] % 2 == 0)
                {
                    yAdd = 1.0f;
                }
                else
                {
                    yAdd = 0;
                }

                if (y % 2 == 0)
                {
                    posY = y + yAdd;
                }
                else
                {
                    posY = -y - 1 + yAdd;
                }

                neuron = Instantiate(neuronPrefab, transform.position, Quaternion.identity, viewerGroup);

                neuron.anchoredPosition = new Vector2(x * decalX, posY * decalY);
                neurons[x][y]           = neuron.GetComponent<Image>();

                neuronsValue[x][y] = neuron.transform.GetChild(0).GetComponent<Text>();
            }
        }
        
        axons = new Image[neuralNet.axonsNet.axons.Length][][];

        for (x = 0; x < neuralNet.axonsNet.axons.Length; x++)
        {
            axons[x] = new Image[neuralNet.axonsNet.axons[x].Length][];

            for (y = 0; y < neuralNet.axonsNet.axons[x].Length; y++)
            {
                axons[x][y] = new Image[neuralNet.axonsNet.axons[x][y].Length];

                for (z = 0; z < neuralNet.axonsNet.axons[x][y].Length; z++)
                {
                    if ((neuralNet.axonsNet.axons[x].Length) % 2 == 0)
                    {
                        yAdd = 1.0f;
                    }
                    else
                    {
                        yAdd = 0;
                    }

                    if (y % 2 == 0)
                    {
                        posY = y + yAdd;
                    }
                    else
                    {
                        posY = -y - 1 + yAdd;
                    }
                    
                    
                    if ((neuralNet.axonsNet.axons[x][y].Length) % 2 == 0)
                    {
                        zAdd = 1.0f;
                    }
                    else
                    {
                        zAdd = 0;
                    }
                    
                    if (z % 2 == 0)
                    {
                        posZ = z + zAdd;
                    }
                    else
                    {
                        posZ = -z - 1 + zAdd;
                    }

                    float midPosX = decalX                 * (x + .5f);
                    float midPosY = (posY + posZ) * decalY * .5f;

                    float zAngle = Mathf.Atan2((posY - posZ) * decalY, decalX) * Mathf.Rad2Deg;


                    axon = Instantiate(axonPrefab, transform.position, Quaternion.identity, viewerGroup);

                    axon.anchoredPosition = new Vector2(midPosX, (midPosY));
                    axon.eulerAngles      = new Vector3(0, 0, zAngle);

                    axon.sizeDelta =
                        new Vector2(new Vector2(decalX, (posY - posZ) * decalY).magnitude * 1 - 35, 2);

                    axons[x][y][z] = axon.GetComponent<Image>();
                }
            }
        }

       

        fitness = Instantiate(fitnessPrefab, transform.position, Quaternion.identity, viewerGroup);

        fitness.anchoredPosition = new Vector2(decalX * neuralNet.neurons.Length * .5f + 300, 0);

        fitnessText = fitness.GetComponent<Text>();
    }

    public void Update()
    {
        for (x = 0; x < agent.brain.neurons.Length; x++)
        {
            for (y = 0; y < agent.brain.neurons[x].Length; y++)
            {
                neurons[x][y].color     = colorGradient.Evaluate((agent.brain.neurons[x][y] + 1) * .5f);
                neuronsValue[x][y].text = agent.brain.neurons[x][y].ToString("F2");
            }
        }

        fitnessText.text = agent.fitness.ToString("F1");
    }

    public void RefreshAxon()
    {
        for (x = 0; x < agent.brain.axonsNet.axons.Length; x++)
        {
            for (y = 0; y < agent.brain.axonsNet.axons[x].Length; y++)
            {
                for (z = 0; z < agent.brain.axonsNet.axons[x][y].Length; z++)
                {
                    axons[x][y][z].color = colorGradient.Evaluate((agent.brain.axonsNet.axons[x][y][z] + 1) * .5f);
                }
            }
        }
    }
}