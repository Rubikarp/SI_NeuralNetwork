using System.Collections;
using System.Collections.Generic;

public class Data
{
    public List<NeuralNetwork> nets;

    public Data()
    {
        nets = new List<NeuralNetwork>();
    }

    public Data(List<NeuralNetwork> _nets)
    {
        nets = _nets;
    }
}


