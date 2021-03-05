using System.Collections;
using System.Collections.Generic;

public class Data
{
    public List<NeuralNetworkARD> nets;

    public Data()
    {
        nets = new List<NeuralNetworkARD>();
    }

    public Data(List<NeuralNetworkARD> _nets)
    {
        nets = _nets;
    }
}


